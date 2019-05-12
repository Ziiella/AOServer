using System;

namespace AOServer
{
    class Exceptions
    {
        public class ClientError : Exception
        {
            public ClientError(ClientManager.Client c, string message)
                : base(message)
            {
                c.send_host_message(message);
            }
        }

        public class AreaError : Exception
        {
            public AreaError(ClientManager.Client c, string message)
            : base(message)
            {
                c.send_host_message(message);
            }
        }
    }


}
