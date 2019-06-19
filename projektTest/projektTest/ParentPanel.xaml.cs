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
            CenterWindowOnScreen();
        }

        public ParentPanel(int _id, SqlConnection _conn)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            id = _id;
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

        int id=0;
        SqlConnection connection;
        Message message = new Message();



        private void Initialization()
        {
            string login="", haslo="", rola="";
            SqlCommand polecenie = new SqlCommand("SELECT Role, Login, Password FROM Logowanie WHERE ID_USER=@id", connection);
            polecenie.Parameters.Add("id", System.Data.SqlDbType.Int).Value = id;
            SqlDataReader czytnik = polecenie.ExecuteReader();

            while (czytnik.Read())
            {
                login = czytnik["Login"].ToString();
                haslo = czytnik["Password"].ToString();
                rola = czytnik["Role"].ToString();
            }

           this.Title = "Rodzic mode("+id+"): " + login + "     |     " + haslo + "     |     " + rola;
        }

        private void Btn_Categories_Click(object sender, RoutedEventArgs e)
        {
            CategoryEdit window = new CategoryEdit(connection);
            window.ShowDialog();
            FillCategories();
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

                czytnik.Close();

            }
            catch
            {
                message.ShowMessage("Błąd", "Podczas pobierania listy kategorii z serwera wystąpił nagły błąd.", false);
            }
        }
    }
}
