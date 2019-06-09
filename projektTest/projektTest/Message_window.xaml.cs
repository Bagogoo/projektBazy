using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace projektTest
{
    /// <summary>
    /// Logika interakcji dla klasy Message_window.xaml
    /// </summary>
    public partial class Message_window : Window
    {
        public Message_window()
        {
            InitializeComponent();
        }

        public Message_window(string header, string body)
        {
            InitializeComponent();
            lbl_info.Content = header;
            tbx_info.Text = body;
        }

        private void Btn_ok_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
