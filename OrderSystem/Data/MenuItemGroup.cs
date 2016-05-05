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
    /// Class that represents a group label in the menu
    /// </summary>
    public class MenuItemGroup : AbstractMenuItem
    {
        private string name;
        private Label label;

        public MenuItemGroup(string name)
        {
            this.name = name;
        }

        public MenuItemGroup(string name, Label label)
        {
            this.name = name;
            this.label = label;
        }

        /// <summary>
        /// Name of the group
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Reference to the created label
        /// </summary>
        public Label Label
        {
            get { return label; }
            set { label = value; }
        }

        public override MenuItemType Type
        {
            get { return MenuItemType.Group; }
        }
    }
}