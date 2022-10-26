using System;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;

namespace PasswordStats
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        Stats stats = new Stats();
        string pwfile = "";
        string onlyFileName = "";
        string onlyPath = "";

        public Window1()
        {
            InitializeComponent();
            Output.Text = "This tool performs some statistics on passwords cracked with HashCat.\n" +
                "Your input file needs to be created with:\n" +
                "\"hashcat -m <your cracking option> <your hash file> --show --username\"";
        }
        
        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog _open_file = new OpenFileDialog();
            if (_open_file.ShowDialog() == true)
            {
                // clear previous statistics
                stats = new Stats();

                pwfile = _open_file.FileName;
                onlyFileName = System.IO.Path.GetFileName(pwfile);
                onlyPath = System.IO.Path.GetFullPath(pwfile);

                StreamReader _read_file = new StreamReader(pwfile);
                while (!_read_file.EndOfStream)
                {
                    string _pw_line = _read_file.ReadLine();

                    PasswordInfo pwinfo = new PasswordInfo(_pw_line);
                    stats.AddPasswordInfo(pwinfo);
                }
                string output_text = "";
                output_text = output_text + "Number of cracked passwords = " + stats.AmountCracked + "\n";
                output_text = output_text + "Number of total passwords = " + stats.Amount + "\n\n";
                for (int _length = 0; _length < stats.Length.Length; _length++)
                {
                    if ((stats.Length[_length] != 0) && (_length != stats.Length.Length))
                    {
                        output_text = output_text + "Number of passwords of length " + _length.ToString() + "\t: " + stats.Length[_length].ToString() + "\n";
                    }
                    else if (_length == (stats.Length.Length - 1))
                    {
                        output_text = output_text + "Number of passwords with a length > 19 :\t" + stats.Length[_length].ToString();
                    }
                }
                output_text += "\n";
                output_text = output_text + stats.EqualToAccountName.ToString() + " passwords are the same as the account name\n\n";
                output_text = output_text + "Passwords with only letters :\t" + stats.OnlyLetters.ToString() + "\n";
                output_text = output_text + "Passwords with only numbers :\t" + stats.OnlyNumbers.ToString() + "\n";
                output_text = output_text + "Passwords with only letters & numbers :\t" + stats.OnlyNumbersLetters.ToString() + "\n";
                output_text = output_text + "Passwords with letters, numbers & symbols :\t" + stats.NumbersLettersCapitalSpecial.ToString() + "\n";

                // get sorted list of password occurances
                var sortedElements = from pw in stats.CountTheOccurrences
                                     orderby pw.Value descending
                                     select pw;
                var top10passwords = sortedElements.Take(10);
                output_text += "\nTop 10 most used passwords :\n";
                foreach (var element in top10passwords)
                {
                    output_text = output_text + element.Key.ToString() + " : \t" + element.Value.ToString() + "\n";
                }

                output_text += "\nAccounts with a password of only Numbers :\n";
                foreach (var element in stats.AccountsWithPassOnlyNumbers)
                {
                    output_text = output_text + element + "\n";
                }

                output_text += "\nAccounts with a password of only Letters :\n";
                foreach (var element in stats.AccountsWithPassOnlyLetters)
                {
                    output_text = output_text + element + "\n";
                }

                output_text += "\nAccounts with a password of only Letters and Numbers :\n";
                foreach (var element in stats.AccountsWithPassNumbersLetters)
                {
                    output_text = output_text + element + "\n";
                }

                output_text += "\nAccounts with a password smaller than 8 characters :\n";
                foreach (var element in stats.AccountsWithSmallPassword)
                {
                    output_text = output_text + element + "\n";
                }

                Output.Text = output_text;
                _read_file.Dispose();
            }
            else
            {
                Output.Text = "Select the file with your hashcat output first";
            }            
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Clipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Output.Text);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save_file = new SaveFileDialog
            {
                Title = "Save password analysis output",
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                InitialDirectory = onlyPath,
                FileName = "exported-" + onlyFileName
            };
            if (save_file.ShowDialog() == true)
            {
                File.WriteAllText(save_file.FileName, Output.Text); 
            }

            
        }
    }
}
