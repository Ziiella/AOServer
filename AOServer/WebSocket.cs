using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using AOServer.ServerAO.Data;

namespace AOServer
{
    class WebSocket
    {
        private TcpClient client;
        private NetworkStream stream;
        string buffer;

        public WebSocket(TcpClient client)
        {
            this.client = client;
            stream = this.client.GetStream();
            ClientManager.new_client(this);

            Thread t = new Thread(socketLoop);
            t.Start(this);

        }

        public void socketLoop(object _transport)
        {
            while (true)
            {
                buffer = Read();
                if (buffer != null)
                {
                    data_received();
                }
            }

        }


        private void data_received()
        {
            /*Handles any data received from the network.
            Receives data, parses them into a command and passes it
            to the command handler.
            :param data: bytes of data*/
            Console.WriteLine(buffer);
        }


        public void Close()
        {
            stream.Close();
            client.Close();
        }

        public void Write(byte[] buffer, int offset, int size)
        {
            stream.Write(buffer, offset, size);
        }

        public string Read()
        {
            byte[] buffer = new byte[1024];
            int byte_count = stream.Read(buffer, 0, buffer.Length);

            if(byte_count == 0)
            {
                return null;
            }

            return Encoding.UTF8.GetString(buffer, 0, byte_count);
        }

        private string[] get_messages(string data)
        {
            string[] args = data.Split('#');
            return args;
        }

    }
}
