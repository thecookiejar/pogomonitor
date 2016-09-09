using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace PokeMonitor
{
    static class Utils
    {
        // Converts Unix.UTC to local GMT+8 DateTime.
        private static readonly DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        public static DateTime UnixToLocalDateTime(long unixTime)
        {
            long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
            return new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc).ToLocalTime();
        }

        public static long LocalToUnixTimestamp(DateTime localTime)
        {
            return (long) (localTime.ToUniversalTime() - unixStart).TotalSeconds;
        }

        private static readonly SpeechSynthesizer synth = new SpeechSynthesizer();

        public static void Speak(string msg)
        {
            // Configure the audio output. 
            synth.SetOutputToDefaultAudioDevice();

            // Speak a string.
            synth.Speak(msg);

        }
    }
}
