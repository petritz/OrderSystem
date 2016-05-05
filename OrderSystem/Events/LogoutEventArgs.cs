using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Events
{
    /// <summary>
    /// Event arguments for the logout event.
    /// </summary>
    public class LogoutEventArgs : MainEventArgs
    {
        public LogoutEventArgs() : base("logout")
        {
        }
    }
}