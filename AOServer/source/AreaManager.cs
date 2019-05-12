using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AOServer.ServerAO.Data.ClientManager;
using System.IO;
using YamlDotNet;
using YamlDotNet.RepresentationModel;

namespace AOServer.ServerAO.Data
{
    class AreaManager
    {
        public class Area
        {
            public List<Client> clients;
            public int id;
            public string name;
            public string background;
            bool bg_lock;
            public int hp_def;
            public int hp_pro;
            //self.iniswap_allowed = iniswap_allowed
            //self.invite_list = {}
            //self.server = server
            //self.music_looper = None
            //self.next_message_time = 0
            //self.doc = 'No document.'
            //self.status = 'IDLE'
            //self.judgelog = []
            //self.current_music_player = ''
            //self.evi_list = EvidenceList()
            //self.is_recording = False
            //self.recorded_messages = []
            //self.evidence_mod = evidence_mod  
            string current_music;         
            public bool owned;
            public bool is_locked;
            public bool is_gmlocked;
            public bool is_modlocked;

            public Area(int area_id, string name, string background, bool bg_lock = false)
            {
                clients = new List<Client>();
                id = area_id;
                this.name = name;
                this.background = background;
                this.bg_lock = bg_lock;
                hp_def = 10;
                hp_pro = 10;

                current_music = "";

                owned = false;
                is_locked = false;
                is_gmlocked = false;
                is_modlocked = false;

            }

            public void new_client(Client c)
            {
                clients.Add(c);
            }

            public void remove_client(Client c)
            {
                clients.Remove(c);
            }

            public void unlock()
            {
                is_locked = false;
                if(!is_gmlocked && !is_modlocked)
                {
                    //invite_list = {};
                }
            }

            public void gmunlock()
            {
                is_gmlocked = false;
                is_locked = false;
                if (!is_modlocked)
                {
                    //invite_list = {};
                }
            }

            public void modunlock()
            {
                is_gmlocked = false;
                is_locked = false;
                is_modlocked = false;
                //invite_list = {};
            }

            public bool is_char_avaliable(int char_id)
            {
                foreach (var client in clients)
                {
                    if(client.char_id == char_id)
                    {
                        return false;
                    }
                }
                return true; 
            }

            public int get_rand_avail_char_id()
            {
                List<int> avail_set = new List<int>();

                for(int i = 0; i < Config.char_list.Count; i++)
                {
                    avail_set.Add(i);
                }

                foreach (var client in clients)
                {
                    avail_set.Remove(client.char_id);               
                }

                var random = new Random();
                int index = random.Next(avail_set.Count);

                return avail_set[index];
            }

            public void send_command(string cmd, string[] args)
            {
                foreach (var client in clients)
                {
                    client.send_command(cmd, args);
                }
            }

            public void send_command(string cmd, List<string> args)
            {
                foreach (var client in clients)
                {
                    client.send_command(cmd, args);
                }
            }

            public void send_host_message(string msg)
            {
                foreach (var client in clients)
                {
                    client.send_host_message(msg);
                }
            }

            public void set_next_msg_delay(int char_id)
            {

            }

            public void play_music(string name, int cid, int length = -1)
            {
                send_command("MC", new string[] { $"{name}", $"{cid}" });
            }

            public void can_send_message()
            {

            }

            public void change_hp(int side, int val)
            {

            }

            public void change_background(string bg)
            {

            }

            public void change_background_mod(string bg)
            {

            }

            public void change_status(int value)
            {

            }

            public void change_doc(string doc = "No document.")
            {

            }

            public void add_to_judgelog(Client client, string msg)
            {

            }

            public void add_music_playing(Client client, string name)
            {

            }

            public void get_evidence_list(Client client)
            {

            }

            public void broadcast_evidence_list()
            {

            }

        }

        public static List<Area> areas = new List<Area>();

        public static void load_areas()
        {

            FileStream charConfigFile = new FileStream("config/areas.yaml", FileMode.Open);
            var input = new StreamReader(charConfigFile);
            var yaml = new YamlStream();
            yaml.Load(input);

            var mapping = (YamlSequenceNode)yaml.Documents[0].RootNode;

            foreach (YamlMappingNode item in mapping)
            {
                bool au = false;
                string setName = item.Children[new YamlScalarNode("set")].ToString();
                if (item.Children[new YamlScalarNode("au")].ToString() == "true")
                    au = true;

                load_areaset(setName, au);
            }

        }

        private static void load_areaset(string setName, bool AU)
        {
            Dictionary<bool, string> sAU = new Dictionary<bool, string> { { true, "AU" }, { false, "" } };
            FileStream setConfig = new FileStream($"config/areas/{setName}.yaml", FileMode.Open);
            var input = new StreamReader(setConfig);
            var yaml = new YamlStream();
            yaml.Load(input);

            var mapping = (YamlSequenceNode)yaml.Documents[0].RootNode;

            foreach (YamlMappingNode item in mapping)
            {
                string aName = $"{areas.Count}-{item.Children[new YamlScalarNode("area")]} {sAU[AU]}";
                string aBG = item.Children[new YamlScalarNode("background")].ToString();
                //Console.WriteLine(item.Children[new YamlScalarNode("bglock")]);
                //Console.WriteLine(item.Children[new YamlScalarNode("evidence_mod")]);
                //Console.WriteLine(item.Children[new YamlScalarNode("locking_allowed")]);
                //Console.WriteLine(item.Children[new YamlScalarNode("iniswap_allowed")]);
                Area area = new Area(areas.Count, aName, aBG);
                areas.Add(area);
            }
            setConfig.Close();
        }

        public static Area default_area()
        {
            return areas[0];
        }

        public static Area get_area_by_name(string name)
        {
            foreach (var area in areas)
            {
                if(area.name == name)
                {
                    return area;
                }
            }
            return null;
        }

        public static Area get_area_by_id(int id)
        {
            foreach (var area in areas)
            {
                if (area.id == id)
                {
                    return area;
                }
            }
            return null;
        }

    }
}
