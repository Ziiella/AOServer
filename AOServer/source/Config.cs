using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using YamlDotNet;
using YamlDotNet.RepresentationModel;

namespace AOServer
{
    class Config
    {
        public static bool announce_areas = false;
        public static int port = 27015;
        public static IPAddress ipAdr = IPAddress.Parse("10.0.0.45");
        public static string ServerName { get; set; } = "Server";
        public static string MOTD = "=== MOTD ===\r\n{}\r\n=============";
        public static int PlayerLimit { get; set; }

        #region DataLists
        public static List<string> char_list = new List<string>();
        public static List<string> music_list = new List<string>();
        public static List<string> bg_list = new List<string>();
        public static List<string> music_list_ao2 = new List<string>();
        #endregion



        public static void Init()
        {
            PlayerLimit = 20;
            load_backgrounds();
            load_chars();
            load_music();

        }

        private static void load_music()
        {
            FileStream charConfigFile = new FileStream("config/music.yaml", FileMode.Open);
            var input = new StreamReader(charConfigFile);
            var yaml = new YamlStream();
            yaml.Load(input);

            var mapping = (YamlSequenceNode)yaml.Documents[0].RootNode;

            foreach (YamlMappingNode category in mapping)
            {
                music_list.Add(category[new YamlScalarNode("category")].ToString());

                var songs = (YamlSequenceNode)category.Children[new YamlScalarNode("songs")];

                foreach(var song in songs)
                {
                    music_list.Add(song[new YamlScalarNode("name")].ToString());
                }

            }

        }

        static void load_chars()
        {
            FileStream charConfigFile = new FileStream("config/characters.yaml", FileMode.Open);
            var input = new StreamReader(charConfigFile);
            var yaml = new YamlStream();
            yaml.Load(input);

            var mapping = (YamlSequenceNode)yaml.Documents[0].RootNode;

            foreach (var entry in mapping.Children)
            {
                char_list.Add(((YamlScalarNode)entry).Value);
            }
        }

        public static void load_iniswaps()
        {

        }

        private static void load_backgrounds()
        {
            FileStream bgConfigFile = new FileStream("config/backgrounds.yaml", FileMode.Open);
            var input = new StreamReader(bgConfigFile);
            var yaml = new YamlStream();
            yaml.Load(input);

            var mapping = (YamlSequenceNode)yaml.Documents[0].RootNode;

            foreach (var entry in mapping.Children)
            {
                bg_list.Add(((YamlScalarNode)entry).Value);
            }
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
