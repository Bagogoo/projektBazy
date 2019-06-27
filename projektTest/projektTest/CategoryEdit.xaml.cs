using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;

namespace projektTest
{
    /// <summary>
    /// Logika interakcji dla klasy CategoryEdit.xaml
    /// </summary>
    public partial class CategoryEdit : Window
    {
        public CategoryEdit(SqlConnection _con)
        {
            connection = _con;
            InitializeComponent();
            ReadData();
            CenterWindowOnScreen();
        }

        SqlConnection connection;
        Message message = new Message();


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

        private void ReadData()
        {
            try
            {
                connection.Open();

                SqlCommand polecenie = new SqlCommand("SELECT Category FROM Categories", connection);
                SqlDataReader czytnik = polecenie.ExecuteReader();

                while (czytnik.Read())
                {
                    lbx_category.Items.Add(czytnik["Category"].ToString());
                }

                connection.Close();
            }
            catch
            {
                message.ShowMessage("Błąd", "Podczas pobierania listy kategorii z serwera wystąpił nagły błąd.", "error");
            }
        }

        private void Btn_addNewCategory_Click(object sender, RoutedEventArgs e)
        {
            if(tbx_categoryName.Text != "") lbx_category.Items.Add(tbx_categoryName.Text);
            tbx_categoryName.Clear();
            tbx_categoryName.Focus();
        }

        private void Btn_removeCategory_Click(object sender, RoutedEventArgs e)
        {
            if (lbx_category.SelectedIndex != -1) lbx_category.Items.Remove(lbx_category.SelectedItem);
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand sql_command_clear = new SqlCommand("DELETE FROM Categories", connection);
                sql_command_clear.ExecuteNonQuery();

                for (int clk = 0; clk < lbx_category.Items.Count; clk++)
                {
                    SqlCommand sql_add_command = new SqlCommand("INSERT INTO Categories(Category) VALUES(@tmp_val)", connection);
                    sql_add_command.Parameters.Add("tmp_val", System.Data.SqlDbType.VarChar).Value = lbx_category.Items[clk].ToString();
                    sql_add_command.ExecuteNonQuery();
                }

                connection.Close();
                message.ShowMessage("Powodzenie", "Zapisano wprowadzone zmiany", "succes");
            }
            catch
            {
                message.ShowMessage("Błąd", "Podczas zapisywania kategorii na serwerze.", "error");
            }
            this.Close();
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }
    }
}
