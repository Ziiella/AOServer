using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOServer.ServerAO
{
    class Config
    {
        public static List<string> Chars = new List<string>();
        public static List<string> Music = new List<string>();
        public static string CharPacket;
        public static string MusicPacket;

        public static void InitConfig()
        {
            AddChars();
            AddMusic();
        }

        private static void AddMusic()
        {
            Music.Add("Versus.mp3");
            Music.Add("BOX 15.mp3");

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Music.Count; i++)
            {
                sb.Append(Music[i]);
                sb.Append("#");
            }

            MusicPacket = $"SM#{sb.ToString()}%";
        }

        static void AddChars()
        {
            Chars.Add("Makoto_HD");
            Chars.Add("Chiaki_HD");
            Chars.Add("Kirigiri_HD");

            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < Chars.Count; i++)
            {
                sb.Append(Chars[i]);
                sb.Append("#");
            }

            CharPacket = $"SC#{sb.ToString()}%";

        }




    }
}
