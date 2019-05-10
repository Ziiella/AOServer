using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace AOServer.ServerAO.Data
{
    class ClientManager
    {
        public class Client
        {

            public AreaManager area;
            string hdid = "";
            bool pm_mute = false;
            int char_id = -1;
            int ipid;
            //self.id = user_id
            WebSocket transport;
            //Area area = server.area_manager.default_area()
            //self.evi_list = []
            //self.server = server
            public string name = "";
            string fake_name = "";
            bool is_mod = false;
            bool is_gm = false;
            bool is_dj = true;
            string pos = "";
            bool is_cm = false;
            bool disemvowel = false;
            bool remove_h = false;
            bool disemconsonant = false;
            bool gimp = false;
            bool muted_global = false;
            bool muted_adverts = false;
            bool is_muted = false;
            bool is_ooc_muted = false;
            int mod_call_time = 0;
            bool in_rp = false;
            string following = "";
            string followedby = "";

            //music flood-guard stuff
            int mus_counter = 0;
            int mute_time = 0;


            public Client(WebSocket transport, int ipid)
            {
                this.transport = transport;
                this.ipid = ipid;
            }

            public void send_raw_message(string msg)
            {
                Console.WriteLine($"Sending raw response: [{msg}]");
                transport.Write(Encoding.UTF8.GetBytes(msg), 0, 1024);
            }

            public void send_command(string command, string[] args)
            {
                if (args != null)
                {
                    if (command == "MS")
                    {
                        //        for evi_num in range(len(self.evi_list)):
                        //            if self.evi_list[evi_num] == args[11]:
                        //                lst = list(args)
                        //                lst[11] = evi_num
                        //                args = tuple(lst)
                        //                break;
                    }

                    string msg = "";
                    for (int i = 0; i < args.Length; i++) { msg += $"{args[i]}#"; }

                    send_raw_message($"{command}#{msg}%");
                }
                else
                {
                    send_raw_message($"{command}#%");
                }

            }

            public void send_host_message(string msg)
            {

            }

            public void send_motd()
            {

            }

            public bool is_valid_name()
            {

                return true;
            }

            public void disconnect()
            {
                transport.Close();
            }

            public void change_character(int char_id, bool force = false)
            {

            }

            public void change_music_cd()
            {

            }

            public void reload_character()
            {

            }

            public void change_area(AreaManager area)
            {

            }

            public void follow_user(Client arg)
            {

            }

            public void unfollow_user()
            {

            }

            public void follow_area(AreaManager area)
            {

            }

            public void send_area_list()
            {

            }

            public void get_area_info(int area_id, bool mods)
            {

            }

            public void send_area_info(int area_id, bool mods)
            {

            }

            public void send_area_hdid(int area_id)
            {

            }

            public void send_all_area_hdid()
            {

            }

            public void send_all_area_ip()
            {

            }

            public void send_done()
            {

            }

            public void char_select()
            {
                char_id = -1;
                send_done();
            }

            public void auth_mod(string password)
            {

            }

            public void auth_cm(string password)
            {

            }

            public void auth_gm(string password)
            {

            }

            public int get_ip()
            { return ipid; }

            public void get_ipreal()
            {
                
            }

            public string get_char_name()
            {
                if (char_id == -1)
                    return "CHAR_SELECT";
                return Config.Chars[char_id];
            }

            public void change_position(string pos = "")
            {
                if (pos != "" || pos != "def" || pos != "pro" || pos != "hld" || pos != "jud" || pos != "wit")
                {
                    
                    return;
                }
                this.pos = pos;
            }

            public void set_mod_call_delay()
            {
                //mod_call_time = 
            }

            public bool can_call_mod()
            {
                return false;
            }

            public string disemvowel_message(string message)
            {
                //To-Do: Disemvowel the message.
                return message;
            }

            public string gimp_message()
            {
                //To-Do: Add true gimp.
                return "gimped";
            }

        }

        public static List<Client> clients_list = new List<Client>();

        public static void new_client(WebSocket transport)
        {
            int cur_id = 0;
            Client c = new Client(transport, 0000);
            clients_list.Add(c);
            Console.WriteLine("Client connected!");
        }

        public static void remove_client(Client client)
        {

        }

        public static void get_targets(Client client, object key, string value, bool local = false)
        {

        }

        public static Client[] get_muted_clients()
        {
            Client[] clients = new Client[Config.PlayerLimit];
            return clients;
        }

        public static Client[] get_ooc_muted_clients()
        {
            Client[] clients = new Client[Config.PlayerLimit];
            return clients;
        }


    }
}
