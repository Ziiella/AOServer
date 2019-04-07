using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

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
                    SendRawMessage(stream, Config.CharPacket);
                    break;

                case "RM":
                    SendRawMessage(stream, Config.MusicPacket);
                    break;

                case "RD":
                    SendRawMessage(stream, "DONE#%");
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


        public static string[] splitArgs(string data)
        {
            string[] args = data.Split('#');
            return args;
        }

    }
}
