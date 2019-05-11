﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOServer.ServerAO.Data;

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
    }


}