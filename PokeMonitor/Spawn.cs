﻿using System;

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
        public void SetIVs(string ivs)
        {
            this.ivs = ivs;
        }

        public string moves = "";
        public void SetMoves(string moves)
        {
            this.moves = moves.ToLower();
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

            Pokemon poke = (Pokemon)Enum.Parse(typeof(Pokemon), pokemonId.ToString());


            if (poke == Pokemon.Dragonite ||
            poke == Pokemon.Lapras ||
            poke == Pokemon.Snorlax ||
            poke == Pokemon.Charizard ||
            poke == Pokemon.Arcanine ||
            poke == Pokemon.Poliwrath)
            {
                //DateTime now = DateTime.Now;
                //if (now.Subtract(lastNotify) > ONE_MINUTE)
                //{
                //    lastNotify = now;
                //new SoundPlayer(@"c:\Windows\Media\Alarm07.wav").Play();
                //Utils.Speak(poke.ToString());
                //}

                Utils.Speak(poke.ToString());
                notified = true;
            } 
            IVScannerPool.AddIVTask(this);
        }

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
