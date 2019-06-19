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
            ReadFromFile();
            RefreshListbox();
        }

        List<Category> categories = new List<Category>();
        SqlConnection connection;
        Message message = new Message();

        private void ReadFromFile()
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

                czytnik.Close();

            }
            catch
            {
                message.ShowMessage("Błąd", "Podczas pobierania listy kategorii z serwera wystąpił nagły błąd.", false);
            }
        }

        private void RefreshListbox()
        {
            lbx_category.Items.Clear();
            foreach (var val in categories) lbx_category.Items.Add(val.get_category());
        }

        private void Btn_addNewCategory_Click(object sender, RoutedEventArgs e)
        {
            if(tbx_categoryName.Text != "") categories.Add(new Category(tbx_categoryName.Text));
            tbx_categoryName.Clear();
            tbx_categoryName.Focus();
            RefreshListbox();
        }

        private void Btn_removeCategory_Click(object sender, RoutedEventArgs e)
        {
            if(lbx_category.SelectedIndex != -1) categories.RemoveAt(lbx_category.SelectedIndex);
            RefreshListbox();
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();

                SqlCommand sql_command_clear = new SqlCommand("DELETE FROM Categories", connection);
                sql_command_clear.ExecuteNonQuery();

                foreach (var val in lbx_category.Items.ToString())
                {
                    SqlCommand sql_add_command = new SqlCommand("INSERT INTO Categories('Category') VALUES(@tmp_val)", connection);
                    sql_add_command.Parameters.Add("tmp_val", System.Data.SqlDbType.VarChar).Value = val;
                    sql_add_command.ExecuteNonQuery();
                }

                connection.Close();
                message.ShowMessage("Powodzenie", "Zapisano wprowadzone zmiany", true);
            }
            catch
            {
                message.ShowMessage("Błąd", "Podczas zapisywania kategorii na serwerze.", false);
            }
            this.Close();
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
