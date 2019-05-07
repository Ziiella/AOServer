using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOServer.ServerAO.Data
{
    class Area
    {
        public List<Client> clients = new List<Client>();
        public string name { get; set; }
        public bool is_locked { get; set; }
        public bool is_modlocked { get; set; }
        public bool is_gmlocked { get; set; }
        public bool owned { get; set; }


    }
}
