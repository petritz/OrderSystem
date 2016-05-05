using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using OrderSystem.Enums;

namespace OrderSystem.Data
{
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