using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOServer.ServerAO.Data;

namespace AOServer.ServerAO
{
    class Config
    {
        public static string ServerName = "Server";
        public static string MOTD = "=== MOTD ===\r\n{}\r\n=============";
        public static int PlayerLimit { get; set; }

        public static List<string> Chars = new List<string>();
        public static List<string> Music = new List<string>();
        public static List<Area> Areas = new List<Area>();



        public static void InitConfig()
        {
            PlayerLimit = 20;
            AddChars();
            AddMusic();
        }

        private static void AddMusic()
        {
            Music.Add("Versus.mp3");
            Music.Add("BOX 15.mp3");
        }

        static void AddChars()
        {
            Chars.Add("Makoto_HD");
            Chars.Add("Chiaki_HD");
            Chars.Add("Kirigiri_HD");
        }




    }
}
