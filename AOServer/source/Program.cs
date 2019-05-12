using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOServer
{
    class Program
    {
        static string Software = "AOServer";
        static int release = 0;
        static int major_version = 0;
        static int minor_version = 1;


        static void Main(string[] args)
        {

            Server server = new Server();
            server.start();

        }

        public static string get_version_string()
        {
            return $"{release}.{major_version}.{minor_version}";
        }

        public static string get_server_software()
        {
            return Software;
        }


    }
}
