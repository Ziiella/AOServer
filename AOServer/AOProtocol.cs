using System;
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

        #endregion
    }
}
