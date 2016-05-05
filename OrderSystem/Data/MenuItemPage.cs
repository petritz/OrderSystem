using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using OrderSystem.Enums;

namespace OrderSystem.Data
{
    /// <summary>
    /// Class that represents a page change action. For example: Profile
    /// </summary>
    public class MenuItemPage : MenuItemButton
    {
        private PageIdentifier pageIdentifier;

        public MenuItemPage(string name, string imageResource, PageIdentifier pageIdentifier)
            : base(name, imageResource)
        {
            this.pageIdentifier = pageIdentifier;
        }

        public MenuItemPage(string name, string imageResource, PageIdentifier pageIdentifier, Button button)
            : base(name, imageResource, button)
        {
            this.pageIdentifier = pageIdentifier;
        }

        /// <summary>
        /// Identifier of the page to change associated to the item
        /// </summary>
        public PageIdentifier PageIdentifier
        {
            get { return pageIdentifier; }
        }

        public override MenuItemType Type
        {
            get { return MenuItemType.Page; }
        }
    }
}