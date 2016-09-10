using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeMonitor
{
    //string directory = @"D:\VMShare";

    static class GPX
    {
        private static readonly decimal[,] routes;

        static GPX()
        {
            routes = new decimal[,] {
                { 0.0000000M, 0.0000000M },
                { 0.0000020M, 0.0000060M },
                { 0.0000007M, 0.0000074M },
                { 0.0000007M, 0.0000074M },
                { 0.0000000M, 0.0000060M },
                { -0.0000047M, 0.0000033M },
                { -0.0000067M, 0.0000027M },
                { -0.0000041M, -0.0000040M },
                { 0.0000000M, -0.0000074M },
                { -0.0000013M, -0.0000074M },
                { -0.0000013M, -0.0000073M },
                { -0.0000014M, -0.0000074M },
                { -0.0000006M, -0.0000074M },
                { 0.0000033M, -0.0000080M },
                { 0.0000047M, -0.0000081M },
                { 0.0000047M, -0.0000080M },
                { 0.0000080M, -0.0000061M },
                { 0.0000081M, -0.0000060M },
                { 0.0000040M, 0.0000060M },
                { 0.0000040M, 0.0000061M },
                { 0.0000020M, 0.0000067M },
                { -0.0000013M, 0.0000047M },
                { -0.0000020M, 0.0000040M },
                { -0.0000020M, 0.0000040M },
                { -0.0000020M, 0.0000040M },
                { -0.0000081M, 0.0000014M },
                { 0.0000087M, 0.0000127M },
                { -0.0000020M, 0.0000034M },
                { -0.0000060M, 0.0000013M },
                { -0.0000114M, -0.0000027M },
                { 0.0000020M, -0.0000080M },
            };
        }

        private static string Generate(decimal latitude, decimal longitude)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no"" ?>
<gpx xmlns=""http://www.topografix.com/GPX/1/1"" xmlns:gpxx=""http://www.garmin.com/xmlschemas/GpxExtensions/v3"" xmlns:gpxtpx=""http://www.garmin.com/xmlschemas/TrackPointExtension/v1"" creator=""mapstogpx.com"" version=""1.1"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd http://www.garmin.com/xmlschemas/GpxExtensions/v3 http://www.garmin.com/xmlschemas/GpxExtensionsv3.xsd http://www.garmin.com/xmlschemas/TrackPointExtension/v1 http://www.garmin.com/xmlschemas/TrackPointExtensionv1.xsd"">
  <metadata>
    <link href=""http://www.mapstogpx.com"">
      <text>Sverrir Sigmundarson</text>
    </link>
    <time>2016-08-19T16:12:22Z</time>
  </metadata>
");
            decimal dlat = latitude;
            decimal dlon = longitude;

            for (int c = 0; c < routes.GetLength(0); c++)
            {
                dlat += routes[c, 0];
                dlon += routes[c, 1];

                sb.Append("  <wpt lat=\"" + dlat + "\" lon=\"" + dlon + "\"><name>" + c + "</name></wpt>\n");
            }
            sb.Append("</gpx>\n");
            return sb.ToString();
        }

        public static void SaveFile(decimal latitude, decimal longitude, string filename)
        {
            System.IO.File.WriteAllText(Properties.Settings.Default.vmshare + filename, Generate(latitude, longitude));
        }

        public static void SavePokemonFile(decimal latitude, decimal longitude)
        {
            SaveFile(latitude, longitude, "pokemon.gpx");
            MessageBox.Show("Saved");
        }

        public static void SaveGymGPXFiles(decimal[] latitude, decimal[] longitude, string[] filenames)
        {
            if (latitude.Length == longitude.Length && latitude.Length == filenames.Length)
            {
                for (int c = 0; c < latitude.Length; c++)
                {
                    SaveFile(latitude[c], longitude[c], filenames[c]);
                }
            }
        }
    }
}
