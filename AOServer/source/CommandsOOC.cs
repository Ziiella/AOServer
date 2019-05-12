using System;
using System.Linq;
using static AOServer.ServerAO.Data.ClientManager;

namespace AOServer
{
    class CommandsOOC
    {

        private static string CombineArgs(string[] args)
        {
            string result = "";
            result += args[1];
            for (int i = 2; i < args.Count(); i++)
            {
                result += " ";
                result += args[i];
            }
            return result;
        }

        public static void ooc_command(Client c, string[] args)
        {
            switch (args[0])
            {
                case "getareas":
                    c.send_area_list();
                    break;

                case "lockdown":
                    ooc_cmd_lockdown(c, args);
                    break;

                case "bg":
                case "background":
                    ooc_cmd_bg(c, args);
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

        #region UserCommands
        public static void ooc_cmd_switch(Client c, string[] args)
        {
            if (args.Count() < 2)
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

        public static void ooc_cmd_bg(Client c, string[] args)
        {
            if (args.Count() < 2)
            {
                c.send_host_message("You must specify a name. Use /bg <background>.");
                return;
            }
            if (!c.is_mod && c.area.bg_lock)
            {
                c.send_host_message("This area's background is locked");
                return;
            }
            try
            {
                string bg = CombineArgs(args);
                c.area.change_background(c, bg);
                c.area.send_host_message($"{c.get_char_name()} changed the background to {c.area.background}.");
            }
            catch { }

        }
        #endregion

        #region ModCommands
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

        public static void ooc_cmd_lockdown(Client c, string[] args)
        {
            if (!c.is_mod)
            {
                c.send_host_message("You are unauthorized to use that command.");
                return;
            }
            if(args.Count() < 2)
            {
                c.send_host_message("Please specify if you're locking down the server or area.");
                return;
            }

            switch (args[1].ToLower())
            {
                case "server":
                    ServerAO.Server.lockdown();
                    break;

                case "area":
                    c.area.lockdown();
                    break;

                default:
                    c.send_host_message("Please specify if you're locking down the server or area.");
                    break;
            }

        }
        #endregion

    }
}
