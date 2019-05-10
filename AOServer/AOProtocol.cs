﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AOServer.ServerAO.Data.ClientManager;

namespace AOServer
{
    class AOProtocol
    {

        public static void data_received(string buffer, Client c)
        {
            /*Handles any data received from the network.
            Receives data, parses them into a command and passes it
            to the command handler.
            :param data: bytes of data*/
            Console.WriteLine(buffer);
            string[] data = get_messages(buffer);

            switch (data[0])
            {
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

        public static void net_cmd_askchaa(Client c)
        {
            /*Ask for the counts of characters/evidence/music
            askchaa#%*/
            string[] args = new string[3];

            args[0] = ServerAO.Config.Chars.Count.ToString(); //Char count
            args[1] = 0.ToString(); //Evidence count
            args[2] = ServerAO.Config.Music.Count.ToString(); //Music Count
            c.send_command("SI", args);
        }

        public static void net_cmd_rc(Client c)
        {
            /*Asks for the whole character list(AO2)  AC#% */
            c.send_command("SC", ServerAO.Config.Chars);
        }

        public static void net_cmd_rm(Client c)
        {
            /* Asks for the whole music list(AO2) AM#% */
            c.send_command("SM", ServerAO.Config.Music);
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
            int sfx = int.Parse(args[7]);
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

        #endregion
    }
}
