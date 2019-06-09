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
using System.IO;
using System.Xml.Serialization;

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
        }

        public ChildPanel(int _id, SqlConnection _conn)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            id_user = _id;
            connection = _conn;
            Initialization();
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
        List<Category> categories = new List<Category>();


        private void Initialization()
        {
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

            this.Title = "Bajtel mode(" + id_user + "): " + login + "     |     " + haslo + "     |     " + rola;
            
            LoadCategories();
            RefreshOperationHistory();
            RefreshCreditsInfo();

            //odczytywanie identyfikatora konta dla identyfikatora użytkowanika
            SqlCommand polecenie2 = new SqlCommand("SELECT ID_ACCOUNT FROM Account WHERE ID_USER=@id", connection);
            polecenie2.Parameters.Add("id", System.Data.SqlDbType.Int).Value = id_user;
            czytnik = polecenie2.ExecuteReader();

            while (czytnik.Read())
            {
                id_account = (int)czytnik["ID_ACCOUNT"];
            }
            connection.Close();
        }

        private void LoadCategories()
        {
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path_directory = path + @"\FamilyCrDatabase";
            path += @"\FamilyCrDatabase\Categories.xml";




            if (File.Exists(path))
            {
                FileStream str = new FileStream(path, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Category>));

                List<Category> tmp = (List<Category>)serializer.Deserialize(str);
                str.Close();

                foreach (Category val in tmp) cbx_category.Items.Add(val.get_category());
            }
            else
            {
                MessageBox.Show("Brak pliku z kategoriami!");
                if (!Directory.Exists(path_directory))
                {
                    Directory.CreateDirectory(path_directory);
                }
                FileStream str = new FileStream(path, FileMode.Create);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Category>));

                serializer.Serialize(str, categories);
                str.Close();
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
                name = czytnik[""].ToString();
                category = czytnik["Category"].ToString();
                money = (double)czytnik["Amount"];
                date = (DateTime)czytnik["Date"];

                lbx_history.Items.Add(name + money + date + category);
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
                tmp = (double)czytnik["Amount"];

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
        }

        private void ClearFields()
        {
            tbx_name.Clear();
            tbx_price.Clear();
            cbx_category.SelectedIndex = 1;
        }




















    }
}
