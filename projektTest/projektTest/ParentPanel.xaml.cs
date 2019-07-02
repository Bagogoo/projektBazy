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
using System.Windows.Shapes;

namespace projektTest
{
    /// <summary>
    /// Logika interakcji dla klasy ParentPanel.xaml
    /// </summary>
    public partial class ParentPanel : Window
    {
        public ParentPanel()
        {
            InitializeComponent();
            FillCategories();
            FillChilds();
            CenterWindowOnScreen();
            RefreshCreditsInfo();
            RefreshOperationHistory();
        }

        public ParentPanel(int _id, SqlConnection _conn)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            id_user = _id;
            connection = _conn;
            Initialization();
            FillCategories();
            FillChilds();
            RefreshCreditsInfo();
            RefreshOperationHistory();
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

        int id_user = 0;
        int id_account = 0;
        string login = "", haslo = "", rola = "";
        SqlConnection connection;
        Message message = new Message();


        private void Initialization()
        {
            cal_calendar.SelectedDate = DateTime.Now.Date;
            connection.Open();
            
            SqlCommand polecenie = new SqlCommand("SELECT Role, Login, Password FROM Logowanie WHERE ID_USER=@id", connection);
            polecenie.Parameters.Add("id", System.Data.SqlDbType.Int).Value = id_user;
            SqlDataReader czytnik = polecenie.ExecuteReader();

            while (czytnik.Read())
            {
                login = czytnik["Login"].ToString();
                haslo = czytnik["Password"].ToString();
                rola = czytnik["Role"].ToString();
            }
            czytnik.Close();

            //odczytywanie identyfikatora konta dla identyfikatora użytkowanika
            SqlCommand polecenie2 = new SqlCommand("SELECT ID_ACCOUNT FROM Account WHERE ID_USER=@id", connection);
            polecenie2.Parameters.Add("id", System.Data.SqlDbType.Int).Value = id_user;
            czytnik = polecenie2.ExecuteReader();

            while (czytnik.Read())
            {
                id_account = (int)czytnik["ID_ACCOUNT"];
            }
            czytnik.Close();
            connection.Close();

            this.Title = "Rodzic mode("+ id_user + "/" + id_account +"): " + login + "     |     " + haslo + "     |     " + rola;

           
        }

        private void Btn_Categories_Click(object sender, RoutedEventArgs e)
        {
            CategoryEdit window = new CategoryEdit(connection);
            window.ShowDialog();
            FillCategories();
        }

        private void FillChilds()
        {
            cbx_childPay.Items.Clear();
            try
            {
                connection.Open();

                SqlCommand polecenie = new SqlCommand("SELECT Login FROM Logowanie WHERE Role='Child'", connection);
                SqlDataReader czytnik = polecenie.ExecuteReader();

                while (czytnik.Read())
                {
                    cbx_childPay.Items.Add(czytnik["Login"].ToString());
                }

                connection.Close();

            }
            catch
            {
                message.ShowMessage("Błąd", "Podczas pobierania informacji o dzieciach wystąpił błąd", "error");
            }
        }

        private void FillCategories()
        {

            cbx_category.Items.Clear();
            try
            {
                connection.Open();

                SqlCommand polecenie = new SqlCommand("SELECT Category FROM Categories", connection);
                SqlDataReader czytnik = polecenie.ExecuteReader();

                while (czytnik.Read())
                {
                    cbx_category.Items.Add(czytnik["Category"].ToString());
                }

                connection.Close();

            }
            catch
            {
                message.ShowMessage("Błąd", "Podczas pobierania listy kategorii z serwera wystąpił nagły błąd.", "error");
            }
        }

        private void RefreshOperationHistory()
        {
            lbx_history.Items.Clear();

            string name = "", category = "";
            DateTime date;
            double money;

            connection.Open();
            SqlCommand polecenie = new SqlCommand("SELECT Name, Category, Amount, Date FROM Operation WHERE ID_ACCOUNT=@id", connection);
            polecenie.Parameters.Add("id", System.Data.SqlDbType.Int).Value = id_account;
            SqlDataReader czytnik = polecenie.ExecuteReader();

            while (czytnik.Read())
            {
                name = czytnik["Name"].ToString();
                category = czytnik["Category"].ToString();
                money = Double.Parse(czytnik["Amount"].ToString());
                date = (DateTime)czytnik["Date"];

                lbx_history.Items.Add(name + ": " + money + "zł, " + string.Format("{0:dd-MM-yyyy}", date) + " (" + category+")");
            }
            connection.Close();
        }

        private void RefreshCreditsInfo()
        {
            double profit = 0, expenses = 0, balance = 0, tmp = 0;

            connection.Open();
            SqlCommand polecenie = new SqlCommand("SELECT Amount FROM Operation WHERE ID_ACCOUNT=@id", connection);
            polecenie.Parameters.Add("id", System.Data.SqlDbType.Int).Value = id_account;
            SqlDataReader czytnik = polecenie.ExecuteReader();

            while (czytnik.Read())
            {
                tmp = Double.Parse(czytnik["Amount"].ToString());

                if (tmp >= 0) profit += tmp;
                else if (tmp < 0) expenses += Math.Abs(tmp);
            }

            if (profit >= expenses) lbl_saldo.Foreground = Brushes.White;
            else lbl_saldo.Foreground = Brushes.Red;

            balance = profit - expenses;
            lbl_saldo.Content = balance;
            lbl_expenses.Content = expenses;
            lbl_revenues.Content = profit;

            connection.Close();
        }

        private void chkbx_childPay_Checked(object sender, RoutedEventArgs e)
        {
            if((bool)chkbx_childPay.IsChecked)
            {
                cbx_childPay.IsEnabled = true;
                cbx_category.IsEnabled = false;
            }
            else
            {
                cbx_childPay.IsEnabled = false;
                cbx_category.IsEnabled = true;
            }
        }

        private void btn_addTransaction_Click(object sender, RoutedEventArgs e)
        {
            double tmp_money = 0;
            try
            {
                tmp_money = Double.Parse(tbx_price.Text);
            }
            catch
            {
                message.ShowMessage("Błąd", "Podano niewłaściwy format wpłaty", "error");
                tmp_money = 0;
            }
            if((bool)rbtn_payin.IsChecked)
            {
                tmp_money = Math.Abs(tmp_money);
            }
            else if((bool)rbtn_payout.IsChecked)
            {
                tmp_money = Math.Abs(tmp_money) * -1;
            }



            try
            {
                connection.Open();

                SqlCommand sql_add_command = new SqlCommand("INSERT INTO Operation(ID_ACCOUNT, Amount, Category, Date, Name) VALUES(@tmp_id, @tmp_amount, @tmp_category, @tmp_date, @tmp_name)", connection);
                sql_add_command.Parameters.Add("tmp_id", System.Data.SqlDbType.Int).Value = id_account;
                sql_add_command.Parameters.Add("tmp_amount", System.Data.SqlDbType.Money).Value = tmp_money;
                if (!(bool)chkbx_childPay.IsChecked) sql_add_command.Parameters.Add("tmp_category", System.Data.SqlDbType.VarChar).Value = cbx_category.SelectedItem.ToString();
                else sql_add_command.Parameters.Add("tmp_category", System.Data.SqlDbType.VarChar).Value = "Kieszonkowe do "+ cbx_childPay.SelectedItem.ToString();
                sql_add_command.Parameters.Add("tmp_date", System.Data.SqlDbType.Date).Value = cal_calendar.SelectedDate;
                sql_add_command.Parameters.Add("tmp_name", System.Data.SqlDbType.VarChar).Value = tbx_name.Text;
                sql_add_command.ExecuteNonQuery();

                if ((bool)chkbx_childPay.IsChecked)
            {
                int child_id = -1;
                SqlCommand polecenie = new SqlCommand("SELECT ID_ACCOUNT FROM Account INNER JOIN Logowanie ON Account.ID_USER=Logowanie.ID_USER WHERE Logowanie.Login=@tmp_val", connection);
                polecenie.Parameters.Add("tmp_val", System.Data.SqlDbType.VarChar).Value = cbx_childPay.SelectedItem.ToString();
                SqlDataReader czytnik = polecenie.ExecuteReader();

                while (czytnik.Read())
                {
                    child_id = (int)czytnik["ID_ACCOUNT"];
                }
                czytnik.Close();


                SqlCommand sql_add_command_to_child = new SqlCommand("INSERT INTO Operation(ID_ACCOUNT, Amount, Category, Date, Name) VALUES(@tmp_id, @tmp_amount, @tmp_category, @tmp_date, @tmp_name)", connection);
                sql_add_command_to_child.Parameters.Add("tmp_id", System.Data.SqlDbType.Int).Value = child_id;
                sql_add_command_to_child.Parameters.Add("tmp_amount", System.Data.SqlDbType.Money).Value = Math.Abs(tmp_money);
                sql_add_command_to_child.Parameters.Add("tmp_category", System.Data.SqlDbType.VarChar).Value = "Przelew od rodzica";
                sql_add_command_to_child.Parameters.Add("tmp_date", System.Data.SqlDbType.Date).Value = cal_calendar.SelectedDate;
                sql_add_command_to_child.Parameters.Add("tmp_name", System.Data.SqlDbType.VarChar).Value = "Przelew od " + login;
                sql_add_command_to_child.ExecuteNonQuery();
            }


            connection.Close();
                message.ShowMessage("Powodzenie", "Zapisano wprowadzone zmiany", "succes");


                tbx_name.Clear();
                tbx_price.Clear();
                rbtn_payout.IsChecked = true;
                chkbx_childPay.IsChecked = false;
                cbx_childPay.IsEnabled = false;
                cal_calendar.SelectedDate = DateTime.Now.Date;

                RefreshCreditsInfo();
                RefreshOperationHistory();
        }
             catch
             {
                 message.ShowMessage("Błąd", "Nie udało się dodać operacji", "error");
             }
}
    }
}
