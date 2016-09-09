using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PokeMonitor
{
    public interface IUserInterface
    {
        // interface members
        void Update(Result results);
        void CancelOrError(string msg);
        void Completed();
        void EnableDirectGPX(bool enabled);
    }

    public struct Result
    {
        public Pokemon current;
        public Pokemon next;
        public Spawn[] spawns;
    }

    internal class Worker : BackgroundWorker
    {
        private readonly IUserInterface ui;
        private readonly List<Pokemon> filter = new List<Pokemon>();

        private readonly IPokeAPI api;

        public Worker(IUserInterface ui)
        {            
            this.ui = ui;

            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;
            this.DoWork += new DoWorkEventHandler(performTask);
            this.ProgressChanged += new ProgressChangedEventHandler(updateProgress);
            this.RunWorkerCompleted += new RunWorkerCompletedEventHandler(taskCompleted);

            api = new SGPokemapAPI();
            //api = new PokeRadarAPI();
            api.EnableDirectGPX(ui);

        }

        internal void setFilter(List<Pokemon> filter)
        {
            this.filter.Clear();
            this.filter.AddRange(filter);
            api.PokeFilterCount(filter.Count);
        }
        
        private void performTask(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            int index = 0;

            while (true)
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    Result results = new Result();

                    results.current = (Pokemon)Enum.ToObject(typeof(Pokemon), filter[index]);
                    results.spawns = api.RequestPokemon((int) results.current);

                    if (++index >= filter.Count) index = 0;
                    results.next = (Pokemon)Enum.ToObject(typeof(Pokemon), filter[index]);

                    worker.ReportProgress(0, results);
                    System.Threading.Thread.Sleep(api.SleepTimer());
                }
            }
            
        }

        
        private void updateProgress(object sender, ProgressChangedEventArgs e)
        {
            ui.Update((Result)e.UserState);
        }

        private void taskCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                ui.CancelOrError("Cancelled");
            }
            else if (!(e.Error == null))
            {
                ui.CancelOrError("Error: " + e.Error.Message);
            }
            else
            {
                ui.Completed();
            }
        }

    }
}
