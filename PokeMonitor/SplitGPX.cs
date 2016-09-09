//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Xml;
//using System.Device.Location;

//namespace PokeMonitor
//{
//    static class SplitGPX
//    {
//        private struct Point
//        {
//            public Decimal latitude;
//            public Decimal longitude;
//        }

//        public static void Run()
//        {
//            openFile();
//        }

//        private static void openFile()
//        {
//            // Create an instance of the open file dialog box.
//            OpenFileDialog dialog = new OpenFileDialog();

//            // Set filter options and filter index.
//            dialog.Filter = "GPX Files (.gpx)|*.gpx";
//            dialog.FilterIndex = 1;

//            dialog.Multiselect = false;

//            // Call the ShowDialog method to show the dialog box.
//            DialogResult userClickedOK = dialog.ShowDialog();

//            // Process input if the user clicked OK.
//            if (userClickedOK == DialogResult.OK)
//            {
//                // Open the selected file to read.
//                XmlDocument doc = new XmlDocument();
//                doc.Load(dialog.FileName);

//                XmlNodeList nodes = doc.GetElementsByTagName("trkpt");

//                List<Point> waypoints = new List<Point>();

//                foreach (XmlNode node in nodes)
//                {
//                    Point point = new Point();
//                    point.latitude = Decimal.Parse(node.Attributes["lat"]?.InnerText);
//                    point.longitude = Decimal.Parse(node.Attributes["lon"]?.InnerText);
                    
//                    waypoints.Add(point);
//                }

//                List<Point> extended = new List<Point>();
//                for (int c = 0; c < waypoints.Count; c++)
//                {
//                    Point from = waypoints[c];
//                    Point tooo = (c + 1 >= waypoints.Count) ? waypoints[0] : waypoints[c + 1];

//                    extended.Add(from);
//                    extended.AddRange(generateBetween(from, tooo));
//                }

//                System.IO.File.WriteAllText(@"D:\VMShare\autowaypoints.gpx", Generate(extended));
//                MessageBox.Show("Saved GPX file");
//            }
//        }

//        private static double DISTANCE_GAP = 5; // 10 metres
//        private static Point[] generateBetween(Point from, Point tooo)
//        {
//            GeoCoordinate frCoord = new GeoCoordinate((double) from.latitude, (double)from.longitude);
//            GeoCoordinate toCoord = new GeoCoordinate((double) tooo.latitude, (double)tooo.longitude);

//            double gapCount = Math.Floor(frCoord.GetDistanceTo(toCoord) / DISTANCE_GAP);

//            double dx = ((double)tooo.latitude - (double)from.latitude) / (gapCount + 1);
//            double dy = ((double)tooo.longitude - (double)from.longitude) / (gapCount + 1);

//            Point[] gaps = new Point[(int) gapCount];

//            for (int c = 0; c < gapCount; c++)
//            {
//                gaps[c] = new Point();
//                gaps[c].latitude = (decimal)((double)from.latitude + (c + 1) * dx);
//                gaps[c].longitude = (decimal)((double)from.longitude + (c + 1) * dy);
//            }

//            return gaps;
//        }

//        private static string Generate(List<Point> points)
//        {
//            StringBuilder sb = new StringBuilder();
//            sb.Append(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no"" ?>
//<gpx xmlns=""http://www.topografix.com/GPX/1/1"" xmlns:gpxx=""http://www.garmin.com/xmlschemas/GpxExtensions/v3"" xmlns:gpxtpx=""http://www.garmin.com/xmlschemas/TrackPointExtension/v1"" creator=""mapstogpx.com"" version=""1.1"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd http://www.garmin.com/xmlschemas/GpxExtensions/v3 http://www.garmin.com/xmlschemas/GpxExtensionsv3.xsd http://www.garmin.com/xmlschemas/TrackPointExtension/v1 http://www.garmin.com/xmlschemas/TrackPointExtensionv1.xsd"">
//  <metadata>
//    <link href=""http://www.mapstogpx.com"">
//      <text>WayPoints</text>
//    </link>
//    <time>2016-08-19T16:12:22Z</time>
//  </metadata>
//");
//            for (int c = 0; c < points.Count; c++)
//            {

//                sb.Append("  <wpt lat=\"" + points[c].latitude + "\" lon=\"" + points[c].longitude + "\"><name>" + c + "</name></wpt>\n");
//            }
//            sb.Append("</gpx>\n");
//            return sb.ToString();
//        }

//    }
//}
