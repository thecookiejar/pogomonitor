using System;
using System.Collections.Generic;
//Request library
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace PokeMonitor
{
    class SGPokemapAPI : IPokeAPI
    {
        public static bool HAS_OFFSET = true;

        private readonly string baseURI = "https://sgpokemap.com/query3.php?since=";
        
        private readonly string requestURI;

        static SGPokemapAPI()
        {
            Spawn.Equalizer = SGPokemapAPI.Equalizer;
            Spawn.Displayer = SGPokemapAPI.Displayer;
            
        }

        public void EnableDirectGPX(IUserInterface form)
        {
            form.EnableDirectGPX(true);
        }

        public SGPokemapAPI()
        {
            requestURI = baseURI;
        }

        private string content;
        private int interval;
        private long lastSinceTime = 0;

        public Spawn[] RequestPokemon(int pokeId, string mons)
        {
            List<Spawn> pokemons = new List<Spawn>();
            try
            {
                long sinceTime = Utils.LocalToUnixTimestamp(DateTime.Now.AddMinutes(-15));
                //Console.WriteLine(pokeId + " " + lastSinceTime);

                if (sinceTime - lastSinceTime > interval)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestURI + sinceTime + "&mons=" + mons);
                    request.AutomaticDecompression = DecompressionMethods.GZip;
                    request.Host = "sgpokemap.com";
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0";
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    //request.Connection = "keep-alive";
                    //request.Referer = "https://sgpokemap.com/query3.php?since=0&mons=" + mons;
                    request.Referer = "https://sgpokemap.com/";
                    request.Headers.Add("Accept-Language", "en-US,en;q=0.5");
                    request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                    //request.Headers.Add(":authority:", "sgpokemap.com");
                    //request.Headers.Add(":method:", "GET");
                    //request.Headers.Add(":scheme:", "https");
                    //request.Headers.Add(":path:", "https");

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {

                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).",
                            response.StatusCode,
                            response.StatusDescription));

                        content = reader.ReadToEnd();

                        lastSinceTime = sinceTime;
                    }
                }                
                
                var resultObjects = AllChildren(JObject.Parse(content))
                    .First(c => c.Type == JTokenType.Array && c.Path.Contains("pokemons"))
                    .Children<JObject>();

                foreach (JObject result in resultObjects)
                {                  
                    int pokemonId = Int32.Parse(result.GetValue("pokemon_id").ToString());

                    if (pokemonId == pokeId)
                    {
                        Spawn spawn = new Spawn();
                        spawn.pokemonId = pokemonId;

                        decimal rawLatitude = Decimal.Parse(result.GetValue("lat").ToString());
                        decimal offset = HAS_OFFSET ? estimateOffset(rawLatitude) : 0;

                        spawn.latitude = rawLatitude - offset;
                        spawn.longitude = Decimal.Parse(result.GetValue("lng").ToString()) - offset;

                        spawn.endLocalTime = Utils.UnixToLocalDateTime(Int64.Parse(result.GetValue("despawn").ToString()));

                        if (isValid(spawn) && !pokemons.Contains(spawn))
                        {
                            pokemons.Add(spawn);
                        }
                    }
                }
                
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            return pokemons.ToArray();
        }

        // recursively yield all children of json
        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }

        private decimal estimateOffset(decimal latitude)
        {
            return latitude * 0.01369M -0.0061M;
        }

        //private int numberOfPokemons;
        public void PokeFilterCount(int count)
        {
            //this.numberOfPokemons = count;
            interval = 60; // 60 seconds
        }

        public int SleepTimer()
        {
            return 1000;
        }

        public bool isValid(Spawn spawn)
        {
            return true;
        }

        private static readonly TimeSpan THIRTY_SECONDS = new TimeSpan(0, 0, 0, 20);

        public static bool Equalizer(Spawn s1, Spawn s2)
        {
            return s1.pokemonId.Equals(s2.pokemonId)
                && (s1.endLocalTime.Subtract(s2.endLocalTime) < THIRTY_SECONDS)
                && (Math.Abs(s1.longitude - s2.longitude) < 0.0001M)
                && (Math.Abs(s1.latitude - s2.latitude) < 0.0001M);
        }
        
        public static string Displayer(Spawn spawn)
        {
            Pokemon poke = (Pokemon)Enum.ToObject(typeof(Pokemon), spawn.pokemonId);
            return spawn.Prefix() + poke.ToString() + " (" + spawn.TimeLeft() + ") " + spawn.ivs + " " + spawn.moves;
        }
        
    }
}
