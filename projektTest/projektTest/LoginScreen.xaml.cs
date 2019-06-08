using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace projektTest
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CenterWindowOnScreen();
            tbx_username.Focus();
        }

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



        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection;
            string connectionString = "Data Source=projektbazy.database.windows.net;Initial Catalog=BazaDziennik;User ID=projektbazy;Password=zaq1@WSX;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            connection = new SqlConnection(connectionString);
            connection.Open();

            string login = tbx_username.Text;
            string password;

            if (cbx_hidepassword.IsChecked == true) password = pbx_password.Password;
            else password = tbx_password.Text;

            SqlCommand polecenie = new SqlCommand("SELECT Role, ID_USER FROM Logowanie WHERE Login=@login AND Password=@password", connection);
            polecenie.Parameters.Add("login", System.Data.SqlDbType.VarChar).Value = login;
            polecenie.Parameters.Add("password", System.Data.SqlDbType.VarChar).Value = password;
            SqlDataReader czytnik = polecenie.ExecuteReader();

            string role = "-"; //P - parent / C - child
            int identyficator = 0;

            while (czytnik.Read())
            {
                role = czytnik["Role"].ToString();
                identyficator = (int)czytnik["ID_USER"];
            }

            czytnik.Close();

            if(role == "Parent")
            {
                ParentPanel controlpanel = new ParentPanel(identyficator, connection);
                this.Hide();
                controlpanel.ShowDialog();
                this.Show();
            }
            else if (role == "Child")
            {
                ChildPanel controlpanel = new ChildPanel(identyficator, connection);
                this.Hide();
                controlpanel.ShowDialog();
                this.Show();
            }
            else
            {
                if (login == "Kujo" && password == "Jotaro") btn_login.Content = "Nani?!";
                else MessageBox.Show("Podano niepoprawne dane logowania!");
            }

        }

        private void KeyDown_on_tbx(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return) BtnLogin_Click(sender, e);
            if (e.Key == Key.PageUp || e.Key == Key.Up) tbx_username.Focus();
            if (e.Key == Key.PageDown || e.Key == Key.Down)
            {
                if (cbx_hidepassword.IsChecked == true) pbx_password.Focus();
                else tbx_password.Focus();
            }
            if (e.Key == Key.Escape) { tbx_username.Clear(); pbx_password.Clear(); tbx_username.Focus(); }
        }

        private void Cbx_hidepassword_Changed(object sender, RoutedEventArgs e)
        {
            if(cbx_hidepassword.IsChecked == true)
            {
                pbx_password.Visibility = Visibility.Visible;
                pbx_password.Height = 26;
                tbx_password.Visibility = Visibility.Hidden;
                tbx_password.Height = 0;
                pbx_password.Password = tbx_password.Text;
            }
            else
            {
                pbx_password.Visibility = Visibility.Hidden;
                pbx_password.Height = 0;
                tbx_password.Visibility = Visibility.Visible;
                tbx_password.Height = 26;
                tbx_password.Text = pbx_password.Password;
            }
        }
    }
}
