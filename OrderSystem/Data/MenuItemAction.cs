using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using OrderSystem.Enums;

namespace OrderSystem.Data
{
    public class MenuItemAction : MenuItemPage
    {
        public MenuItemAction(string name, string imageResource, Button button) : base(name, imageResource, button)
        {
        }

        public override MenuItemType Type
        {
            get { return MenuItemType.Action; }
        }
    }
}
