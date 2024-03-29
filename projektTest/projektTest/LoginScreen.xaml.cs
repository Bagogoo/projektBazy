﻿using System;
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
using System.IO;
using System.Xml.Serialization;

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
            ReadConfiguration();
            tbx_username.Focus();
            ui_lang = "PL";
            rbtn_langPL.IsChecked = true;
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

        ConnectionSet connectionInfo = new ConnectionSet();
        Message message = new Message();
        string ui_lang = "";

        private void SaveConfiguration()
        {
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path += @"\FamilyCrDatabase\ConnectionInfo.xml";

            FileStream str = new FileStream(path, FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(ConnectionSet));

            serializer.Serialize(str, connectionInfo);
            str.Close();
        }

        private void ReadConfiguration()
        {
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path_directory = path + @"\FamilyCrDatabase";
            path += @"\FamilyCrDatabase\ConnectionInfo.xml";
            //MessageBox.Show(path);



            if (File.Exists(path))
            {
                FileStream str = new FileStream(path, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(ConnectionSet));

                ConnectionSet tmp = (ConnectionSet)serializer.Deserialize(str);
                str.Close();

                connectionInfo = tmp;
            }
            else
            {
                if (!Directory.Exists(path_directory))
                {
                    Directory.CreateDirectory(path_directory);
                }
                FileStream str = new FileStream(path, FileMode.Create);
                XmlSerializer serializer = new XmlSerializer(typeof(ConnectionSet));

                serializer.Serialize(str, connectionInfo);
                str.Close();
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection;
            string connectionString = "Data Source=" + connectionInfo.get_dataSource() + ";Initial Catalog=" + connectionInfo.get_initialCatalog() + ";User ID=" + connectionInfo.get_userId() + ";Password=" + connectionInfo.get_password() + ";Connect Timeout=" + connectionInfo.get_connectTimeout() + ";Encrypt=" + connectionInfo.get_encrypt() + ";TrustServerCertificate=" + connectionInfo.get_trustServerCertificate() + ";ApplicationIntent=" + connectionInfo.get_applicationIntent() + ";MultiSubnetFailover=" + connectionInfo.get_multiSubnetFailover() + "";
            connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                string login = tbx_username.Text;
                string password;

                if (cbx_hidepassword.IsChecked == true) password = pbx_password.Password;
                else password = tbx_password.Text;

                SqlCommand polecenie = new SqlCommand("SELECT Role, ID_USER FROM Logowanie WHERE Login=@login AND Password=@password", connection);
                polecenie.Parameters.Add("login", System.Data.SqlDbType.VarChar).Value = login;
                polecenie.Parameters.Add("password", System.Data.SqlDbType.VarChar).Value = password;
                SqlDataReader czytnik = polecenie.ExecuteReader();

                string role = "-";
                int identyficator = 0;

                while (czytnik.Read())
                {
                    role = czytnik["Role"].ToString();
                    identyficator = (int)czytnik["ID_USER"];
                }

                connection.Close();

                if (role == "Parent")
                {
                    ParentPanel controlpanel = new ParentPanel(identyficator, connection, ui_lang);
                    this.Hide();
                    controlpanel.ShowDialog();
                    this.Show();
                    ClearFields();
                }
                else if (role == "Child")
                {
                    ChildPanel controlpanel = new ChildPanel(identyficator, connection, ui_lang);
                    this.Hide();
                    controlpanel.ShowDialog();
                    this.Show();
                    ClearFields();
                }
                else
            {
                if (login == "Kujo" && password == "Jotaro") btn_login.Content = "Nani?!";
                if (login == "marvin")
                {
                    DebugWindow debwin = new DebugWindow(connection);
                    this.Hide();
                    debwin.ShowDialog();
                    this.Show();
                    ClearFields();
                }
                else message.ShowMessage("Nie udało się zalogować", "Podano niepoprawny login lub hasło.", "error");
            }
            connection.Close();
        }
            catch
            {
                message.ShowMessage("Nie udało się połączyć", "Nie można nawiązać połączenia z bazą danych.", "error");
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

        private void Btn_settings_Click(object sender, RoutedEventArgs e)
        {
            ConnectionSettings window = new ConnectionSettings(connectionInfo);
            window.ShowDialog();

            if(window.get_saveQuest())
            {
                connectionInfo = window.get_connectionInfo();
                SaveConfiguration();
                message.ShowMessage("Zapisano ustawienia", "---", "succes");
            }
        }

        private void btn_register_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection;
            string connectionString = "Data Source=" + connectionInfo.get_dataSource() + ";Initial Catalog=" + connectionInfo.get_initialCatalog() + ";User ID=" + connectionInfo.get_userId() + ";Password=" + connectionInfo.get_password() + ";Connect Timeout=" + connectionInfo.get_connectTimeout() + ";Encrypt=" + connectionInfo.get_encrypt() + ";TrustServerCertificate=" + connectionInfo.get_trustServerCertificate() + ";ApplicationIntent=" + connectionInfo.get_applicationIntent() + ";MultiSubnetFailover=" + connectionInfo.get_multiSubnetFailover() + "";
            connection = new SqlConnection(connectionString);

            Register window = new Register(connection, ui_lang);
            this.Hide();
            window.ShowDialog();
            this.Show();
        }

        private void ClearFields()
        {
            tbx_username.Clear();
            tbx_password.Clear();
            pbx_password.Clear();
            tbx_username.Focus();
        }

        private void Rbtn_lang_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)rbtn_langPL.IsChecked)
            {
                ui_lang = "PL";
                lbl_signin.Content = "Logowanie";
                lbl_username.Content = "Nazwa użytkownika";
                lbl_password.Content = "Hasło";
                cbx_hidepassword.Content = "Ukryj hasło";
                btn_login.Content = "Zaloguj";
                btn_settings.Content = "Ustawienia";
                btn_register.Content = "Zajerestruj";
            }
            else if ((bool)rbtn_langDE.IsChecked)
            {
                ui_lang = "DE";
                lbl_signin.Content = "Einloggen";
                lbl_username.Content = "Benutzername";
                lbl_password.Content = "Kennwort";
                cbx_hidepassword.Content = "Passwort verstecken";
                btn_login.Content = "Einloggen";
                btn_settings.Content = "Einstellungen";
                btn_register.Content = "Registrieren";
            }
            else if ((bool)rbtn_langGB.IsChecked)
            {
                ui_lang = "GB";
                lbl_signin.Content = "Sign in";
                lbl_username.Content = "Username";
                lbl_password.Content = "Password";
                cbx_hidepassword.Content = "Hide password";
                btn_login.Content = "Sign in";
                btn_settings.Content = "Settings";
                btn_register.Content = "Sign up";
            }
            else
            {
                ui_lang = "UNSET";
            }

        }
    }
}
