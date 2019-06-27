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

        public Message_window(string header, string body, string type)
        {
            InitializeComponent();
            lbl_info.Content = header;
            tbx_info.Text = body;

            if (type == "error")
            {
                this.Background = Brushes.Red;
                tbx_info.Background = Brushes.DarkRed;
                btn_ok.Background = Brushes.DarkRed;
            }
            else if (type == "message")
            {
                this.Background = Brushes.Gray;
                tbx_info.Background = Brushes.DarkGray;
                btn_ok.Background = Brushes.DarkGray;
            }
            else if (type == "succes")
            {
                this.Background = Brushes.Green;
                tbx_info.Background = Brushes.DarkSlateGray;
                btn_ok.Background = Brushes.DarkSlateGray;
            }
        }

        public Message_window(string header, string body, Brush bg, Brush tbx, Brush btn)
        {
            InitializeComponent();
            lbl_info.Content = header;
            tbx_info.Text = body;

                this.Background = bg;
                tbx_info.Background = tbx;
                btn_ok.Background = btn;
        }

        private void Btn_ok_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Btn_ok_Click(sender, e);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }
    }
}
