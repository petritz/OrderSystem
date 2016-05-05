using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using OrderSystem.Enums;

namespace OrderSystem.Data
{
    /// <summary>
    /// Class that represents an action item in the menu. For example: Logout
    /// </summary>
    public class MenuItemAction : MenuItemButton
    {
        private ActionIdentifier actionIdentifier;

        public MenuItemAction(string name, string imageResource, ActionIdentifier actionIdentifier)
            : base(name, imageResource)
        {
            this.actionIdentifier = actionIdentifier;
        }

        public MenuItemAction(string name, string imageResource, ActionIdentifier actionIdentifier, Button button)
            : base(name, imageResource, button)
        {
            this.actionIdentifier = actionIdentifier;
        }

        /// <summary>
        /// Get the action identifier associated to the item
        /// </summary>
        public ActionIdentifier ActionIdentifier
        {
            get { return actionIdentifier; }
        }

        public override MenuItemType Type
        {
            get { return MenuItemType.Action; }
        }
    }
}