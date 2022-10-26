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
            if (!string.IsNullOrEmpty(hashCatLine))
            {
                string[] _info = hashCatLine.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (_info.Length > 2)
                {
                    if (!string.IsNullOrEmpty(_info[2]))
                    {
                        if (_info[2] != "hex:") Password = _info[2];
                        else
                        {
                            string hexstring = (_info[2].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries))[1];
                            Password = HexStringToString(hexstring);
                        }

                        Length = Password.Length;

                        // Check if the cracked password is simple or not
                        OnlyNumbersLetters = Password.All(Char.IsLetterOrDigit);
                        OnlyNumbers = Password.All(Char.IsDigit);
                        OnlyLetters = Password.All(Char.IsLetter);

                        // Check what characters are used to construct the cracked password
                        Numbers = Password.Any(Char.IsDigit);
                        Letters = Password.Any(Char.IsLetter);
                        SpecialChars = Password.Any(Char.IsSymbol);
                        Capital = Password.Any(Char.IsUpper);
                        NumbersLettersCapital = Numbers && Letters && Capital;
                        NumbersLettersCapitalSpecial = Numbers && Letters && Capital && SpecialChars;


                        DomainUsername = _info[0];
                        // Get the Domain and Username
                        if (DomainUsername.Contains('\\'))
                        {
                            string[] _user = DomainUsername.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                            Domain = _user[0];
                            UserName = _user[1];
                        }
                        else
                        {
                            UserName = _info[0];
                        }

                        // Check if the password is equal to the username - regardless of lower/uppercase
                        if (String.Compare(UserName.ToLower(), Password.ToLower(), true) == 0) EqualToAccountName = true;

                        Cracked = true;
                    }

                    else Cracked = false;
                }
            }
        }

        public string Domain { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string DomainUsername { get; set; }

        public int Length { get; set; }
        public bool OnlyNumbers { get; set; }
        public bool OnlyLetters { get; set; }
        public bool OnlyNumbersLetters { get; set; }
        public bool Numbers { get; set; }
        public bool Letters { get; set; }
        public bool Capital { get; set; }

        public bool NumbersLettersCapital { get; private set; }
        public bool NumbersLettersCapitalSpecial { get; private set; }
        public bool SpecialChars { get; set; }
        public bool EqualToAccountName { get; set; }
        public bool Cracked { get; set; }

        private string HexStringToString(string hexString)
        {
            if (hexString == null || (hexString.Length & 1) == 1)
            {
                throw new ArgumentException();
            }
            var sb = new StringBuilder();
            for (var i = 0; i < hexString.Length; i += 2)
            {
                var hexChar = hexString.Substring(i, 2);
                sb.Append((char)Convert.ToByte(hexChar, 16));
            }
            return sb.ToString();
        }
    }
}
