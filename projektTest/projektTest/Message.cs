using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektTest
{
    class Message
    {
        public void ShowMessage(string head, string body, bool type)
        {
            Message_window window = new Message_window(head, body, type);
            window.ShowDialog();
        }
    }
}
