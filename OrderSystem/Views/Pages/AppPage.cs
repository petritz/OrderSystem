using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrderSystem.Views.Pages
{
    public class AppPage
    {
        private Page page;
        private PageIdentifiers identifier;
        private Button menuItem;

        public AppPage(PageIdentifiers id, Page page, Button menuItem)
        {
            this.identifier = id;
            this.page = page;
            this.menuItem = menuItem;
        }

        // Properties

        public Page Page
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
