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
        public Register(SqlConnection _conn)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            connection = _conn;

            cbx_type.Items.Add("Portfel");
            cbx_type.Items.Add("Konto bankowe");
            cbx_type.SelectedIndex = 1;
        }

        SqlConnection connection;
        Message message = new Message();
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand command_add_login = new SqlCommand("INSERT INTO Logowanie (Login,Password,Role) VALUES (@Login,@Password,@Role)", connection);
                command_add_login.Parameters.Add("@Login", System.Data.SqlDbType.VarChar).Value = tbx_username.Text;
                if ((bool)cbx_hidepassword.IsChecked) command_add_login.Parameters.Add("@Password", System.Data.SqlDbType.VarChar).Value = pbx_password.Password;
                else command_add_login.Parameters.Add("@Password", System.Data.SqlDbType.VarChar).Value = tbx_password.Text;
                if (cbx_role.IsChecked == true) command_add_login.Parameters.Add("Role", System.Data.SqlDbType.VarChar).Value = "Parent";
                else command_add_login.Parameters.Add("@Role", System.Data.SqlDbType.VarChar).Value = "Child";
                command_add_login.CommandType = CommandType.Text;
                command_add_login.ExecuteNonQuery();

                SqlCommand command_read_iduser = new SqlCommand("SELECT ID_USER FROM Logowanie WHERE Login=@login AND Password=@password", connection);
                command_read_iduser.Parameters.Add("login", System.Data.SqlDbType.VarChar).Value = tbx_username.Text;
                if ((bool)cbx_hidepassword.IsChecked) command_read_iduser.Parameters.Add("@Password", System.Data.SqlDbType.VarChar).Value = pbx_password.Password;
                else command_read_iduser.Parameters.Add("@Password", System.Data.SqlDbType.VarChar).Value = tbx_password.Text;
                SqlDataReader czytnik = command_read_iduser.ExecuteReader();

                int new_uid = -1;

                while (czytnik.Read())
                {
                    new_uid = (int)czytnik["ID_USER"];
                }

                czytnik.Close();


                SqlCommand command_add_account = new SqlCommand("INSERT INTO Account(ID_USER,Type,Balance) VALUES(@tmp_idu,@tmp_type,@tmp_bala)", connection);
                command_add_account.Parameters.Add("@tmp_idu", System.Data.SqlDbType.Int).Value = new_uid;
                command_add_account.Parameters.Add("@tmp_type", System.Data.SqlDbType.VarChar).Value = cbx_type.SelectedItem.ToString();
                command_add_account.Parameters.Add("@tmp_bala", System.Data.SqlDbType.Money).Value = 0;
                command_add_account.CommandType = CommandType.Text;
                command_add_account.ExecuteNonQuery();

                connection.Close();
                message.ShowMessage("Gratulacje", "Pomyślnie zarejestrowano", "succes");
            }
            catch
            {
                message.ShowMessage("Błąd", "Podczas połączenia z serwerem", "error");
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

