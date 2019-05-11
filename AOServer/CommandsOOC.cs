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
        public static void ooc_command_send(Client c, string[] args)
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

                default:
                    c.send_host_message("Invalid command.");
                    break;
            }
        }
    }
}
