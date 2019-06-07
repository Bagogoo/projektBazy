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
        }

    


        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection;
            string connectionString = "Data Source=projektbazy.database.windows.net;Initial Catalog=BazaDziennik;User ID=projektbazy;Password=********;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand polecenie = new SqlCommand("SELECT * FROM dziennik", connection);
            SqlDataReader czytnik = polecenie.ExecuteReader();

            string temp = "";
            while (czytnik.Read())
            {
                temp += czytnik["Test"].ToString() + "\n";
            }

            czytnik.Close();
            MessageBox.Show(temp);
        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
