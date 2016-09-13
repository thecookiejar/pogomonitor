using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeMonitor
{
    interface IPokeAPI
    {
        void EnableDirectGPX(IUserInterface form);
        int SleepTimer();
        Spawn[] RequestPokemon(int pokeId, string mons);
        void PokeFilterCount(int count);
    }
}
