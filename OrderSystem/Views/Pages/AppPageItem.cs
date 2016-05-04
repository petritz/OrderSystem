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

        // Properties

        public AppPage App
        {
            get
            {
                return page;
            }

            private set
            {
                page = value;
            }
        }

        public PageIdentifier Identifier
        {
            get
            {
                return identifier;
            }

            private set
            {
                identifier = value;
            }
        }

        public MenuItemPage MenuItem
        {
            get
            {
                return menuItem;
            }
            
            private set
            {
                menuItem = value;
            }
        }
    }
}
