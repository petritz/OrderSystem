using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Events
{
    /// <summary>
    /// Event arguments for the register event.
    /// </summary>
    public class RegisterEventArgs : MainEventArgs
    {
        private bool success;

        public RegisterEventArgs(bool success) : base("register")
        {
            this.success = success;
        }

        /// <summary>
        /// Determines if the registration was successfull or not.
        /// </summary>
        public bool Success
        {
            get { return success; }
            set { success = value; }
        }
    }
}