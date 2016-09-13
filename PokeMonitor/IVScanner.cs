using System;
using System.Threading;
using System.Diagnostics;

using System.Collections.Generic;

namespace PokeMonitor
{
    public class IVScannerPool
    {
        static Object locker = new object();

        private static readonly List<IVScanner> manager = new List<IVScanner>();

        static IVScannerPool()
        {
            //ThreadPool.SetMaxThreads(1, 1);
        }

        private static void push(IVScanner scanner)
        {
            lock(locker)
            {
                manager.Add(scanner);

                if (manager.Count == 1)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc), manager[0]);
                }
            }
        }

        private static void pop(IVScanner scanner)
        {
            lock (locker)
            {
                manager.Remove(scanner);
                if (manager.Count > 0)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc), manager[0]);
                }
            }

        }

        public static void AddIVTask(Spawn spawn)
        {
            push(new IVScanner(spawn));
        }

        // Wrapper method for use with thread pool.
        static void ThreadProc(Object stateInfo)
        {
            //Console.WriteLine("..... chceking iv");
            IVScanner scanner = (IVScanner)stateInfo;
            scanner.CheckIV();
            Thread.Sleep(500);
            pop(scanner);
        }
    }

    class IVScanner
    {   
        //private ManualResetEvent doneEvent;
        private Spawn spawn;

        public IVScanner(Spawn spawn)
        {
            this.spawn = spawn;
        }

        public void CheckIV()
        {
            if (!spawn.isDespawned())
            {
                Process cmdProcess = new Process();
                cmdProcess.StartInfo = getStartInfo(getArguments(spawn.pokemonId, spawn.latitude, spawn.longitude));
                cmdProcess.ErrorDataReceived += cmd_Error;
                cmdProcess.OutputDataReceived += cmd_DataReceived;
                cmdProcess.EnableRaisingEvents = true;

                cmdProcess.Start();
                cmdProcess.BeginOutputReadLine();
                cmdProcess.BeginErrorReadLine();
                cmdProcess.WaitForExit();
            }
        }
        
        //private static readonly double baseIV = (double) 100.0f / Math.Round(15 * Math.Sqrt(15 * 15), 0);

        void cmd_DataReceived(object sender, DataReceivedEventArgs e)
        {
            if ("completed".Equals(e.Data))
            {
                spawn.EndEncounter();
                ((Process)sender).Close();
            }

            if (e.Data != null && e.Data.StartsWith("results:")) {
                string[] v = e.Data.Split(':');

                int pokeId = Int32.Parse(v[1]);
                decimal latitude = Decimal.Parse(v[3]);
                decimal longitude = Decimal.Parse(v[4]);
                
                if (pokeId == spawn.pokemonId 
                    && Math.Abs(latitude - spawn.latitude) < 0.0001M
                    && Math.Abs(longitude - spawn.longitude) < 0.0001M)
                {
                    int atk = Int32.Parse(v[5]);
                    int def = Int32.Parse(v[6]);
                    int sta = Int32.Parse(v[7]);

                    int score = Pokestats.CalcAltIV(pokeId, atk, def, sta);

                    string ivs = v[5] + ":" + v[6] + ":" + v[7] + " [" + score + "%] ";


                    spawn.SetEncounter(ivs, Int32.Parse(v[8]), Int32.Parse(v[9]), score);
                }
            }            
        }

        void cmd_Error(object sender, DataReceivedEventArgs e)
        {
            // do nothing.
            //Console.WriteLine(e.Data);
        }

        private static readonly string username = Properties.Settings.Default.username;
        private static readonly string password = Properties.Settings.Default.password;

        private static string getArguments(int pokeId, decimal latitude, decimal longitude)
        {
            string latlong = @"""" + latitude + "," + longitude + @"""";

            string args = "del pogom.db & python runserver.py -a ptc -u " + username + " -p " + password + " -l " + latlong + " -st 1 -sd " +
                Properties.Settings.Default.pgo_scan_delay + " -ld " + Properties.Settings.Default.pgo_login_delay + " -ns -dc -ng -nk -k dummygoogle -j -iv " + pokeId.ToString();
            //Console.WriteLine(args);
            return args;
        }

        private static ProcessStartInfo getStartInfo(string arguments)
        {
            
            ProcessStartInfo cmdStartInfo = new ProcessStartInfo();
            cmdStartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            cmdStartInfo.WorkingDirectory = Properties.Settings.Default.pogomap_dir;
            cmdStartInfo.RedirectStandardOutput = true;
            cmdStartInfo.RedirectStandardError = true;
            //cmdStartInfo.RedirectStandardInput = true;
            cmdStartInfo.UseShellExecute = false;
            cmdStartInfo.CreateNoWindow = true;
            cmdStartInfo.Arguments = "/c " + arguments;

            return cmdStartInfo;
        }

        
    }
}
