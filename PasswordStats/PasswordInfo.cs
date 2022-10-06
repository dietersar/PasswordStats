using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordStats
{
    internal class PasswordInfo
    {
        public PasswordInfo(string hashCatLine)
        {
            
            string[] _info = hashCatLine.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (_info.Length > 2)
            {
                Password = _info[2];
            }
            if (_info[0].Contains('\\'))
            {
                string[] _user = _info[0].Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                Domain = _user[0];
                UserName = _user[1];
            }
            else
            {
                UserName = _info[0];
            }
        }

        public string Domain { get;set; }
        public string Password { get;set; }
        public string UserName { get;set; }

    }
}
