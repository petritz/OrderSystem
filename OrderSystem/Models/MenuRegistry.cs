using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Data;
using OrderSystem.Enums;

namespace OrderSystem.Models
{
    public class MenuRegistry
    {
        private List<AbstractMenuItem> items;

        public MenuRegistry()
        {
            items = new List<AbstractMenuItem>
            {
                new MenuItemGroup("Allgemein"),
                new MenuItemPage("Bestellung", "/Resources/Order.png", PageIdentifier.OrderPage),
                new MenuItemPage("Bestellungen", "/Resources/Statistic.png", PageIdentifier.StatisticPage),
                new MenuItemPage("Profil", "/Resources/Profile.png", PageIdentifier.ProfilePage),
                new MenuItemSplitter(),
                new MenuItemAction("Logout", "/Resources/Logout.png", ActionIdentifier.Logout)
            };
        }

        public List<AbstractMenuItem> Items
        {
            get { return items; }
        }
    }
}
