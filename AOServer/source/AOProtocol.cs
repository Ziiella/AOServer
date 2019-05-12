using System;
using System.Collections.Generic;
using System.Linq;
using static AOServer.ClientManager;

namespace AOServer
{
    class AOProtocol
    {

        public static void data_received(string msg, Client c)
        {
            /*Handles any data received from the network.
            Receives data, parses them into a command and passes it
            to the command handler.
            :param data: bytes of data*/

            Command cmd = new Command(msg);
            Console.WriteLine(cmd.get_string());

            switch (cmd.get_command())
            {
                case "HI":
                    net_cmd_hi(c, cmd.get_args());
                    break;
                case "ID":
                    net_cmd_id(c, cmd.get_args());
                    break;

                case "askchaa":
                    net_cmd_askchaa(c);
                    break;

                case "RC":
                    net_cmd_rc(c);
                    break;

                case "RM":
                    net_cmd_rm(c);
                    break;

                case "RD":
                    net_cmd_rd(c);
                    break;

                case "CC":
                    net_cmd_cc(c, cmd.get_args());
                    break;

                case "CT":
                    net_cmd_ct(c, cmd.get_args());
                    break;

                case "MS":
                    net_cmd_ms(c, cmd.get_args());
                    break;

                case "MC":
                    net_cmd_mc(c, cmd.get_args());
                    break;

                default:
                    break;
            }

        }

        #region net_cmd_

        public static void net_cmd_hi(Client c, string[] args)
        {
            c.hdid = args[0];

            Logger.log_server($"Connected. HDID: {c.hdid}.");
            c.send_command(new Command("ID", c.id.ToString(), Program.get_server_software(), Program.get_version_string()));
            c.send_command(new Command("PN", (Server.get_player_count() - 1).ToString(), Config.PlayerLimit.ToString()));

        }

        public static void net_cmd_id(Client c, string[] args)
        {
            /*Client version and PV
            ID#<pv:int>#<software:string>#<version:string>#%*/

            c.is_ao2 = false;

            if (args.Count() > 2)
                return;

            //string[] version_list = args[2].Split('.');
            //
            //if len(version_list) < 3:
            //    return
            //
            //release = int(version_list[0])
            //major = int(version_list[1])
            //minor = int(version_list[2])
            //
            //if args[0] != 'AO2':
            //    return
            //if release < 2:
            //    return
            //elif release == 2:
            //    if major < 2:
            //        return
            //    elif major == 2:
            //        if minor < 5:
            //            return
            //
            if(args[0].ToLower() == "ao2")
            {
                c.is_ao2 = true;
            }
            c.send_command(new Command("FL", "yellowtext", "customobjections", "flipping", "fastloading", "noencryption", "deskmod", "evidence"));

            }

        public static void net_cmd_askchaa(Client c)
        {
            /*
             Ask for the counts of characters/evidence/music
            askchaa#%
            */

            c.send_command(new Command("SI", Config.char_list.Count.ToString(), 0.ToString(), Config.music_list.Count.ToString()));
        }

        public static void net_cmd_rc(Client c)
        {
            /*Asks for the whole character list(AO2)  AC#% */
            c.send_command(new Command("SC", Config.char_list));
        }

        public static void net_cmd_rm(Client c)
        {
            /* Asks for the whole music list(AO2) AM#% */
            c.send_command(new Command("SM", Config.music_list_ao2));        }

        public static void net_cmd_rd(Client c)
        {
            /*Asks for server metadata(charscheck, motd etc.) and a DONE#% signal(also best packet) RD#% */

            c.send_done();
            if (Config.announce_areas)
                c.send_area_list();
            c.send_motd();
        }

        public static void net_cmd_cc(Client c, string[] args)
        {
            /*
                Character selection.
                CC#<client_id:int>#<char_id:int>#<hdid:string>#%
            */

            int cid = int.Parse(args[1]);
            try
            {
                c.change_character(cid);
            }
            catch { return; }
        }

        public static void net_cmd_ct(Client c, string[] args)
        {
            /*
                OOC Message
                CT#<name:string>#<message:string>#%
            */

            if(c.name == "" || c.name != args[0])
                c.name = args[0];
            if (c.is_ooc_muted)
            {
                c.send_host_message("You have been muted by a moderator");
                return;
            }
            if (c.name == " ")
            {
                c.send_host_message("You must insert a name with at least one letter.");
                return;
            }
            if (c.name.StartsWith(Config.ServerName))
            {
                c.send_host_message("That name is reserved!");
                return;
            }
            if (c.name.Length > 15)
            {
                c.send_host_message("OOC name must be lower or equal than 15 characters.");
                return;
            }



            //Commands
            if (args[1].StartsWith("/"))
            {
                args[1] = args[1].Substring(1);
                string[] spl = args[1].Split(' ');
                CommandsOOC.ooc_command(c, spl);
            }
            else
            {
                if (c.area.is_lockdown && !c.is_mod)
                {
                    c.send_host_message("The area is currently in lockdown.");
                    return;
                }
                if (Server.is_lockdown && !c.is_mod)
                {
                    c.send_host_message("The server is currently in lockdown.");
                    return;
                }
                else if (c.disemvowel)
                {
                    args[1] = c.disemvowel_message(args[1]);
                }
                Logger.log_server($"[OOC][{c.area.id}][{c.get_char_name()}][{c.name}]{args[1]}");

                c.area.send_command(new Command("CT", c.name, args[1]));
            }
        }

        public static void net_cmd_ms(Client c, string[] args)
        {
            /*IC message.
            Refer to the implementation for details.*/

            //if self.client.is_muted:  # Checks to see if the client has been muted by a mod
            if (c.is_muted)
            {
                c.send_host_message("You have been muted by a moderator");
                return;
            }
            if (c.area.is_lockdown && !c.is_mod)
            {
                c.send_host_message("The area is currently in lockdown.");
                return;
            }
            if (Server.is_lockdown && !c.is_mod)
            {
                c.send_host_message("The server is currently in lockdown.");
                return;
            }

            if (!c.area.can_send_message())
                return;


            string msg_type = args[0];
            string pre = args[1];
            string folder = args[2];
            string anim = args[3];
            string text = args[4];
            string pos = args[5];
            string sfx = args[6];
            int anim_type = int.Parse(args[7]);
            int cid = int.Parse(args[8]);
            int sfx_delay = int.Parse(args[9]);
            int button = int.Parse(args[10]);
            int evidence = int.Parse(args[11]);
            int flip = int.Parse(args[12]);
            int ding = int.Parse(args[13]);
            int color = int.Parse(args[14]);

            //if self.client.area.is_iniswap(self.client, pre, anim, folder) and folder != self.client.get_char_name():
            //    self.client.send_host_message("Iniswap is blocked in this area")
            //    return


            if (msg_type != "chat" && msg_type != "0" && msg_type != "1")
                return;

            //if anim_type not in (0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10):
            //    return

            //if cid != self.client.char_id:
            //    return

            //if sfx_delay < 0:
            //    return

            //if button not in (0, 1, 2, 3, 4, 5, 6, 7):
            //    return

            //if evidence < 0:
            //    return

            //if ding not in (0, 1, 2):
            //    return

            //if color not in (0, 1, 2, 3, 4, 5, 6):
            //    return

            //if color == 5 and not self.client.is_mod and not self.client.is_cm:
            //    color = 0

            //if color == 6:
            //    text = re.sub(r'[^\x00-\x7F]+',' ', text) #remove all unicode to prevent now yellow text abuse
            //    if len(text.strip( ' ' )) == 1:
            //        color = 0
            //    else:
            //        if text.strip( ' ' ) in ('<num>', '<percent>', '<dollar>', '<and>'):
            //            color = 0

            if (c.pos != null)
            {
                pos = c.pos;
            }
            else if (pos != "def" && pos != "pro" && pos != "hld" && pos != "hlp" && pos != "jud" && pos != "wit")
            {
                return;
            }

            string msg = text;
            if(text.Length > 256)
                msg = text.Substring(0, 256);


            if (c.gimp)  //If you're gimped, gimp message.
            {
                msg = c.gimp_message();
            }

            if (c.disemvowel)  //If you're disemvoweled, replace string.
            {
                msg = c.disemvowel_message(msg);
            }

            c.pos = pos;

            //if evidence:
            //    if self.client.area.evi_list.evidences[self.client.evi_list[evidence] - 1].pos != 'all':
            //        self.client.area.evi_list.evidences[self.client.evi_list[evidence] - 1].pos = 'all'
            //        self.client.area.broadcast_evidence_list()


            string[] newargs;
            newargs = new string[] {  };

            c.area.send_command(new Command("MS", msg_type, pre, folder, anim, msg, pos, $"{sfx}", $"{anim_type}", $"{cid}", 
                                              $"{sfx_delay}", $"{button}", $"{evidence}", $"{flip}", $"{ding}", $"{color}"));

            Logger.log_server($"[IC][{c.area.id}][{c.get_char_name()}]{msg}");

            //self.client.area.set_next_msg_delay(len(msg))

            //if self.client.area.is_recording:
            //    self.client.area.recorded_messages.append(args)

            //if color == 2:
            //logger.log_mod('[IC][Redtext][{}][{}][{}]{}'.format(self.client.area.id, self.client.area.status,
            //                                                    self.client.get_char_name(), msg), self.client)
        }

        public static void net_cmd_mc(Client c, string[] args)
        {
            AreaManager.Area area = AreaManager.get_area_by_name(args[0]);
            if(area != null)
            {
                try
                {
                    c.change_area(area);
                }
                catch (Exceptions.ClientError)
                {
                    Console.WriteLine("Error.");
                }
                    
            }
            else
            {

                if (c.is_muted)
                {
                    c.send_host_message("You have been muted by a moderator.");
                    return;
                }
                if (!c.is_dj)
                {
                    c.send_host_message("You were blockdj\'d by a moderator.");
                    return;
                }

                //if not self.validate_net_cmd(args, self.ArgType.STR, self.ArgType.INT):
                //    return

                if (int.Parse(args[1]) != c.char_id)
                    return;
                //    return

                //if self.client.change_music_cd():
                //    self.client.send_host_message('You changed song too much times. Please try again after {} seconds.'.format(int(self.client.change_music_cd())))
                //    return

                //try:
                //    name, length = self.server.get_song_data(args[0])
                c.area.play_music(args[0], int.Parse(args[1]));
                //    self.client.area.play_music(name, self.client.char_id, length)
                //    self.client.area.add_music_playing(self.client, name)
                Logger.log_server($"[{c.area.id}][{c.get_char_name()}]Changed music to {args[0]}.");






            }

        }

        #endregion
    }
}
