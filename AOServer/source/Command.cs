using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOServer
{
    class Command
    {

        public string command { get; set; }
        public string[] Args { get; set; }

        public Command(byte[] buffer)
        {

        }

        public Command(string _string)
        {

        }

        public byte[] Buffer()
        {
            return Encoding.UTF8.GetBytes($"{command}#{string.Join("#", Args)}#%");
        }

        public string String()
        {
            return $"{command}#{string.Join("#", Args)}#%";
        }


    }
}
