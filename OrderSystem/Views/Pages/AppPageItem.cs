using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using OrderSystem.Data;
using OrderSystem.Enums;

namespace OrderSystem.Views.Pages
{
    /// <summary>
    /// The item for the app page.
    /// </summary>
    public class AppPageItem
    {
        private AppPage page;
        private PageIdentifier identifier;
        private MenuItemPage menuItem;

        public AppPageItem(PageIdentifier id, AppPage page, MenuItemPage menuItem)
        {
            this.identifier = id;
            this.page = page;
            this.menuItem = menuItem;
        }

        /// <summary>
        /// The view element
        /// </summary>
        public AppPage App
        {
            get { return page; }

            private set { page = value; }
        }

        /// <summary>
        /// The page identifier
        /// </summary>
        public PageIdentifier Identifier
        {
            get { return identifier; }

            private set { identifier = value; }
        }

        /// <summary>
        /// The menu item
        /// </summary>
        public MenuItemPage MenuItem
        {
            get { return menuItem; }

            private set { menuItem = value; }
        }
    }
}