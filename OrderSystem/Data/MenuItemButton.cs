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
    /// Abstract class used to represent a button in the menu
    /// </summary>
    public abstract class MenuItemButton : AbstractMenuItem
    {
        private string name;
        private string imageResource;
        private Button button;

        public MenuItemButton(string name, string imageResource)
        {
            this.name = name;
            this.imageResource = imageResource;
        }

        public MenuItemButton(string name, string imageResource, Button button)
        {
            this.name = name;
            this.imageResource = imageResource;
            this.button = button;
        }

        /// <summary>
        /// Name of the menu item
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Path to the icon
        /// </summary>
        public string ImageResource
        {
            get { return imageResource; }
        }

        /// <summary>
        /// Reference to the created button
        /// </summary>
        public Button Button
        {
            get { return button; }
            set { button = value; }
        }
    }
}