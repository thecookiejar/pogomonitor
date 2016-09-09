using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeMonitor
{
    class Gym
    {
        internal decimal latitude;
        internal decimal longitude;
        internal string filename;

        internal Gym(decimal latitude, decimal longitude, string name)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.filename = name;
        }
    }

    static class GymGPX
    {
        private static readonly List<Gym> gyms = new List<Gym>();

        static GymGPX()
        {
            decimal[,]  gymmies = new decimal[,] {
                { 1.3228331M, 103.8198030M },
                { 1.3224905M, 103.8150394M },
                { 1.3206242M, 103.8159084M },
                { 1.3191869M, 103.8153934M },

                { 1.3161622M, 103.8159728M },
                { 1.3140492M, 103.8146961M },
                { 1.3122043M, 103.8137949M },
                { 1.3104345M, 103.8162732M },
                { 1.3071202M, 103.8186765M },
            };
            
            for (int c = 0; c < gymmies.GetLength(0); c++)
            {
                gyms.Add(new Gym(gymmies[c, 0], gymmies[c, 1], "gym" + c + ".gpx"));
            }

         }

        public static void GenerateGymGPX()
        {
            decimal[] lats = new decimal[gyms.Count];
            decimal[] lons = new decimal[gyms.Count];
            string[] files = new string[gyms.Count];

            for (int c = 0; c < gyms.Count; c++)
            {
                lats[c] = gyms[c].latitude;
                lons[c] = gyms[c].longitude;
                files[c] = gyms[c].filename;
            }

            GPX.SaveGymGPXFiles(lats, lons, files);
        }
    }
}
