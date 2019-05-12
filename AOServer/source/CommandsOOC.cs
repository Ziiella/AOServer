using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AOServer.ServerAO.Data.ClientManager;

namespace AOServer
{
    class CommandsOOC
    {

        public static void ooc_command(Client c, string[] args)
        {
            switch (args[0])
            {
                case "getareas":
                    c.send_area_list();
                    break;

                case "roll":
                    Random random = new Random();
                    
                    int result = random.Next(1, 6);
                    c.area.send_host_message($"{c.get_char_name()} rolled a {result} out of 6.");
                    break;

                case "switch":
                    ooc_cmd_switch(c, args);
                    break;

                case "randomchar":
                    c.change_character(c.area.get_rand_avail_char_id());
                    break;

                case "login":
                    ooc_cmd_login(c, args);
                    break;

                default:
                    c.send_host_message("Invalid command.");
                    break;
            }

            
        }


        public static void ooc_cmd_login(Client c, string[] args)
        {
            if (args.Count() < 2)
            {
                c.send_host_message("You must specify the password.");
            }
            try
            {
                c.auth_mod(args[1]);
                c.send_host_message("Logged in as a moderator.");
                //logger.log_mod('[{}][{}] Logged in as moderator.'.format(client.area.id, client.get_char_name()), client)

            }
            catch { }

         }

        

        public static void ooc_cmd_switch(Client c, string[] args)
        {
            if(args.Count() < 2)
            {
                c.send_host_message("You must specify a character name.");
                return;
            }
            int cid = ServerAO.Server.get_char_id_by_name(args[1]);
            try
            {
                c.change_character(cid, c.is_mod);
                c.send_host_message("Character changed.");
            }
            catch { };
        }
    }
}
