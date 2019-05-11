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
                return true; 
            }

            public int get_rand_avail_char_id()
            {
                return 0;
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

            public void send_host_message(int char_id)
            {

            }

            public void set_next_msg_delay(int char_id)
            {

            }

            public void play_music(string name, int cid)
            {

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
            Area area = new Area(0, "test", "test");
            areas.Add(area);
            area = new Area(1, "test2", "test");
            areas.Add(area);
            area = new Area(2, "test3", "test");
            areas.Add(area);

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
