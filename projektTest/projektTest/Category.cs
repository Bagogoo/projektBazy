using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace projektTest
{
    public class Category
    {
        public Category() { category = ""; }
        public Category(string val) { category = val; }

        [XmlAttribute]
        private string category;

        public string get_category() { return category; }
        public void set_category(string val) { category = val; }
    }
}
