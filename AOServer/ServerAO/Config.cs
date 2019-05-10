using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOServer.ServerAO.Data;
using System.Net.Sockets;
using System.Net;

namespace AOServer.ServerAO
{
    class Config
    {
        public static bool announce_areas = true;
        public static int port = 27015;
        public static IPAddress ipAdr = IPAddress.Parse("10.0.0.45");
        public static string ServerName { get; set; } = "Server";
        public static string MOTD = "=== MOTD ===\r\n{}\r\n=============";
        public static int PlayerLimit { get; set; }

        #region DataLists
        public static List<string> Chars = new List<string>();
        public static List<string> Music = new List<string>();
        public static List<AreaManager> Areas = new List<AreaManager>();
        #endregion



        public static void Init()
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



        public static void load_iniswaps()
        {

        }


        public static void load_backgrounds()
        {

        }


        public static void build_char_pages_ao1()
        {

        }

        public static void build_music_pages_ao1()
        {

        }

        public static void build_music_list_ao2()
        {

        }

    }
}
