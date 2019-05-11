using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AOServer.ServerAO.Data.ClientManager;
using AOServer.ServerAO.Data;
using AOServer.ServerAO;

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

            if (msg.StartsWith("#"))
            {
                msg = msg.Substring(1);
                string[] spl = msg.Split('#');
                msg = $"{FantaCrypt.fanta_decrypt(spl[0])}#{spl[1]}#%";
            }
            Console.WriteLine(msg);
            string[] data = get_messages(msg);

            switch (data[0])
            {
                case "HI":
                    net_cmd_hi(c, data);
                    break;
                case "ID":
                    net_cmd_id(c, data);
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
                    net_cmd_cc(c, data);
                    break;

                case "MS":
                    net_cmd_ms(c, data);
                    break;

                case "MC":
                    net_cmd_mc(c, data);
                    break;

                default:
                    break;
            }

        }


        public static string[] get_messages(string data)
        {
            string[] args = data.Split('#');
            return args;
        }



        #region net_cmd_

        public static void net_cmd_hi(Client c, string[] args)
        {
            c.hdid = args[1];
            c.send_command("ID", new string[] { $"{c.id}", $"{Program.get_server_software()}", $"{Program.get_version_string()}" });
            c.send_command("PN", new string[] { $"{Server.get_player_count() - 1}", $"{Config.PlayerLimit}" });
        }

        public static void net_cmd_id(Client c, string[] args)
        {
            /*Client version and PV
            ID#<pv:int>#<software:string>#<version:string>#%*/

            c.is_ao2 = false;

            if (args.Count() > 3)
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
            if(args[1].ToLower() == "ao2")
            {
                c.is_ao2 = true;
            }

            c.send_command("FL", new string[] { "yellowtext", "customobjections", "flipping", "fastloading", "noencryption", "deskmod", "evidence" });
            }

        public static void net_cmd_askchaa(Client c)
        {
            /*Ask for the counts of characters/evidence/music
            askchaa#%*/
            string[] args = new string[3];

            args[0] = ServerAO.Config.char_list.Count.ToString(); //Char count
            args[1] = 0.ToString(); //Evidence count
            args[2] = ServerAO.Config.music_list.Count.ToString(); //Music Count
            c.send_command("SI", args);
        }

        public static void net_cmd_rc(Client c)
        {
            /*Asks for the whole character list(AO2)  AC#% */
            c.send_command("SC", ServerAO.Config.char_list);
        }

        public static void net_cmd_rm(Client c)
        {
            /* Asks for the whole music list(AO2) AM#% */
            c.send_command("SM", ServerAO.Config.music_list_ao2);
        }

        public static void net_cmd_rd(Client c)
        {
            /*Asks for server metadata(charscheck, motd etc.) and a DONE#% signal(also best packet) RD#% */

            c.send_done();
            if (ServerAO.Config.announce_areas)
                c.send_area_list();
            c.send_motd();
        }

        public static void net_cmd_cc(Client c, string[] args)
        {
            /*Character selection.
            //CC#<client_id:int>#<char_id:int>#<hdid:string>#%
            */

            //if not self.validate_net_cmd(args, self.ArgType.INT, self.ArgType.INT, self.ArgType.STR, needs_auth=False):
            //    return
            int cid = int.Parse(args[2]);
            //try:
            c.change_character(cid);
            //except ClientError:
            //    return
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
            
            //if not self.client.area.can_send_message():
            //    return


            //if not self.validate_net_cmd(args, self.ArgType.STR, self.ArgType.STR_OR_EMPTY, self.ArgType.STR,
            //                             self.ArgType.STR,
            //                             self.ArgType.STR, self.ArgType.STR, self.ArgType.STR, self.ArgType.INT,
            //                             self.ArgType.INT, self.ArgType.INT, self.ArgType.INT, self.ArgType.INT,
            //                             self.ArgType.INT, self.ArgType.INT, self.ArgType.INT):
            //    return


            string msg_type = args[1];
            string pre = args[2];
            string folder = args[3];
            string anim = args[4];
            string text = args[5];
            string pos = args[6];
            string sfx = args[7];
            int anim_type = int.Parse(args[8]);
            int cid = int.Parse(args[9]);
            int sfx_delay = int.Parse(args[10]);
            int button = int.Parse(args[11]);
            int evidence = int.Parse(args[12]);
            int flip = int.Parse(args[13]);
            int ding = int.Parse(args[14]);
            int color = int.Parse(args[15]);

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
            newargs = new string[] { msg_type, pre, folder, anim, msg, pos, $"{sfx}", $"{anim_type}", $"{cid}", $"{sfx_delay}", $"{button}", $"{evidence}", $"{flip}", $"{ding}", $"{color}" };

            c.area.send_command("MS", newargs);

            //self.client.area.set_next_msg_delay(len(msg))
            //logger.log_server('[IC][{}][{}]{}'.format(self.client.area.id, self.client.get_char_name(), msg), self.client)

            //if self.client.area.is_recording:
            //    self.client.area.recorded_messages.append(args)
        }

        public static void net_cmd_mc(Client c, string[] args)
        {
            AreaManager.Area area = AreaManager.get_area_by_name(args[1]);
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
                if (c.is_muted)
                {
                    c.send_host_message("You have been muted by a moderator.");
                    return;
                }

                //if not self.validate_net_cmd(args, self.ArgType.STR, self.ArgType.INT):
                //    return

                if (int.Parse(args[2]) != c.char_id)
                    return;
                //    return

                //if self.client.change_music_cd():
                //    self.client.send_host_message('You changed song too much times. Please try again after {} seconds.'.format(int(self.client.change_music_cd())))
                //    return

                //try:
                //    name, length = self.server.get_song_data(args[0])
                c.area.play_music(args[1], int.Parse(args[2]));
                //    self.client.area.play_music(name, self.client.char_id, length)
                //    self.client.area.add_music_playing(self.client, name)
                //    logger.log_server('[{}][{}]Changed music to {}.'
                //                      .format(self.client.area.id, self.client.get_char_name(), name), self.client)
                //except ServerError:
                //    return

                //except ClientError as ex:

                //self.client.send_host_message(ex)






            }

        }

        #endregion
    }
}
