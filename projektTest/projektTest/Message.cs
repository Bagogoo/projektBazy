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

namespace projektTest
{
    class Message
    {
        /*Opcje typów okna:
         * "error" - błąd
         * "message" - powiadomienie
         * "succes" - powodzenie wykonania operacji
         */
        public void ShowMessage(string head, string body, string type)
        {
            Message_window window = new Message_window(head, body, type);
            window.ShowDialog();
        }
        public void ShowMessage(string head, string body, Brush bg, Brush tbx, Brush btn, Brush txt)
        {
            Message_window window = new Message_window(head, body, bg, tbx, btn, txt);
            window.ShowDialog();
        }
    }
}
