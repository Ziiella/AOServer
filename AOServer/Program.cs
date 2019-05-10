using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOServer.ServerAO;

namespace AOServer
{
    class Program
    {
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

        
    }
}
