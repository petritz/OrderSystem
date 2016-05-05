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
    /// Class that represents the seperator in the menu
    /// </summary>
    public class MenuItemSplitter : AbstractMenuItem
    {
        private Separator seperator;

        public MenuItemSplitter()
        {
        }

        public MenuItemSplitter(Separator seperator)
        {
            this.seperator = seperator;
        }

        /// <summary>
        /// Reference to the created seperator
        /// </summary>
        public Separator Seperator
        {
            get { return seperator; }
            set { seperator = value; }
        }

        public override MenuItemType Type
        {
            get { return MenuItemType.Splitter; }
        }
    }
}