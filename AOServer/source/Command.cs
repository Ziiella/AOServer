using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOServer
{
    class Command
    {
        private string command;
        private string[] Args;

        public Command(byte[] buffer)
        {
            parse_string(Encoding.ASCII.GetString(buffer));
        }

        public Command(string _string)
        {
            parse_string(_string);
        }

        public Command(string command, params string[] args)
        {
            this.command = command;
            Args = args;
        }

        public Command(string command, List<string> args)
        {
            this.command = command;
            Args = args.ToArray();
        }

        public byte[] get_buffer()
        {
            return Encoding.UTF8.GetBytes(get_string());
        }

        public string get_string()
        {
            if(Args.Count() == 0)
            {
                return $"{command}#%";
            }
            else
            {
                return $"{command}#{string.Join("#", Args)}#%";
            }
            
        }

        public string get_command()
        {
            return command;
        }

        public string[] get_args()
        {
            return Args;
        }

        private void parse_string(string msg)
        {
            if (msg.StartsWith("#"))
            {
                msg = msg.Substring(1);
                string[] spl = msg.Split('#');
                command = FantaCrypt.fanta_decrypt(spl[0]);
                Args = new string[] { spl[1] };
            }
            else
            {

                string[] spl = msg.Split('#');
                command = spl[0];

                string[] args = new string[spl.Count() - 1];

                for (int i = 1; i < spl.Count(); i++)
                {
                        args[i - 1] = spl[i];
                }

                Args = args;

            }
        }
    }
}
