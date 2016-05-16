using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Data;
using OrderSystem.Enums;

namespace OrderSystem.Models
{
    /// <summary>
    /// This class stores all menu items. The items are initialized when the instance was accessed.
    /// </summary>
    public class MenuRegistry
    {
        private static MenuRegistry instance;
        private List<AbstractMenuItem> items;

        private MenuRegistry()
        {
            if (Session.IsValidSession)
            {
                items = new List<AbstractMenuItem>
                {
                    new MenuItemGroup("Allgemein"),
                    new MenuItemPage("Bestellung", "/Resources/Order.png", PageIdentifier.OrderPage),
                    new MenuItemPage("Deine Bestellungen", "/Resources/Statistic.png", PageIdentifier.StatisticPage),
                    new MenuItemPage("Profil", "/Resources/Profile.png", PageIdentifier.ProfilePage),
                };

                UserModel model = (UserModel)ModelRegistry.Get(ModelIdentifier.User);
                if (model.GetUser(Session.Instance.CurrentUserId).Admin)
                {
                    items.Add(new MenuItemGroup("Administration"));
                    items.Add(new MenuItemPage("Bestellungen", "/Resources/AdminOrder.png", PageIdentifier.AdminOrderPage));
                    items.Add(new MenuItemPage("Produkte", "/Resources/AdminProducts.png", PageIdentifier.AdminProducts));
                }

                items.Add(new MenuItemSplitter());
                items.Add(new MenuItemAction("Logout", "/Resources/Logout.png", ActionIdentifier.Logout));
            }
        }

        /// <summary>
        /// The menu items
        /// </summary>
        public List<AbstractMenuItem> Items
        {
            get { return items; }
        }

        /// <summary>
        /// The instance to the menu registry
        /// </summary>
        public static MenuRegistry Instance
        {
            get
            {
                if (instance == null) instance = new MenuRegistry();
                return instance;
            }
        }

        /// <summary>
        /// Reloads menu registry
        /// </summary>
        public static void Reload()
        {
            instance = new MenuRegistry();
        }
    }
}