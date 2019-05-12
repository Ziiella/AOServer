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

            public AreaManager.Area area;
            public WebSocket transport;
            public bool is_muted;
            public string hdid;
            public int char_id;
            public int ipid;
            public int id;
            public string pos;
            public bool is_ao2;
            public bool is_mod;
            public bool is_cm;
            public bool is_gm;
            public bool is_dj;


            //self.evi_list = []
            //self.server = server
            public string name = "";
            public string fake_name = "";


            
            public bool disemvowel = false;
            public bool remove_h = false;
            public bool disemconsonant = false;
            public bool gimp = false;
            public bool muted_global = false;
            public bool muted_adverts = false;

            public bool is_ooc_muted = false;
            public int mod_call_time = 0;
            public string following = "";
            public Client followedby = null;

            //music flood-guard stuff
            int mus_counter = 0;
            int mute_time = 0;


            public Client(WebSocket transport, int ipid, int user_id)
            {
                area = AreaManager.default_area();
                area.new_client(this);
                this.transport = transport;
                is_muted = false;
                hdid = "REPLACEME";
                pos = "wit";
                is_mod = false;
                is_cm = false;
                is_gm = false;
                is_dj = true;
                char_id = -1;
                id = user_id;
                this.ipid = ipid;

                
                send_command("decryptor", new string[] { $"{34}" });

            }

            public void send_raw_message(string msg)
            {
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
                }
                else
                {
                    send_raw_message($"{command}#%");
                }

            }

            public void send_host_message(string msg)
            {
                string[] args = new string[] { $"{Config.ServerName}", msg };
                send_command("CT", args);
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
                    throw new Exceptions.ClientError(this, "Invalid Character ID.");
                }

                if (!area.is_char_avaliable(char_id))
                {
                    if(this.char_id != char_id)
                    {
                        if (force)
                        {
                            foreach (var client in area.clients)
                            {
                                if(client.char_id == char_id)
                                {
                                    client.char_select();
                                }
                            }
                        }
                        else
                        {
                            throw new Exceptions.ClientError(this, "Character not available.");
                        }
                    }
                }

                //if (!area.is_char_available(char_id))
                //{
                //    if(this.char_id != char_id)
                //    {

                //    }
                //}  

                string old_char = get_char_name();
                this.char_id = char_id;
                pos = "wit";
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

            public void change_area(AreaManager.Area area)
            {
                if (this.area == area)
                {
                    throw new Exceptions.ClientError(this, "User is already in target area.");
                }
                if(area.is_locked && !is_mod && !is_gm) //and not(self.ipid in area.invite_list):
                {
                    throw new Exceptions.ClientError(this, "That area is locked!");
                }
                if (area.is_gmlocked && !is_mod && !is_gm) //and not(self.ipid in area.invite_list):
                {
                    throw new Exceptions.ClientError(this, "That area is gm-locked!");
                }
                if (area.is_gmlocked && !is_mod) //and not(self.ipid in area.invite_list):
                {
                    throw new Exceptions.ClientError(this, "That area is mod-locked!");
                }

                AreaManager.Area old_area = this.area;

                if (!area.is_char_avaliable(char_id))
                {
                    int new_char_id = area.get_rand_avail_char_id();
                    if(new_char_id == null)
                    {
                        throw new Exceptions.ClientError(this, "No available characters in that area.");
                    }
                    else
                    {
                        change_character(new_char_id);
                        send_host_message($"Character taken, switched to {get_char_name()}.");
                    }
                }

                this.area.remove_client(this);
                this.area = area;
                area.new_client(this);
                
                send_host_message($"Changed area to {area.name}.");//[{area.status}]
                //logger.log_server(
                //    '[{}]Changed area from {} ({}) to {} ({}).'.format(self.get_char_name(), old_area.name, old_area.id,
                //                                                       self.area.name, self.area.id), self)
                //#logger.log_rp(
                //#    '[{}]Changed area from {} ({}) to {} ({}).'.format(self.get_char_name(), old_area.name, old_area.id,
                //#                                                       self.area.name, self.area.id), self)

                send_command("HP", new string[] { $"{1}", $"{area.hp_def}" });
                send_command("HP", new string[] { $"{2}", $"{area.hp_pro}" });
                send_command("BN", new string[] { $"{area.background}" });
                //self.send_command('LE', *self.area.get_evidence_list(self))
                //
                if (followedby != null)
                {
                    followedby.follow_area(area);
                }
            }

            public void follow_user(Client arg)
            {

            }

            public void unfollow_user()
            {

            }

            public void follow_area(AreaManager.Area area)
            {

            }

            public void send_area_list()
            {
                string msg = "=== Areas ===";
                int i = 0;
                Dictionary<bool, string> aLock = new Dictionary<bool, string> { { true, "[LOCKED]" }, { false, "" } };

                foreach (var area in AreaManager.areas)
                {
                    i++;
                    bool locked = false;
                    string owner = "FREE";

                    if (area.owned)
                    {
                        foreach (var client in area.clients)
                        {
                            if (client.is_cm)
                            {
                                owner = $"MASTER: {client.get_char_name()}";
                                break;
                            }
                        }
                        if (area.is_gmlocked || area.is_modlocked || area.is_locked)
                        {
                            locked = true;
                        }
                        else
                        {
                            locked = false;
                        }
                    }
                    msg += $"\r\nArea {i}: {area.name} (users: {area.clients.Count}) {aLock[locked]}";
                    if (this.area == area)
                        msg += " [*]";

                }

                send_host_message(msg);

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
                send_command("CharsCheck", Config.char_list);
                send_command("HP", new string[] { $"{1}", $"{area.hp_def}" });
                send_command("HP", new string[] { $"{2}", $"{area.hp_pro}" });
                send_command("BN", new string[] { $"{area.background}" });
                //self.send_command('LE', *self.area.get_evidence_list(self))
                send_command("MM", new string[] { $"{1}" });
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
                if (is_mod)
                {
                    throw new Exceptions.ClientError(this, "Aready logged in.");
                }

                if (password == "ReplaceMe")
                {
                    is_mod = true;
                }
                else
                {
                    throw new Exceptions.ClientError(this, "Invalid password.");
                }
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
                return Config.char_list[char_id];
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

        public static List<Client> clients = new List<Client>();
        int cur_id = 0;

        public static void new_client(WebSocket transport)
        {
            
            
            Client c = new Client(transport, 0000, clients.Count);
            clients.Add(c);

            
            Thread t = new Thread(clientLoop);
            t.Start(c);
        }

        public static void remove_client(Client client)
        {
            clients.Remove(client);
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
            while (c.transport.isConnected())
            {
                string buffer = c.transport.Read();
                if (buffer != null)
                {
                    AOProtocol.data_received(buffer, c);
                }
            }
            c.disconnect();
            c.area.remove_client(c);
            remove_client(c);
        }


    }
}
