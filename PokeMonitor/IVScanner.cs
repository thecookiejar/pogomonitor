using System;
using System;
using System.Diagnostics;


namespace PokeMonitor
{
    static class IVScanner
    {
        private static string getArguments(int pokeId)
        {
            return @"del pogom.db & python runserver.py -a ptc -u Papakinn -p password88 -l ""1.263588,103.8223501"" -st 1 -sd 10 -ld 10 -ns -dc -ng -nk -k dummygoogle -j -iv " + pokeId.ToString();
        }
        public static void CheckIV(int pokeId)
        {

            ProcessStartInfo cmdStartInfo = new ProcessStartInfo();
            cmdStartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            cmdStartInfo.WorkingDirectory = @"E:\gitspace\Pokemon\PokeIVScanner\";
            cmdStartInfo.RedirectStandardOutput = true;
            cmdStartInfo.RedirectStandardError = true;
            //cmdStartInfo.RedirectStandardInput = true;
            cmdStartInfo.UseShellExecute = false;
            cmdStartInfo.CreateNoWindow = true;
            cmdStartInfo.Arguments = "/c " + getArguments(pokeId);
            
            Process cmdProcess = new Process();
            cmdProcess.StartInfo = cmdStartInfo;
            cmdProcess.ErrorDataReceived += cmd_Error;
            cmdProcess.OutputDataReceived += cmd_DataReceived;
            cmdProcess.EnableRaisingEvents = true;

            cmdProcess.Start();
            cmdProcess.BeginOutputReadLine();
            cmdProcess.BeginErrorReadLine();
            cmdProcess.WaitForExit();
        }
        
        static void cmd_DataReceived(object sender, DataReceivedEventArgs e)
        {
            //Console.WriteLine("Output from other process");
            Console.WriteLine(e.Data);
            if ("completed".Equals(e.Data)) ((Process)sender).Close();
        }

        static void cmd_Error(object sender, DataReceivedEventArgs e)
        {
            //Console.WriteLine("Error from other process");
            Console.WriteLine(e.Data);
        }
    }
}
