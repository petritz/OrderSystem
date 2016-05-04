using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Enums;

namespace OrderSystem.Events
{
    public class ActionClickedEventArgs : MainEventArgs
    {
        private ActionIdentifier actionIdentifier;

        public ActionClickedEventArgs(ActionIdentifier actionIdentifier) : base("menu_action_clicked")
        {
            this.actionIdentifier = actionIdentifier;
        }

        public ActionIdentifier ActionIdentifier
        {
            get { return actionIdentifier; }
        }
    }
}
