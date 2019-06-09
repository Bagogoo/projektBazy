using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektTest
{
    class Message
    {
        public void ShowMessage(string head, string body)
        {
            Message_window window = new Message_window(head, body);
            window.ShowDialog();
        }
    }
}
