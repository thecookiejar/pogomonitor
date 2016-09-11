using System;

namespace PokeMonitor
{
    public class Spawn : IEquatable<Spawn>, IComparable<Spawn>
    {
        public string uuid;
        public string created;
        public string downvotes;
        public string upvotes;
        public decimal latitude;
        public decimal longitude;

        public int pokemonId;
        public string trainerName;
        public string userId;
        public string deviceId;

        public DateTime endLocalTime;

        private bool despawned;

        public string ivs = "";
        public int score = -1;
        public string moves = "";
        public void SetEncounter(string ivs, int move1, int move2, int score)
        {
            this.ivs = ivs;
            this.moves = moves.ToLower();
            this.score = score;

            PokeMoves m1 = (PokeMoves)Enum.Parse(typeof(PokeMoves), move1.ToString());
            PokeMoves m2 = (PokeMoves)Enum.Parse(typeof(PokeMoves), move2.ToString());
            moves = "[" + PokemovesEval.Grade(pokemonId, move1, move2) + "] " + m1 + ":" + m2;
            
            if (score >= 90)
            {
                Pokemon poke = (Pokemon)Enum.Parse(typeof(Pokemon), pokemonId.ToString());
                Utils.Speak(poke.ToString());
            }            
        }

        public string Prefix()
        {
            if (!encountered) return "? ";

            if (score >= 90) return "*** ";

            return score >= 75 ? "+ " : "";
        }

        public bool encountered = false;
        public void EndEncounter()
        {
            encountered = true;
        }

        private static readonly TimeSpan ONE_SECOND = new TimeSpan(0, 0, 0, 1);

        public bool isDespawned()
        {
            if (!despawned)
            {
                TimeSpan diff = endLocalTime.Subtract(DateTime.Now);
                despawned = despawned || diff < ONE_SECOND;
            }
            return despawned;
        }

        public string TimeLeft()
        {
            if (despawned)
            {
                return "00:00";
            } else
            {
                return endLocalTime.Subtract(DateTime.Now).ToString(@"mm\:ss");
            }
        }
        
        //public string GoogleMap()
        //{
        //    return "https://www.google.com.sg/maps/dir/" + Coordinates() + "//@" + Coordinates() + ",13z";
        //}
        
        public string BingMap()
        {
            return "http://www.bing.com/maps/default.aspx?rtp=pos." + latitude + "_" + longitude + "&lvl=12";
        }

        public delegate string DisplayString(Spawn spawn);

        public static DisplayString Displayer;

        public override string ToString()
        {
            return Displayer(this);
        }

        public delegate bool EqualsDelegate(Spawn s1, Spawn s2);

        public static EqualsDelegate Equalizer;

        public bool Equals(Spawn p)
        {
            // If parameter is null return false:
            if (p == null)
            {
                return false;
            }

            // Return true if the fields match:
            // return (created.Equals(p.created));
            return Equalizer(this, p);
        }

        private bool notified = false;

        public void Notify(bool enabled)
        {
            if (!enabled || notified) return;

            //Pokemon poke = (Pokemon)Enum.Parse(typeof(Pokemon), pokemonId.ToString());

            //if (poke == Pokemon.Dragonite ||
            //    poke == Pokemon.Lapras ||
            //    poke == Pokemon.Snorlax ||
            //    poke == Pokemon.Charizard ||
            //    poke == Pokemon.Arcanine ||
            //    poke == Pokemon.Poliwrath)
            //{
            //    //DateTime now = DateTime.Now;
            //    //if (now.Subtract(lastNotify) > ONE_MINUTE)
            //    //{
            //    //    lastNotify = now;
            //    //new SoundPlayer(@"c:\Windows\Media\Alarm07.wav").Play();
            //    //Utils.Speak(poke.ToString());
            //    //}

            //    Utils.Speak(poke.ToString());
            notified = true;
            //}

            if (endLocalTime.Subtract(DateTime.Now) > ONE_MINUTE)
            {
                IVScannerPool.AddIVTask(this);
            } else
            {
                encountered = true; // ignore this encounter
            }
        }

        private static readonly TimeSpan ONE_MINUTE = new TimeSpan(0, 0, 1, 0);

        // Default comparer for Part type.
        public int CompareTo(Spawn spawn)
        {
            // A null value means that this object is greater.
            if (spawn == null)
                return 1;

            else
                return this.endLocalTime.CompareTo(spawn.endLocalTime);
        }


    }
}
