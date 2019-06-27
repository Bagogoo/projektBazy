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
using System.IO;
using System.Xml.Serialization;

namespace projektTest
{
    /// <summary>
    /// Logika interakcji dla klasy DebugWindow.xaml
    /// </summary>
    public partial class DebugWindow : Window
    {
        public DebugWindow()
        {
            InitializeComponent();
        }

        public DebugWindow(SqlConnection con)
        {
            InitializeComponent();
            connection = con;
        }

        Message message = new Message();
        SqlConnection connection;
        string loaded;

        private void Btn_fillLogowanie_Click(object sender, RoutedEventArgs e)
        {
            loaded = "Logowanie";
            string login, password, role;
            int id;
            lbx_contener.Items.Clear();
            try
            {
                connection.Open();

                SqlCommand polecenie = new SqlCommand("SELECT ID_USER, Login, Password, Role FROM Logowanie", connection);
                SqlDataReader czytnik = polecenie.ExecuteReader();

                while (czytnik.Read())
                {
                    login = czytnik["Login"].ToString();
                    password = czytnik["Password"].ToString();
                    role = czytnik["Role"].ToString();
                    id = (int)czytnik["ID_USER"];

                    lbx_contener.Items.Add("ID_USER: " + id.ToString() + "<>Login: " + login + "<>Password: " + password + "<>Role: " + role);
                }

                connection.Close();

            }
            catch
            {
                message.ShowMessage("Błąd", "Podczas pobierania listy kategorii z serwera wystąpił nagły błąd.", "error");
            }
        }

        private void Btn_fillAccount_Click(object sender, RoutedEventArgs e)
        {
            loaded = "Account";
            string type;
            int idu, ida;
            double money;
            lbx_contener.Items.Clear();
            try
            {
                connection.Open();

                SqlCommand polecenie = new SqlCommand("SELECT ID_ACCOUNT, ID_USER, Type, Balance FROM Account", connection);
                SqlDataReader czytnik = polecenie.ExecuteReader();

                while (czytnik.Read())
                {
                    type = czytnik["Tyoe"].ToString();
                    money = (double)czytnik["Balance"];
                    ida = (int)czytnik["ID_ACCOUNT"];
                    idu = (int)czytnik["ID_USER"];

                    lbx_contener.Items.Add("ID_USER: " + idu.ToString() + "<>ID_ACCOUNT: " + ida.ToString() + "<>Type: " + type + "<>Balance: " + money.ToString());
                }

                connection.Close();

            }
            catch
            {
                message.ShowMessage("Błąd", "Podczas pobierania listy kategorii z serwera wystąpił nagły błąd.", "error");
            }
        }

        private void Btn_fillCategories_Click(object sender, RoutedEventArgs e)
        {
            loaded = "Categories";
            string cat;
            int id;
            lbx_contener.Items.Clear();
            try
            {
                connection.Open();

                SqlCommand polecenie = new SqlCommand("SELECT Category_ID, Category FROM Categories", connection);
                SqlDataReader czytnik = polecenie.ExecuteReader();

                while (czytnik.Read())
                {
                    cat = czytnik["Category"].ToString();
                    id = (int)czytnik["Category_ID"];

                    lbx_contener.Items.Add("Category_ID: " + id.ToString() + "<>Category: " + cat);
                }

                connection.Close();

            }
            catch
            {
                message.ShowMessage("Błąd", "Podczas pobierania listy kategorii z serwera wystąpił nagły błąd.", "error");
            }
        }

        private void Btn_fillOperation_Click(object sender, RoutedEventArgs e)
        {
            loaded = "Operation";
            string name, cat;
            int ido, ida;
            double money;
            DateTime date;
            lbx_contener.Items.Clear();
            try
            {
                connection.Open();

                SqlCommand polecenie = new SqlCommand("SELECT ID_OPERATION, ID_ACCOUNT, Amount, Category, Date, Name FROM Operation", connection);
                SqlDataReader czytnik = polecenie.ExecuteReader();

                while (czytnik.Read())
                {
                    name = czytnik["Name"].ToString();
                    cat = czytnik["Category"].ToString();
                    money = (double)czytnik["Amount"];
                    date =(DateTime)czytnik["Date"];
                    ido = (int)czytnik["ID_OPERATION"];
                    ida = (int)czytnik["ID_USER"];

                    lbx_contener.Items.Add("ID_OPERATION: " + ido.ToString() + "<>ID_ACCOUNT: " + ida.ToString() + "<>Amount: " + money.ToString() + "<>Category: " + cat + "<>Date: " + date.ToString() + "<>Name: " + name);
                }

                connection.Close();

            }
            catch
            {
                message.ShowMessage("Błąd", "Podczas pobierania listy kategorii z serwera wystąpił nagły błąd.", "error");
            }
        }

        private void Btn_operationClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (loaded == "Logowanie")
                {
                    connection.Open();
                    SqlCommand sql_command_clear = new SqlCommand("DELETE FROM Logowanie", connection);
                    sql_command_clear.ExecuteNonQuery();
                    connection.Close();
                }
                else if (loaded == "Account")
                {
                    connection.Open();
                    SqlCommand sql_command_clear = new SqlCommand("DELETE FROM Account", connection);
                    sql_command_clear.ExecuteNonQuery();
                    connection.Close();
                }
                else if (loaded == "Operation")
                {
                    connection.Open();
                    SqlCommand sql_command_clear = new SqlCommand("DELETE FROM Operation", connection);
                    sql_command_clear.ExecuteNonQuery();
                    connection.Close();
                }
                else if (loaded == "Categories")
                {
                    connection.Open();
                    SqlCommand sql_command_clear = new SqlCommand("DELETE FROM Categories", connection);
                    sql_command_clear.ExecuteNonQuery();
                    connection.Close();
                }
                lbx_contener.Items.Clear();
                message.ShowMessage("Wykonano", "Wyczyszczono tabele " + loaded, "succes");
            }
            catch
            {
                message.ShowMessage("Błąd", "Nie udało się wyczyścić tabeli.", "error");
            }
        }

        private void Btn_operationRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (loaded == "Logowanie")
                {
                    //connection.Open();
                    //SqlCommand sql_command_clear = new SqlCommand("DELETE FROM Logowanie WHERE ID_USER=@tmp", connection);
                    //sql_command_clear.Parameters.Add("tmp", System.Data.SqlDbType.Int).Value = id_user;
                    //sql_command_clear.ExecuteNonQuery();
                    //connection.Close();

                    Btn_fillLogowanie_Click(sender, e);
                }
                else if (loaded == "Account")
                {


                    Btn_fillAccount_Click(sender, e);
                }
                else if (loaded == "Operation")
                {


                    Btn_fillOperation_Click(sender, e);
                }
                else if (loaded == "Categories")
                {


                    Btn_fillCategories_Click(sender, e);
                }
                lbx_contener.Items.Clear();
            }
            catch
            {
                message.ShowMessage("Błąd", "Nie udało się skasować rekordu w tabeli " + loaded, "error");
            }
        }

        private void Btn_testmbxMessage_Click(object sender, RoutedEventArgs e)
        {
            message.ShowMessage("TEST", "test is not real confirmation with imagination, becouse its onlu test :)" + loaded, "message");
        }

        private void Btn_testmbxFail_Click(object sender, RoutedEventArgs e)
        {
            message.ShowMessage("TEST", "test is not real confirmation with imagination, becouse its onlu test :)" + loaded, "error");
        }

        private void Btn_testmbxSucces_Click(object sender, RoutedEventArgs e)
        {
            message.ShowMessage("TEST", "test is not real confirmation with imagination, becouse its onlu test :)" + loaded, "succes");
        }

        private void Btn_testmbxUser_Click(object sender, RoutedEventArgs e)
        {
            message.ShowMessage("TEST", "test is not real confirmation with imagination, becouse its onlu test :)" + loaded, "message");
        }
    }
}
