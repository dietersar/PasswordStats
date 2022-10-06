using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Data;

namespace PasswordStats
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        string pwfile = "";

        public Window1()
        {
            InitializeComponent();
            Output.Text = "This tool performs some statistics on passwords cracked with HashCat.\n" +
                "Your input file needs to be created with:\n" +
                "\"hashcat -m <your cracking option> <your hash file> --show --username\"";
        }

        
        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            
            bool begin_read_data = false;

            // Array to store the length of the passwords
            int[] pw_length = new int[20];
            /*
             * other_info[0] = same as account
             * 1 = only letters
             * 2 = lower & uppercase
             * 3 = only numbers
             * 4 = letters & numbers
             * 5 = also special chars
             */
            int[] other_info = new int[6];
            int uncracked = 0;


            OpenFileDialog _open_file = new OpenFileDialog();
            if (_open_file.ShowDialog() == true)
            {
                pwfile = _open_file.FileName;



                int count_pass = 0;

                StreamReader _read_file = new StreamReader(pwfile);
                while (!_read_file.EndOfStream)
                {
                    string _pw_line = _read_file.ReadLine();

                    PasswordInfo pwinfo = new PasswordInfo(_pw_line);

                    // password checks

                    if ((pwinfo.Password != "hex:") && (pwinfo.Password != null))
                    {
                        // First check: length
                        if (pwinfo.Password.Length < 19)
                        {
                            pw_length[pwinfo.Password.Length] = pw_length[pwinfo.Password.Length] + 1;

                        }
                        else
                        {
                            // pw_length[19] holds the number of passwords bigger than 8 characters.
                            pw_length[19] = pw_length[19] + 1;
                        }

                        // Second check: password equal to accountname
                        if (String.Compare(pwinfo.UserName.ToLower(), pwinfo.Password.ToLower(), true) == 0)
                        {
                            other_info[0] = other_info[0] + 1;
                        }

                        // Third check: capital letters included?


                        // Fourth check: only letters, numbers or combination or special chars also?
                        char[] pw_chars = pwinfo.Password.ToCharArray();
                        /*
                         * This bool array keeps track of everything that is in the password.
                         * 
                         * 0 = letters
                         * 1 = capital letter
                         * 2 = small letter
                         * 3 = numbers
                         * 4 = symbols
                         */
                        bool[] content = { false, false, false, false, false };
                        foreach (char i in pw_chars)
                        {
                            if (Char.IsDigit(i)) content[3] = true;
                            if (Char.IsLetter(i))
                            {
                                content[0] = true;
                                if (Char.IsLower(i))
                                {
                                    content[2] = true;
                                }
                                if (Char.IsUpper(i))
                                {
                                    content[3] = true;
                                }
                            }
                            if (Char.IsSymbol(i) || Char.IsControl(i)) content[4] = true;
                        }
                        if (content[0] && !content[3] && !content[4]) other_info[1] = other_info[1] + 1;
                        else if (!content[0] && content[3] && !content[4]) other_info[3] = other_info[3] + 1;
                        else if (content[0] && content[3] && !content[4]) other_info[4] = other_info[4] + 1;
                        else if (content[0] && content[3] && content[4]) other_info[5] = other_info[5] + 1;
                        if (content[2] && content[3]) other_info[2] = other_info[2] + 1;

                        count_pass++;
                    }
                    else
                    {
                        uncracked++;
                    }
                }
                string output_text = "";
                output_text = output_text + "Number of passwords = " + count_pass.ToString() + "\n\n";
                for (int _length = 0; _length < pw_length.Length; _length++)
                {
                    if ((pw_length[_length] != 0) && (_length != pw_length.Length))
                    {
                        output_text = output_text + "Number of passwords of length " + _length.ToString() + "\t: " + pw_length[_length].ToString() + "\n";
                    }
                    else if (_length == (pw_length.Length - 1))
                    {
                        output_text = output_text + "Number of passwords with a length > 19 :\t" + pw_length[_length].ToString();
                    }
                }
                output_text += "\n";
                output_text = output_text + other_info[0].ToString() + " passwords are the same as the account name\n\n";
                output_text = output_text + "Passwords with only letters :\t" + other_info[1].ToString() + "\n";
                output_text = output_text + "Passwords with only numbers :\t" + other_info[3].ToString() + "\n";
                output_text = output_text + "Passwords of letters & numbers :\t" + other_info[4].ToString() + "\n";
                output_text = output_text + "Passwords of letters, numbers & symbols :\t" + other_info[5].ToString() + "\n";

                output_text = output_text + "Uncracked passwords :\t" + uncracked.ToString() + "\n";

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

        private void SelectFile3_Click(object sender, RoutedEventArgs e)
        {
            OpenFile new_file = new OpenFile();
            new_file.ShowDialog();
            string output = "filetype: " + new_file.PasswordFileType.ToString() + " - filename: " + new_file.Pwfile.ToString();
            MessageBox.Show(output);
        }

    }
}
