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
        public static List<string> char_list = new List<string>();
        public static List<string> music_list = new List<string>();
        public static List<string> music_list_ao2 = new List<string>();
        #endregion



        public static void Init()
        {
            PlayerLimit = 20;
            AddChars();
            AddMusic();

        }

        private static void AddMusic()
        {
            music_list.Add("Versus.mp3");
            music_list.Add("BOX 15.mp3");
        }

        static void AddChars()
        {
            char_list.Add("Makoto_HD");
            char_list.Add("Chiaki_HD");
            char_list.Add("Kirigiri_HD");
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
            foreach (var area in AreaManager.areas)
            {
                music_list_ao2.Add(area.name);
            }

            foreach (var music in music_list)
            {
                music_list_ao2.Add(music);
            }
        }



    }
}
