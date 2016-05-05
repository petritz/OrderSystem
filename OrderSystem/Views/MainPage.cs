using OrderSystem.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrderSystem.Views
{
    /// <summary>
    /// The main page for every page displayed in the root window.
    /// </summary>
    public class MainPage : Page
    {
        /// <summary>
        /// Delegate method for the events
        /// </summary>
        /// <param name="sender">The sender of this event</param>
        /// <param name="e">The event arguments</param>
        public delegate void MainEventHandler(object sender, MainEventArgs e);

        /// <summary>
        /// The Event
        /// </summary>
        public event MainEventHandler DefaultEvent;

        public MainPage()
        {
        }

        /// <summary>
        /// The event trigger method
        /// </summary>
        /// <param name="e">The arguments</param>
        protected virtual void OnEvent(MainEventArgs e)
        {
            if (DefaultEvent != null) DefaultEvent(this, e);
        }
    }
}