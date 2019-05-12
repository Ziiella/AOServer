using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AOServer
{
    class Logger
    {
        static bool debug = false;
        static bool console = false;


        public static void Init(bool debug, bool console)
        {
            Logger.debug = debug;
            Logger.console = console;

            if (!Directory.Exists("logs"))
            {
                Directory.CreateDirectory("logs");
            }
        }


        public static void log_server(string log)
        {
            if (console)
                Console.WriteLine(log);

            StreamWriter serverLog = File.AppendText("logs\\server.log");
            serverLog.WriteLine(log);
            serverLog.Close();
        }

        public static void log_debug(string log)
        {
            if (debug)
            { 
                StreamWriter debugLog = File.AppendText("logs\\debug.log");
                debugLog.WriteLine(log);
                debugLog.Close();
                if (console)
                {
                    Console.WriteLine(log);
                }
            }
        }
    }
}
