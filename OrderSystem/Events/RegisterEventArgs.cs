using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Events
{
    public class RegisterEventArgs : MainEventArgs
    {
        private bool success;

        public RegisterEventArgs(bool success) : base("register")
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