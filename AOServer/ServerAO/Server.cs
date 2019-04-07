using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace AOServer.ServerAO
{
    class Server
    {
        readonly static object _lock = new object();
        readonly static Dictionary<int, TcpClient> userList = new Dictionary<int, TcpClient>();


        static TcpListener server = null;

        static int userCount = 0;

        //TO-DO: Make config file to store these:
        static Int32 port = 27015;
        static IPAddress ipAdr = IPAddress.Parse("10.0.0.45");


        public static void start()
        {
            Console.WriteLine("AOServer - v0.01 ");
            Config.InitConfig();
            server = new TcpListener(ipAdr, port);
            server.Start();


            while (true) // Add your exit flag here
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Client connected!");
                userCount++;
                lock (_lock) userList.Add(userCount, client);
                Thread t = new Thread(clientHandler);
                t.Start(userCount);

            }

        }

        public static void clientHandler(object o)
        {
            int id = (int)o;
            TcpClient client;

            lock (_lock) client = userList[id];
            //byte[] msg = Encoding.ASCII.GetBytes("decryptor#322#%");
            NetworkStream stream = client.GetStream();
            //stream.Write(msg, 0, msg.Length);
            //Console.WriteLine("Sent: {0}", "decryptor#34#%");


            while (true)
            {
                
                byte[] buffer = new byte[1024];
                int byte_count = stream.Read(buffer, 0, buffer.Length);
                

                if (byte_count == 0)
                {
                    break;
                }

                string data = Encoding.UTF8.GetString(buffer, 0, byte_count);
                Console.WriteLine(data);

                ClientHandler.HandleProtocol(ClientHandler.splitArgs(data), stream);


            }

            lock (_lock) userList.Remove(id);
            client.Client.Shutdown(SocketShutdown.Both);
            client.Close();


        }

        public static void broadcast(string message)
        {
            lock (_lock)
            {
                foreach (TcpClient c in userList.Values)
                {
                    NetworkStream stream = c.GetStream();

                    ClientHandler.SendRawMessage(stream, message);
                }
            }
        }
    }
}
