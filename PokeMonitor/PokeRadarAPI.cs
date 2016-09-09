//using System;
//using System.Collections.Generic;
////Request library
//using System.Net;
//using System.IO;
//using Newtonsoft.Json.Linq;
//using System.Linq;

//namespace PokeMonitor
//{
//    class PokeRadarAPI : IPokeAPI
//    {
//        private readonly string DEVICE_ID = "64e9d7906a0b11e6b83279f0f637ba8f";

//        private readonly string baseURI = "https://www.pokeradar.io/api/v1/submissions?";
//        private readonly string minLatitude = "1.2602362";
//        private readonly string maxLatitude = "1.4761245198527968";
//        private readonly string minLongitude = "103.63454819";
//        private readonly string maxLongitude = "104.00568008";
        
//        private readonly string requestURI;

//        static PokeRadarAPI()
//        {
//            Spawn.Equalizer = PokeRadarAPI.Equalizer;
//            Spawn.Displayer = PokeRadarAPI.Displayer;
//        }

//        public void EnableDirectGPX(IUserInterface form)
//        {
//            form.EnableDirectGPX(false);
//        }

//        public PokeRadarAPI()
//        {
//            requestURI = baseURI + "deviceId=" + DEVICE_ID;
//            requestURI += "&minLatitude=" + minLatitude;
//            requestURI += "&maxLatitude=" + maxLatitude;
//            requestURI += "&minLongitude=" + minLongitude;
//            requestURI += "&maxLongitude=" + maxLongitude;
//            requestURI += "&pokemonId=";
//        }

//        public Spawn[] RequestPokemon(int pokeId)
//        {
//            List<Spawn> pokemons = new List<Spawn>();
//            try
//            {
//                string content;
//                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestURI + pokeId.ToString());
//                request.AutomaticDecompression = DecompressionMethods.GZip;
                
//                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
//                using (Stream stream = response.GetResponseStream())
//                using (StreamReader reader = new StreamReader(stream))
//                {

//                    if (response.StatusCode != HttpStatusCode.OK)
//                        throw new Exception(String.Format(
//                        "Server error (HTTP {0}: {1}).",
//                        response.StatusCode,
//                        response.StatusDescription));

//                    content = reader.ReadToEnd();
//                }

//                var resultObjects = AllChildren(JObject.Parse(content))
//                    .First(c => c.Type == JTokenType.Array && c.Path.Contains("data"))
//                    .Children<JObject>();

//                foreach (JObject result in resultObjects)
//                {                  
//                    Spawn spawn = new Spawn();
//                    spawn.uuid = result.GetValue("id").ToString();
//                    spawn.created = result.GetValue("created").ToString();
//                    spawn.downvotes = result.GetValue("downvotes").ToString();
//                    spawn.upvotes = result.GetValue("upvotes").ToString();

//                    spawn.latitude = Decimal.Parse(result.GetValue("latitude").ToString());
//                    spawn.longitude = Decimal.Parse(result.GetValue("longitude").ToString());

//                    spawn.pokemonId = Int32.Parse(result.GetValue("pokemonId").ToString());
//                    spawn.trainerName = result.GetValue("trainerName").ToString();
//                    spawn.userId = result.GetValue("userId").ToString();
//                    spawn.deviceId = result.GetValue("deviceId").ToString();

//                    spawn.endLocalTime = Utils.UnixToLocalDateTime(Int64.Parse(spawn.created)).AddMinutes(15);

//                    if (isValid(spawn) && !pokemons.Contains(spawn))
//                    {
//                        pokemons.Add(spawn);
//                    }
//                }
                
//            } catch (Exception e) {
//                Console.WriteLine(e.Message);
//            }

//            return pokemons.ToArray();
//        }

//        // recursively yield all children of json
//        private static IEnumerable<JToken> AllChildren(JToken json)
//        {
//            foreach (var c in json.Children())
//            {
//                yield return c;
//                foreach (var cc in AllChildren(c))
//                {
//                    yield return cc;
//                }
//            }
//        }
        
//        public void PokeFilterCount(int count)
//        {

//        }

//        public int SleepTimer()
//        {
//            return 5000;
//        }

//        public bool isValid(Spawn spawn)
//        {
//            return spawn.deviceId.Equals("80sxy0vumg2h5hhv8hgc0axt9jr29al7") && spawn.userId.Equals("13661365") && spawn.trainerName.Equals("(Poke Radar Prediction)");
//        }

//        public static bool Equalizer(Spawn s1, Spawn s2)
//        {
//            return s1.created.Equals(s2.created);
//        }
        
//        public static string Displayer(Spawn spawn)
//        {
//            Pokemon poke = (Pokemon)Enum.ToObject(typeof(Pokemon), spawn.pokemonId);
//            int totalVotes = Int32.Parse(spawn.upvotes) + Int32.Parse(spawn.downvotes);

//            return poke.ToString() + " (" + spawn.TimeLeft() + ") " + spawn.upvotes + "/" + totalVotes;
//        }

//    }
//}
