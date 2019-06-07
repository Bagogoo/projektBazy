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

        string server = "mysql1.ugu.pl";
        string database = "db696467";
        string user = "db696467";
        string password = "zaq1@WSX";
        SqlConnection connection;


        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "SERVER=" + server + ";DATABASE=" + database + ";UID=" + user + ";PASSWORD=" + password + ";";
            connection = new SqlConnection(connectionString);

            SqlCommand polecenie = new SqlCommand("SELECT * FROM testowanko", connection);
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
