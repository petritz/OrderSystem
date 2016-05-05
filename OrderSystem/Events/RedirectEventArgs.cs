using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Events
{
    /// <summary>
    /// Event arguments for the redirect event
    /// </summary>
    public class RedirectEventArgs : MainEventArgs
    {
        private string view;

        public RedirectEventArgs(string view) : base("redirect")
        {
            this.view = view;
        }

        /// <summary>
        /// The view to redirect to
        /// </summary>
        public string View
        {
            get { return view; }
            set { view = value; }
        }
    }
}