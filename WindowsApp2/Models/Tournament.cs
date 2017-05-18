using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsApp2.Models
{
    public class TournamentModel
    {
        public string Name { get; set; }
        public string Server { get; set; }
        public int NofPlayers { get; set; }
        public int TeamSize { get; set; }
        public string Mode { get; set; }
        public string EliminationMode { get; set; }
        public string Map { get; set; }

        public TournamentModel(string name, string server, int nofplayers, int teamsize, string mode, string eliminationmode, string map)
        {
            Name = name;
            Server = server;
            NofPlayers = nofplayers;
            TeamSize = teamsize;
            Mode = mode;
            EliminationMode = eliminationmode;
            Map = map;
        }
    }
}
