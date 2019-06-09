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
            float money;

            SqlCommand polecenie = new SqlCommand("SELECT Name, Category, Amount, Date FROM Operation WHERE ID_ACCOUNT=@id", connection);
            polecenie.Parameters.Add("id", System.Data.SqlDbType.Int).Value = id_account;
            SqlDataReader czytnik = polecenie.ExecuteReader();

            while (czytnik.Read())
            {
                name = czytnik[""].ToString();
                category = czytnik["Category"].ToString();
                money = (float)czytnik["Amount"];
                date = (DateTime)czytnik["Date"];

                lbx_history.Items.Add(name + money + date + category);
            }
        }

        private void RefreshCreditsInfo()
        {
            float profit = 0, expenses = 0, balance = 0, tmp = 0;
            SqlCommand polecenie = new SqlCommand("SELECT Amount FROM Operation WHERE ID_ACCOUNT=@id", connection);
            polecenie.Parameters.Add("id", System.Data.SqlDbType.Int).Value = id_account;
            SqlDataReader czytnik = polecenie.ExecuteReader();

            while (czytnik.Read())
            {
                tmp = (float)czytnik["Amount"];

                if (tmp >= 0) profit += tmp;
                else if (tmp < 0) expenses += Math.Abs(tmp);
            }

            if (profit >= expenses) lbl_saldo.Foreground = Brushes.Black;
            else lbl_saldo.Foreground = Brushes.Red;

            balance = profit - expenses;
            lbl_saldo.Content = balance;
            lbl_expenses.Content = expenses;
            lbl_revenues.Content = profit;

        }







    }
}
