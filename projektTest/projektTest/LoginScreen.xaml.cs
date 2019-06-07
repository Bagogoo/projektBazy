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

            string login = txtUsername.Text;
            string password = txtPass.Text;

            SqlCommand polecenie = new SqlCommand("SELECT Role, ID_USER FROM Logowanie WHERE Login=@login AND Password=@password", connection);
            polecenie.Parameters.Add("login", System.Data.SqlDbType.VarChar).Value = login;
            polecenie.Parameters.Add("password", System.Data.SqlDbType.VarChar).Value = password;
            SqlDataReader czytnik = polecenie.ExecuteReader();

            string role = "-"; //P - parent / C - child
            int identyficator = 0;

            while (czytnik.Read())
            {
                role = czytnik["Role"].ToString();
                identyficator = (int)czytnik["ID_USER"];
            }

            czytnik.Close();

            if(role == "Parent")
            {
                ParentPanel controlpanel = new ParentPanel(identyficator, connection);
                this.Hide();
                controlpanel.ShowDialog();
                this.Show();
            }
            else if (role == "Child")
            {
                ChildPanel controlpanel = new ChildPanel(identyficator, connection);
                this.Hide();
                controlpanel.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Podano niepoprawne dane logowania!");
            }


            //po podaniu loginu i hasla wysyłane zostanie polecenie do bazy aby sprawdziło kto jest pod tymi danymi

            //jak to jest dziecko to otwiera panel dziecka wczytując odpowiednią osobę

            //jak to rodzic to wczytuje panel rodzica wczytując odpowiednią osobę a jak dziadkowie to wczytuje dziadków:

            //kolejnie otwiera odpowiednie okno w zależności od tego jaką role rodzinną wczytało wysyłając w konstruktorze dane a okno logowania ukrywa
        }

    }
}
