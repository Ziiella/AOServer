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

        public WebSocket(TcpClient client)
        {
            this.client = client;
            stream = this.client.GetStream();
            ClientManager.new_client(this);

        }

        public void Close()
        {
            stream.Close();
            client.Close();
        }

        public void Write(byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
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



    }
}
