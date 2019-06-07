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
            string connectionString = "Data Source=projektbazy.database.windows.net;Initial Catalog=BazaDziennik;User ID=projektbazy;Password=zaq1@WSX;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand polecenie = new SqlCommand("SELECT * FROM dziennik", connection);
            SqlDataReader czytnik = polecenie.ExecuteReader();

            string temp = "";
            while (czytnik.Read())
            {
                temp += "Login: "+czytnik["Logowanie"].ToString() + "\t";
                temp += "Hasło: "+czytnik["Haslo"].ToString() + "\t";
                temp += czytnik["Test"].ToString() + "\n";
            }

            czytnik.Close();
            MessageBox.Show(temp);

            //po podaniu loginu i hasla wysyłane zostanie polecenie do bazy aby sprawdziło kto jest pod tymi danymi

            //jak to jest dziecko to otwiera panel dziecka wczytując odpowiednią osobę

            //jak to rodzic to wczytuje panel rodzica wczytując odpowiednią osobę a jak dziadkowie to wczytuje dziadków:

            //kolejnie otwiera odpowiednie okno w zależności od tego jaką role rodzinną wczytało wysyłając w konstruktorze dane a okno logowania ukrywa
        }
        
    }
}
