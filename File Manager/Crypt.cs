using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Manager
{
    class Crypt
    {
        public static string Encrypt(string s)
        {
            string ret = "";
            for (int  i = 0; i < s.Length; i++)
            {
                ret += (char)(s[i] + 1);
            }
            return ret;
        }

        public static string Decrypt(string s)
        {
            string ret = "";
            for (int i = 0; i < s.Length; i++)
            {
                ret += (char)(s[i] - 1);
            }
            return ret;
        }
    }
}
