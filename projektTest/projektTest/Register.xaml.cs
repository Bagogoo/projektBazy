using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace projektTest
{
    /// <summary>
    /// Logika interakcji dla klasy Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
            CenterWindowOnScreen();
        }
        /*   public Register(SqlConnection _conn)
           {
               CenterWindowOnScreen();
               InitializeComponent();
               connection = _conn;
           }
           */
        SqlConnection connection = new SqlConnection("Data Source=projektbazy.database.windows.net;Initial Catalog=BazaDziennik;User ID=projektbazy;Password=zaq1@WSX;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        Message message = new Message();
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommand polecenie = new SqlCommand("INSERT INTO Logowanie (Login,Password,Role) VALUES (@Login,@Password,@Role)", connection);
                polecenie.Parameters.Add("@Login", System.Data.SqlDbType.VarChar).Value = tbx_username.Text;
                polecenie.Parameters.Add("@Password", System.Data.SqlDbType.VarChar).Value = pbx_password.Password;
                if (cbx_role.IsChecked == true) polecenie.Parameters.Add("Role", System.Data.SqlDbType.VarChar).Value = "Parent";
                else polecenie.Parameters.Add("@Role", System.Data.SqlDbType.VarChar).Value = "Child";
                polecenie.CommandType = CommandType.Text;
                connection.Open();
                polecenie.ExecuteNonQuery();


            }
            catch
            {
                message.ShowMessage("Błąd", "Podczas połączenia z serwerem", "error");
            }
            finally
            {
                message.ShowMessage("Gratulacje", "Pomyślnie zarejestrowano", "succes");
                connection.Close();
            }
            this.Close();
        }
        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private void Cbx_hidepassword_Changed(object sender, RoutedEventArgs e)
        {
            if (cbx_hidepassword.IsChecked == true)
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

        private void KeyDown_on_tbx(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.PageUp || e.Key == Key.Up) tbx_username.Focus();
            if (e.Key == Key.PageDown || e.Key == Key.Down)
            {
                if (cbx_hidepassword.IsChecked == true) pbx_password.Focus();
                else tbx_password.Focus();
            }
            
            if (e.Key == Key.Escape) { tbx_username.Clear(); pbx_password.Clear(); tbx_username.Focus(); }
        }
    }
 }

