using System.Text;
using System.Net.Sockets;

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

        public bool isConnected()
        {
            if (client.Client.Poll(0, SelectMode.SelectRead))
            {
                byte[] buff = new byte[1];
                if (client.Client.Receive(buff, SocketFlags.Peek) == 0)
                {
                    // Client disconnected
                    return false;
                }
            }

            return true;
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
