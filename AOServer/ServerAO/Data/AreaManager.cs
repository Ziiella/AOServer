using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AOServer.ServerAO.Data.ClientManager;

namespace AOServer.ServerAO.Data
{
    class AreaManager
    {
        public int id;
        public List<Client> clients = new List<Client>();
        public string name { get; set; }
        public bool is_locked { get; set; }
        public bool is_modlocked { get; set; }
        public bool is_gmlocked { get; set; }
        public bool owned { get; set; }


    }
}
