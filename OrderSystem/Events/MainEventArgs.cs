using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Events
{
    public abstract class MainEventArgs : EventArgs
    {
        private string action;

        public MainEventArgs(string action)
        {
            this.action = action;
        }

        public string Action
        {
            get
            {
                return action;
            }
            private set
            {
                action = value;
            }
        }
    }
}
