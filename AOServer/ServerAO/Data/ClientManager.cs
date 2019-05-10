using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace AOServer.ServerAO.Data
{
    class ClientManager
    {
        public class Client
        {

            public AreaManager area = null;
            string hdid = "";
            bool pm_mute = false;
            int char_id = -1;
            int ipid;
            int id;
            public WebSocket transport;
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


            public Client(WebSocket transport, int ipid, int user_id)
            {
                this.transport = transport;
                this.ipid = ipid;
                id = user_id;
            }

            public void send_raw_message(string msg)
            {
                Console.WriteLine($"Sending raw response: [{msg}]");
                transport.Write(Encoding.UTF8.GetBytes(msg));
            }

            public void send_command(string command, string[] args = null)
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
                    Console.WriteLine($"{command}#{msg}%");
                }
                else
                {
                    send_raw_message($"{command}#%");
                }

            }

            public void send_command(string command, List<string> args)
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
                    for (int i = 0; i < args.Count; i++) { msg += $"{args[i]}#"; }

                    send_raw_message($"{command}#{msg}%");
                    Console.WriteLine($"{command}#{msg}%");
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
                if (!Server.is_valid_char_id(char_id))
                {
                    //raise ClientError('Invalid Character ID.')
                }

                //if (!area.is_char_available(char_id))
                //{
                //    if(this.char_id != char_id)
                //    {
                //        if (force)
                //        {
                //            //for client in self.area.clients:
                //            //    if client.char_id == char_id:
                //            //        client.char_select()
                //        }
                //        else
                //        {
                //            //raise ClientError('Character not available.')
                //        }
                //    }
                //}  

                string old_char = get_char_name();
                this.char_id = char_id;
                pos = "";
                string[] args;

                args = new string[]{ $"{id}", "CID", $"{char_id}" };
                send_command("PV", args);
                //            logger.log_server('[{}]Changed character from {} to {}.'
                //                              .format(self.area.id, old_char, self.get_char_name()), self)
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

                //avail_char_ids = set(range(len(self.server.char_list))) - set([x.char_id for x in self.area.clients])
                //char_list = [-1] * len(self.server.char_list)

                //for x in avail_char_ids:
                //    char_list[x] = 0
                send_command("CharsCheck", Config.Chars);
                //self.send_command('HP', 1, self.area.hp_def)
                //self.send_command('HP', 2, self.area.hp_pro)
                //self.send_command('BN', self.area.background)
                //self.send_command('LE', *self.area.get_evidence_list(self))
                //self.send_command('MM', 1)
                //self.send_command('OPPASS', fantacrypt.fanta_encrypt(self.server.config['guardpass']))
                send_command("DONE");
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
            Client c = new Client(transport, 0000, clients_list.Count);
            clients_list.Add(c);
            Console.WriteLine("Client connected!");
            Thread t = new Thread(clientLoop);
            t.Start(c);
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

        public static void clientLoop(object _transport)
        {
            Client c = (Client)_transport;
            while (true)
            {
                string buffer = c.transport.Read();
                if (buffer != null)
                {
                    AOProtocol.data_received(buffer, c);
                }
            }

        }


    }
}
