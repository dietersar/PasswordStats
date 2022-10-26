using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordStats
{
    internal class Stats
    {
        public readonly SortedDictionary<string, int> CountTheOccurrences = new SortedDictionary<string, int>();
        public readonly List<string> AccountsWithPassOnlyNumbers = new List<string>();
        public readonly List<string> AccountsWithPassOnlyLetters = new List<string>();
        public readonly List<string> AccountsWithPassNumbersLetters = new List<string>();
        public readonly List<string> AccountsWithSmallPassword = new List<string>();

        public Stats()
        {
        }

        public int[] Length { get; private set; } = new int[20];
        public int OnlyNumbers { get; private set; }
        public int OnlyLetters { get; private set; }
        public int OnlyNumbersLetters { get; private set; }
        public int Numbers { get; private set; }
        public int Letters { get; private set; }
        public int Capital { get; private set; }
        public int SpecialChars { get; private set; }
        public int NumbersLettersCapital { get; private set; }
        public int NumbersLettersCapitalSpecial { get; private set; }
        public int EqualToAccountName { get; private set; }
        public int Amount { get; private set; }
        public int AmountCracked { get; private set; }

        public void AddPasswordInfo(PasswordInfo pwinfo)
        {
            if (pwinfo.Password != null)
            {
                if (pwinfo.Length < 19)
                {
                    Length[pwinfo.Length] = Length[pwinfo.Length] + 1;
                    if (pwinfo.Length < 8) AccountsWithSmallPassword.Add(pwinfo.DomainUsername);
                }
                else
                {
                    // length[19] holds the number of passwords bigger than 18 characters.
                    Length[19] = Length[19] + 1;
                }

                if (pwinfo.OnlyLetters)
                {
                    OnlyLetters++;
                    AccountsWithPassOnlyLetters.Add(pwinfo.DomainUsername);
                }
                if (pwinfo.OnlyNumbers)
                {
                    OnlyNumbers++;
                    AccountsWithPassOnlyNumbers.Add(pwinfo.DomainUsername);
                }
                if (pwinfo.OnlyNumbersLetters)
                {
                    OnlyNumbersLetters++;
                    AccountsWithPassNumbersLetters.Add(pwinfo.DomainUsername);
                }
                if (pwinfo.Numbers) Numbers++;
                if (pwinfo.Letters) Letters++;
                if (pwinfo.Capital) Capital++;
                if (pwinfo.SpecialChars) SpecialChars++;
                if (pwinfo.NumbersLettersCapital) NumbersLettersCapital++;
                if (pwinfo.NumbersLettersCapitalSpecial) NumbersLettersCapitalSpecial++;
                if (pwinfo.EqualToAccountName) EqualToAccountName++;

                // Counting occurances of each password
                CountTheOccurrences.TryGetValue(pwinfo.Password, out int count);
                CountTheOccurrences[pwinfo.Password] = count + 1;

                if (pwinfo.Cracked) AmountCracked++;
            }
            Amount++;
        }
    }
}
