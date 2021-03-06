﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace AOServer
{
    class Server
    {
        public static bool is_lockdown;
        static TcpListener server;
        //self.allowed_iniswaps = None
        //self.loaded_ips = {}

        //self.load_iniswaps()
        //self.load_gimps()
        //self.client_manager = ClientManager(self)
        //self.area_manager = AreaManager(self)
        //self.serverpoll_manager = ServerpollManager(self)
        //self.ban_manager = BanManager()
        //self.software = 'tsuserver3'
        //self.version = 'tsuserver3dev'
        //self.char_list = None
        //self.char_pages_ao1 = None
        //self.music_list = None
        //self.music_list_ao2 = None
        //self.music_pages_ao1 = None
        //self.backgrounds = None
        //self.data = None
        //self.features = set()
        //self.stats_manager = Database(self)
        //self.district_client = None
        //self.ms_client = None
        //logger.setup_logger(debug=self.config['debug'], log_size=self.config['log_size'],
        //                    log_backups=self.config['log_backups'], areas=self.area_manager.areas)

        public Server()
        {
            is_lockdown = false;
            Logger.Init(false, false);
            Config.Init();
            AreaManager.load_areas();
            Config.build_music_list_ao2();
            //self.load_characters()
            //self.load_music()
            //self.load_backgrounds()
            //self.load_data()
            //self.load_ids()
            //self.enable_features()

        }

        public void start()
        {
            Console.WriteLine($"AOServer - v{Program.get_version_string()}");
            server = new TcpListener(Config.ipAdr, Config.port);
            server.Start();

            Console.WriteLine("Server started.");
            while (true) 
            {
                WebSocket client = new WebSocket(server.AcceptTcpClient());

            }
            Console.WriteLine("Server shutting down.");


        }

        public static void lockdown()
        {
            if (is_lockdown)
            {
                is_lockdown = false;
                send_all_host_message("The server is no longer in lockdown.");
            }
            else if (!is_lockdown)
            {
                is_lockdown = true;
                send_all_host_message("The server is now in lockdown.");
            }
        }


        public static int get_player_count()
        {
            return ClientManager.clients.Count();
        }

        public static string get_ipid()
        {
            return "N/A";
        }

        public static bool is_valid_char_id(int char_id)
        {
            return Config.char_list.Count > char_id && char_id >= 0;
        }

        public static int get_char_id_by_name(string name)
        {
            for (int i = 0; i < Config.char_list.Count(); i++)
            {
                if (Config.char_list[i] == name)
                {
                    return i;
                }
            }

            return 999;
        }

        public static string get_song_data(int music)
        {
            return "nothing.mp3";
        }

        public static void send_all_cmd_pred(Command cmd)
        {
            foreach(var client in ClientManager.clients)
            {
                client.send_command(cmd);
            }

            //for client in self.client_manager.clients:
            //if pred(client):
            //    client.send_command(cmd, *args)
        }

        public static void send_all_host_message(string msg)
        {
            foreach (var client in ClientManager.clients)
            {
                client.send_host_message(msg);
            }
        }

        public static void broadcast_global(ClientManager.Client client, string msg, bool as_mod = false)
        {

        }

        public static void broadcast_globalooc(ClientManager.Client client, string msg, bool as_mod = false)
        {

        }

        public static void broadcast_globalmod(ClientManager.Client client, string msg, bool as_mod = false)
        {
            string username = client.name;
            string ooc_name = $"{"Global"}[{username}]";
            if (as_mod)
                ooc_name += "[MOD]";
            //self.send_all_cmd_pred('CT', ooc_name, msg, pred=lambda x: not x.muted_global)
            
            //if self.config['use_district']:
            //	self.district_client.send_raw_message(
            //	'GLOBAL#{}#{}#{}#{}'.format(int(as_mod), client.area.id, username, msg))

        }

        public static void broadcast_need(ClientManager.Client client, string msg)
        {
            string char_name = client.get_char_name();
            string area_name = client.area.name;
            int area_id = client.area.id;

            //self.send_all_cmd_pred('CT', '{}'.format(self.config['hostname']),
            //                       '=== Advert ===\r\n{} in {} [{}] needs {}\r\n==============='
            //                       .format(char_name, area_name, area_id, msg), pred=lambda x: not x.muted_adverts)
            //if self.config['use_district']:
            //    self.district_client.send_raw_message('NEED#{}#{}#{}#{}'.format(char_name, area_name, area_id, msg))

        }
    }
}
