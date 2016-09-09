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
        private readonly string baseURI = "http://sgpokemap.com/query.php?since=";
        
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

        public Spawn[] RequestPokemon(int pokeId)
        {
            List<Spawn> pokemons = new List<Spawn>();
            try
            {
                long sinceTime = Utils.LocalToUnixTimestamp(DateTime.Now.AddMinutes(-15));
                //Console.WriteLine(pokeId + " " + lastSinceTime);

                if (sinceTime - lastSinceTime > interval)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestURI + sinceTime);
                    request.AutomaticDecompression = DecompressionMethods.GZip;

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
                        decimal offset = estimateOffset(rawLatitude);

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
            interval = 120; // 120 seconds
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
            return poke.ToString() + " (" + spawn.TimeLeft() + ") " + spawn.ivs + " " + spawn.moves;
        }
        
    }
}
