using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Enums;

namespace OrderSystem.Events
{
    /// <summary>
    /// The event arguments used by the action clicked event.
    /// </summary>
    public class ActionClickedEventArgs : MainEventArgs
    {
        private ActionIdentifier actionIdentifier;

        public ActionClickedEventArgs(ActionIdentifier actionIdentifier) : base("menu_action_clicked")
        {
            this.actionIdentifier = actionIdentifier;
        }

        /// <summary>
        /// The action identifier that triggered this event
        /// </summary>
        public ActionIdentifier ActionIdentifier
        {
            get { return actionIdentifier; }
        }
    }
}