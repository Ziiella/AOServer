using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOServer
{
    class FantaCrypt
    {
        static int CRYPT_CONST_1 = 53761;
        static int CRYPT_CONST_2 = 32618;
        static int CRYPT_KEY = 5;

        public static string fanta_decrypt(string data)
        {
            List<int> data_bytes = new List<int>();
            char[] text = data.ToCharArray();


            for(int x = 0; x < text.Length; x += 2)
            {
                char[] _byte = { text[x], text[x + 1] };
                string hex = new string(_byte);
                int intValue = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);

                //short num = BitConverter.ToInt16(item, 0);
                data_bytes.Add(intValue);
            }

            int key = CRYPT_KEY;
            string ret = "";

            foreach(var bytee in data_bytes)
            {
                int val = bytee ^ ((key & 0xffff) >> 8);
                ret += (char)val;
                key = ((bytee + key) * CRYPT_CONST_1) + CRYPT_CONST_2;
            }

            return ret;
        }
    }
}
