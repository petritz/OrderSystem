using OrderSystem.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrderSystem.Views
{
    public class MainPage : Page
    {
        public delegate void MainEventHandler(object sender, MainEventArgs e);

        public event MainEventHandler DefaultEvent;

        public MainPage()
        {
        }

        protected virtual void OnEvent(MainEventArgs e)
        {
            if (DefaultEvent != null) DefaultEvent(this, e);
        }
    }
}