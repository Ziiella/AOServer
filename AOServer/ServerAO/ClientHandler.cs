using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using AOServer.ServerAO.Data;

namespace AOServer.ServerAO
{
    class ClientHandler
    {
        public static void HandleProtocol(string[] args, NetworkStream stream)
        {
            
            switch (args[0])
            {
                case "askchaa":
                    SendRawMessage(stream, $"SI#{Config.Chars.Count}#{0}#{Config.Music.Count}#%");
                    break;

                case "RC":
                    SendCommand(stream, "SC", Config.Chars);
                    break;

                case "RM":
                    SendCommand(stream, "SM", Config.Music);
                    break;

                case "RD":
                    SendRawMessage(stream, "DONE#%");
                    SendRawMessage(stream, $"CT#{Config.ServerName}#{Config.MOTD}#%");
                    break;

                case "CC":
                    SendRawMessage(stream, $"PV#{args[1]}#{args[3]}#{args[2]}#%");
                    break;

                case "MC":
                    Server.broadcast($"MC#{args[1]}#{args[2]}#%");
                    break;

                case "MS":
                    Server.broadcast($"MS#{args[1]}#{args[2]}#{args[3]}#{args[4]}#{args[5]}#{args[6]}#{args[7]}#{args[8]}#{args[9]}#{args[10]}#{args[11]}#{args[12]}#{args[13]}#{args[14]}#{args[15]}#%");
                    break;

                //Chat
                case "CT":
                    //To-Do: OOC not implemented: Need a Broadcasting system.
                    Server.broadcast($"CT#{args[1]}#{args[2]}#%");
                    
                    break;
            }
        }

        public static void SendRawMessage(NetworkStream stream, string message)
        {
            Console.WriteLine($"Sending response: [{message}]");
            byte[] msg = new byte[1024];
            msg = Encoding.UTF8.GetBytes(message);
            stream.Write(msg, 0, msg.Length);
        }

        public static void SendCommand(NetworkStream stream, string command, List<string> args)
        {
            //Console.WriteLine($"Sending response: [{message}]");
            byte[] msg = new byte[1024];

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < args.Count; i++)
            {
                sb.Append(args[i]);
                sb.Append("#");
            }

            string rawmsg = $"{command}#{sb.ToString()}%";


            msg = Encoding.UTF8.GetBytes(rawmsg);
            stream.Write(msg, 0, msg.Length);
        }

        public static void SendAreaList(Client self)
        {
            string msg = "=== Areas ===";

            Dictionary<bool, string> aLock = new Dictionary<bool, string> { { true, "[LOCKED]"}, { false, ""} };

            for(int i = 0; i < Config.Areas.Count; i++)
            {
                bool locked = false;
                Area area = Config.Areas[i];
                string owner = "FREE";

                if (area.owned)
                {
                    for (int x = 0; x < area.clients.Count; x++)
                    {

                        if (area.clients[x].is_cm)
                        {
                            owner = $"MASTER: {area.clients[x].char_name}";
                            break;
                        }
                        
                    }
                }
                if (area.is_gmlocked || area.is_modlocked || area.is_locked)
                {
                    locked = true;
                }
                else
                {
                    locked = false;
                }
                msg += $"\r\nArea {i}: {area.name} (users: {area.clients.Count}) {aLock[locked]}";
                if (self.area == area)
                    msg += " [*]";
            }

            SendHostMessage(self, msg);
        }


        public static void SendHostMessage(Client self, string msg)
        {

        }



        public static string[] splitArgs(string data)
        {
            string[] args = data.Split('#');
            return args;
        }

    }
}
