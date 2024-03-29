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
using System.Windows.Shapes;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Markup;

namespace projektTest
{
    /// <summary>
    /// Logika interakcji dla klasy ChildPanel.xaml
    /// </summary>
    public partial class ChildPanel : Window
    {
        public ChildPanel()
        {
            InitializeComponent();
            CenterWindowOnScreen();
            FillCategories();
            SetLang();
        }

        public ChildPanel(int _id, SqlConnection _conn, string _lang)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            id_user = _id;
            connection = _conn;
            Initialization();
            FillCategories();
            ui_lang = _lang;
            SetLang();
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
        SqlConnection connection;
        Message message = new Message();
        string ui_lang = "UNSET";
        private void Initialization()
        {
            cal_calendar.SelectedDate = DateTime.Now.Date;
            //ustawianie nagłówka okna
            connection.Open();
            string login = "", haslo = "", rola = "";
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
            connection.Close();

           



            RefreshOperationHistory();
            RefreshCreditsInfo();



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

                lbx_history.Items.Add(name + ": " + money + "zł, " + string.Format("{0:dd-MM-yyyy}", date) + " (" + category + ")");
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

        private void SetLang()
        {
            if (ui_lang == "PL")
            {
                this.Title = "Panel dziecka";
                btn_raport.Content = "Generuj raport";
                lbl_history.Content = "Historia operacji";
                lbl_newpay.Content = "Nowa operacja";
                lbl_name.Content = "Nazwa operacji";
                lbl_price.Content = "Kwota";
                lbl_category.Content = "Kategoria";
                btn_addPayment.Content = "Zatwierdź";
                lbl_expensesinfo.Content = "Wydatki:";
                lbl_revenuesinfo.Content = "Przychody:";
                lbl_saldoinfo.Content = "Saldo:";
                cal_calendar.Language = XmlLanguage.GetLanguage("pl-PL");
            }
            else if (ui_lang == "GB")
            {
                this.Title = "Child panel";
                btn_raport.Content = "Generate report";
                lbl_history.Content = "Operation history";
                lbl_newpay.Content = "New operation";
                lbl_name.Content = "Operation name";
                lbl_price.Content = "Amount";
                lbl_category.Content = "Category";
                btn_addPayment.Content = "Submit";
                lbl_expensesinfo.Content = "Expenses:";
                lbl_revenuesinfo.Content = "Revenues:";
                lbl_saldoinfo.Content = "Balance:";
                cal_calendar.Language = XmlLanguage.GetLanguage("en-US");
            }
            else if (ui_lang == "DE")
            {
                this.Title = "Child panel";
                btn_raport.Content = "Einen Bericht erstellen";
                lbl_history.Content = "Betriebsverlauf";
                lbl_newpay.Content = "Neuer Betrieb";
                lbl_name.Content = "Der Name der Operation";
                lbl_price.Content = "Betrag";
                lbl_category.Content = "Kategorie";
                btn_addPayment.Content = "Einreichen";
                lbl_expensesinfo.Content = "Kosten:";
                lbl_revenuesinfo.Content = "Einnahmen:";
                lbl_saldoinfo.Content = "Balance:";
                cal_calendar.Language = XmlLanguage.GetLanguage("de-DE");
            }
            else
            {
                lbl_history.Content = "Operation history";
                lbl_newpay.Content = "New operation";
                lbl_name.Content = "Operation name";
                lbl_price.Content = "Amount";
                lbl_category.Content = "Category";
                btn_addPayment.Content = "Submit";
                lbl_expensesinfo.Content = "Expenses:";
                lbl_revenuesinfo.Content = "Revenues:";
                lbl_saldoinfo.Content = "Balance:";
                cal_calendar.Language = XmlLanguage.GetLanguage("en-US");
            }

        }

        private void btn_addPayment_click(object sender, RoutedEventArgs e)
        {
            string name = tbx_name.Text;
            Double.TryParse(tbx_price.Text, out double price);
            price = Math.Abs(price);
            price *= -1;
            string category = cbx_category.SelectedItem.ToString();
            DateTime date = cal_calendar.SelectedDate.Value;

            connection.Open();
            SqlCommand polecenie = new SqlCommand("INSERT INTO Operation(ID_ACCOUNT, Name, Amount, Category, Date) VALUES(@id, @name, @amount, @category, @date)", connection);
            polecenie.Parameters.Add("id", System.Data.SqlDbType.Int).Value = id_account;
            polecenie.Parameters.Add("name", System.Data.SqlDbType.VarChar).Value = name;
            polecenie.Parameters.Add("amount", System.Data.SqlDbType.Money).Value = price;
            polecenie.Parameters.Add("category", System.Data.SqlDbType.VarChar).Value = category;
            polecenie.Parameters.Add("date", System.Data.SqlDbType.Date).Value = date;
            polecenie.ExecuteNonQuery();

            connection.Close();
            RefreshOperationHistory();
            RefreshCreditsInfo();
            ClearFields();
            cal_calendar.SelectedDate = DateTime.Now.Date;


        }

        private void ClearFields()
        {
            tbx_name.Clear();
            tbx_price.Clear();
            cbx_category.SelectedIndex = 1;
        }

        private void Btn_addCategory_Click(object sender, RoutedEventArgs e)
        {
            CategoryEdit window = new CategoryEdit(connection);
            window.ShowDialog();
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

        private void btn_raport_click(object sender, RoutedEventArgs e)
        {
            string name = "", category = "";
            DateTime date = Convert.ToDateTime("0/0/0000");
            double money = 0;
            try
            {
                FileStream fs = new FileStream("Raport.txt", FileMode.OpenOrCreate);
                StreamWriter plik = new StreamWriter(fs);
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


                }
                connection.Close();
                plik.WriteLine("Raport");
                plik.WriteLine(name + ": " + money + "zł, " + string.Format("{0:dd-MM-yyyy}", date) + " (" + category + ")");
                plik.Close();
            }
            catch
            {
                message.ShowMessage("Błąd", "Nie udało wygenerować się raportu.", "error");
            }

        }

        private void Lbx_history_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
