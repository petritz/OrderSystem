using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Events
{
    public class RedirectEventArgs : MainEventArgs
    {
        private string view;

        public RedirectEventArgs(string view) : base("redirect")
        {
            this.view = view;
        }

        public string View
        {
            get { return view; }
            set { view = value; }
        }
    }
}