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
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace PasswordStats
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class OpenFile : Window
    {
        private string _filetype = "";
        private string _pwfile = "";

        public string PasswordFileType
        {
            get
            {
                return _filetype;
            }
        }

        public string Pwfile
        {
            get
            {
                return _pwfile;
            }
        }

        public OpenFile()
        {
            InitializeComponent();
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog _open_file = new OpenFileDialog();
            if (_open_file.ShowDialog() == true)
            {
                _pwfile = _open_file.FileName;
            }
            if ((bool)RB_cain.IsChecked) { _filetype = "cain"; }
            else if ((bool)RB_lophtcrack.IsChecked) { _filetype = "lopht"; }
            else if ((bool)RB_rcrack.IsChecked) { _filetype = "rcrack"; }
            this.Close();
        }
    }
}
