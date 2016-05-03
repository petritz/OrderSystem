using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Events
{
    public class LoginEventArgs : MainEventArgs
    {
        private bool success;

        public LoginEventArgs(bool success) : base("login")
        {
            this.success = success;
        }
        
        public bool Success
        {
            get { return success; }
            set { success = value; }
        }
    }
}
