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

namespace projektTest
{
    /// <summary>
    /// Logika interakcji dla klasy CategoryEdit.xaml
    /// </summary>
    public partial class CategoryEdit : Window
    {
        public CategoryEdit()
        {
            InitializeComponent();
            ReadFromFile();
            RefreshListbox();
        }

        List<Category> categories = new List<Category>();
        Message message = new Message();

        private void ReadFromFile()
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

                foreach (Category val in tmp) lbx_category.Items.Add(val.get_category());
            }
            else
            {
                message.ShowMessage("Błąd", "Brak pliku z kategoriami", false);
                if (!Directory.Exists(path_directory))
                {
                    Directory.CreateDirectory(path_directory);
                    message.ShowMessage("Tworzenie", "Tworzenie kataloku z kategoriami w MojeDomumenty", true);
                }
                message.ShowMessage("Tworzenie", "Tworzenie pliku z kategoriami w MojeDomumenty", true);
                FileStream str = new FileStream(path, FileMode.Create);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Category>));

                serializer.Serialize(str, categories);
                str.Close();
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
                string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                path += @"\FamilyCrDatabase\Categories.xml";

                FileStream str = new FileStream(path, FileMode.Create);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Category>));

                serializer.Serialize(str, categories);
                str.Close();
                message.ShowMessage("Powodzenie", "Zapisano wprowadzone zmiany", true);
            }
            catch
            {
                message.ShowMessage("Błąd", "Nie udało się zapisać wprowadzonych zmian", false);
            }
            this.Close();
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
