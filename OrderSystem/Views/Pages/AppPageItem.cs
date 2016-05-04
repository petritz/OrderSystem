using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using OrderSystem.Enums;

namespace OrderSystem.Views.Pages
{
    public class AppPageItem
    {
        private AppPage page;
        private PageIdentifiers identifier;
        private Button menuItem;

        public AppPageItem(PageIdentifiers id, AppPage page, Button menuItem)
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

        public PageIdentifiers Identifier
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

        public Button MenuItem
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
