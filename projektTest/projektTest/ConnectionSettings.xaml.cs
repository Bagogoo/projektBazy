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
    /// Logika interakcji dla klasy ConnectionSettings.xaml
    /// </summary>
    public partial class ConnectionSettings : Window
    {
        public ConnectionSettings()
        {
            InitializeComponent();
            CenterWindowOnScreen();
        }

        public ConnectionSettings(ConnectionSet _cs)
        {
            InitializeComponent();

            connectionInfo = _cs;
            FillFields();
            CenterWindowOnScreen();
        }

        private ConnectionSet connectionInfo;
        private bool save = false;

        //centrowanie okna na ekranie
        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private void FillFields()
        {
            tbx_dataSource.Text = connectionInfo.get_dataSource();
            tbx_initialCatalog.Text = connectionInfo.get_initialCatalog();
            tbx_userId.Text = connectionInfo.get_userId();
            tbx_password.Text = connectionInfo.get_password();
            tbx_connectTimeout.Text = connectionInfo.get_connectTimeout();
            if (connectionInfo.get_encrypt() == "True") cbx_encrypt.IsChecked = true;
            else cbx_encrypt.IsChecked = false;
            if (connectionInfo.get_trustServerCertificate() == "True") cbx_trustServerCertificate.IsChecked = true;
            else cbx_trustServerCertificate.IsChecked = false;
            tbx_applicationIntent.Text = connectionInfo.get_applicationIntent();
            if (connectionInfo.get_multiSubnetFailover() == "True") cbx_multiSubnetFailover.IsChecked = true;
            else cbx_multiSubnetFailover.IsChecked = false;
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            save = true;

            connectionInfo.set_dataSource(tbx_dataSource.Text);
            connectionInfo.set_initialCatalog(tbx_initialCatalog.Text);
            connectionInfo.set_userId(tbx_userId.Text);
            connectionInfo.set_password(tbx_password.Text);
            connectionInfo.set_connectTimeout(tbx_connectTimeout.Text);
            if (cbx_encrypt.IsChecked == true) connectionInfo.set_encrypt("True");
            else connectionInfo.set_encrypt("False");
            if (cbx_trustServerCertificate.IsChecked == true) connectionInfo.set_trustServerCertificate("True");
            else connectionInfo.set_trustServerCertificate("False");
            connectionInfo.set_applicationIntent(tbx_applicationIntent.Text);
            if (cbx_multiSubnetFailover.IsChecked == true) connectionInfo.set_multiSubnetFailover("True");
            else connectionInfo.set_multiSubnetFailover("False");

            this.Close();
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            save = false;
            this.Close();
        }

        public bool get_saveQuest() { return save; }
        public ConnectionSet get_connectionInfo() { return connectionInfo; }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        private void Btn_default_Click(object sender, RoutedEventArgs e)
        {
            ConnectionSet tmp = new ConnectionSet();
            connectionInfo = tmp;
            FillFields();
        }
    }
}
