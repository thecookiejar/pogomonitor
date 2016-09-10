using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeMonitor
{
    public partial class PokeForm : Form, IUserInterface
    {
        private static List<Pokemon> defaultPokes = new List<Pokemon>
        {
            Pokemon.Snorlax,
            Pokemon.Dragonite,
            Pokemon.Charizard,
            Pokemon.Venusaur,
            Pokemon.Blastoise,
            Pokemon.Exeggutor,
            Pokemon.Lapras,

            Pokemon.Poliwrath,
            Pokemon.Arcanine,

            Pokemon.Flareon,
            Pokemon.Vaporeon,
            Pokemon.Jolteon,

            Pokemon.Gengar,
            Pokemon.Gyarados,

        };

        private static List<Pokemon> missing = new List<Pokemon>
        {
            Pokemon.Wigglytuff,
            Pokemon.Clefable,
            Pokemon.Kabutops,
            Pokemon.Alakazam,
            Pokemon.Machamp,

            Pokemon.Farfetched,
            Pokemon.Kangaskhan,
            Pokemon.MrMime,
            Pokemon.Tauros,

            //Pokemon.Ditto,
            //Pokemon.Mew,
            //Pokemon.Mewtwo,
            //Pokemon.Articuno,
            //Pokemon.Moltres,
            //Pokemon.Zapdos,
        };

        private readonly BindingList<Spawn> spawns = new BindingList<Spawn>();

        private readonly Worker worker;
        private bool isReady = true;

        private bool enableNotification = true;

        public PokeForm()
        {
            InitializeComponent();            
            listSpawns.DataSource = spawns;
            worker = new Worker(this);
            isReady = updateUI(true);

            // build listbox
            buildPokeList();
            resetPokemons();

            cbxNotification.Checked = enableNotification;

        }
        
        private void buildPokeList()
        {
            foreach (Pokemon poke in Enum.GetValues(typeof(Pokemon)))
            {
                listPokedex.Items.Add(poke, defaultPokes.Contains(poke));
            }
        }

        private void bnMissing_Click(object sender, EventArgs e)
        {
            for (int c = 0; c < listPokedex.Items.Count; c++)
            {
                listPokedex.SetItemChecked(c, listPokedex.GetItemChecked(c) || missing.Contains((Pokemon)listPokedex.Items[c]));
            }
        }

        private void bnReset_Click(object sender, EventArgs e)
        {
            resetPokemons();
        }

        private void resetPokemons()
        {
            for (int c = 0; c < listPokedex.Items.Count; c++)
            {
                listPokedex.SetItemChecked(c, defaultPokes.Contains((Pokemon)listPokedex.Items[c]));
            }
        }

        private void bnClear_Click(object sender, EventArgs e)
        {
            for (int c = 0; c < listPokedex.Items.Count; c++)
            {
                listPokedex.SetItemChecked(c, false);
            }

            spawns.Clear();
            listSpawns.DataSource = null;
            listSpawns.DataSource = spawns;
        }

        private List<Pokemon> getFilter()
        {
            List<Pokemon> selected = new List<Pokemon>();
            for (int c = listPokedex.Items.Count - 1; c >= 0; c--) 
            {
                if (listPokedex.GetItemChecked(c))
                {
                    selected.Add((Pokemon)listPokedex.Items[c]);
                }
            }
            return selected;
        }

        private void bnMain_Click(object sender, EventArgs e)
        {
            if (isReady) // can start
            {
                if (worker.IsBusy != true)
                {
                    spawns.Clear();
                    listSpawns.DataSource = null;
                    listSpawns.DataSource = spawns;

                    worker.setFilter(getFilter());
                    worker.RunWorkerAsync();
                }
            }
            else
            {
                if (worker.WorkerSupportsCancellation == true)
                {
                    worker.CancelAsync();
                }
            }

            isReady = updateUI(!isReady);
        }

        private bool updateUI(bool isReady)
        {
            bnMain.Text = isReady ? "Start" : "Cancel";
            bnReset.Enabled = bnMissing.Enabled = bnClear.Enabled = isReady;
            listPokedex.SelectionMode = isReady ? SelectionMode.One : SelectionMode.None;

            return isReady;
        }
        
        public void Update(Result results)
        {
            int currpokeId = (int)results.current;

            // remove old pokemons
            for (int c = spawns.Count - 1; c >= 0; c--)
            {
                if (spawns[c].pokemonId == currpokeId && spawns[c].isDespawned()) spawns.Remove(spawns[c]);
            }

            // insert current pokemons 
            foreach (Spawn spawn in results.spawns)
            {   
                if (!spawns.Contains(spawn) && !spawn.isDespawned())
                {
                    spawns.Insert(0, spawn);
                    spawn.Notify(enableNotification);
                }
                
            }

            listSpawns.DataSource = null;
            listSpawns.DataSource = spawns;
            
            //update label
            lblProgress.Text = "Next: " + results.next.ToString();
            
        }

        public void CancelOrError(string msg)
        {

        }

        public void Completed()
        {

        }

        private void pokeList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 2)
            {
                var list = (ListBox)sender;
                if (list.SelectedIndex >= 0)
                {
                    Spawn spawn = (Spawn)list.SelectedItem;

                    try
                    {
                        GPX.SavePokemonFile(spawn.latitude, spawn.longitude);
                       
                    }
                    catch { }
                }
            }
        }


        private void pokeList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1)
            {
                var list = (ListBox)sender;

                if (list.SelectedIndex >= 0)
                {
                    Spawn spawn = (Spawn)list.SelectedItem;

                    try
                    {
                        btnReset.Navigate(new Uri(spawn.BingMap()));
                    }
                    catch { }
                }
            }
        }

        private void listPokedex_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private string GetDeviceId()
        {
            return Properties.Settings.Default["DeviceId"].ToString();
        }

        public void EnableDirectGPX(bool enabled)
        {
            btnGPX.Enabled = enabled;
        }

        private void btnGPX_Click(object sender, EventArgs e)
        {
            //IVScanner.CheckIV();
            //GymGPX.GenerateGymGPX();
            //SplitGPX.Run();
            //generateDirectGPX();
        }

        private void cbxNotification_CheckedChanged(object sender, EventArgs e)
        {
            enableNotification = cbxNotification.Checked;
        }


        //private static readonly string[] delims = { "!4d" };

        //private void generateDirectGPX()
        //{
        //    //string clipboard = "https://www.google.com/maps/place/1%C2%B020'16.3%22N+103%C2%B045'11.6%22E/@1.33786,103.7510313,17z/data=!3m1!4b1!4m5!3m4!1s0x0:0x0!8m2!3d1.33786!4d103.75322";
        //    string clipboard = Clipboard.GetText();

        //    string uri = clipboard;

        //    int index = uri.IndexOf("!8m2!3d1", 0) + 7;
        //    string coords = uri.Substring(index, uri.Length - index);

        //    string[] latlon = coords.Split(delims, StringSplitOptions.RemoveEmptyEntries);

        //    GPX.SaveFile(Decimal.Parse(latlon[0]), Decimal.Parse(latlon[1]));
        //}


    }
}
