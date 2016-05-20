using OrderSystem.Data;
using OrderSystem.Events;
using OrderSystem.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OrderSystem.Enums;
using OrderSystem.Models;
using OrderSystemLibrary.Data;

namespace OrderSystem.Views
{
    /// <summary>
    /// The root app that loads the app pages
    /// </summary>
    public partial class RootApp : MainPage
    {
        public Page currentPage;
        public Dictionary<PageIdentifier, AppPageItem> pages;

        public RootApp()
        {
            InitializeComponent();
            InitPages();
            NavigateToPage(PageIdentifier.OrderPage);
        }

        private void InitPages()
        {
            pages = new Dictionary<PageIdentifier, AppPageItem>();

            foreach (AbstractMenuItem item in MenuRegistry.Instance.Items)
            {
                if (item is MenuItemPage)
                {
                    MenuItemPage page = (MenuItemPage)item;
                    AddPage(page);
                }
            }
        }

        private void AddPage(MenuItemPage page)
        {
            switch (page.PageIdentifier)
            {
                case PageIdentifier.OrderPage:
                    pages.Add(page.PageIdentifier, new AppPageItem(page.PageIdentifier, new OrderPage(), page));
                    break;
                case PageIdentifier.StatisticPage:
                    pages.Add(page.PageIdentifier, new AppPageItem(page.PageIdentifier, new StatisticPage(), page));
                    break;
                case PageIdentifier.CreditPage:
                    pages.Add(page.PageIdentifier, new AppPageItem(page.PageIdentifier, new CreditPage(), page));
                    break;
                case PageIdentifier.ProfilePage:
                    pages.Add(page.PageIdentifier, new AppPageItem(page.PageIdentifier, new ProfilePage(), page));
                    break;
                case PageIdentifier.AdminOrderPage:
                    pages.Add(page.PageIdentifier, new AppPageItem(page.PageIdentifier, new AdminOrderPage(), page));
                    break;
                case PageIdentifier.AdminProducts:
                    pages.Add(page.PageIdentifier, new AppPageItem(page.PageIdentifier, new AdminProductsPage(), page));
                    break;
                case PageIdentifier.AdminOpenOrdersPage:
                    pages.Add(page.PageIdentifier, new AppPageItem(page.PageIdentifier, new AdminOpenOrdersPage(), page));
                    break;
                case PageIdentifier.AdminOpenCreditPage:
                    pages.Add(page.PageIdentifier, new AppPageItem(page.PageIdentifier, new AdminCreditPage(), page));
                    break;
            }
        }

        private void NavigateToPage(PageIdentifier identifier)
        {
            if (pages.ContainsKey(identifier))
            {
                AppPageItem page = pages[identifier];

                if (!page.App.LoadedView)
                {
                    Logger.I("Loading view for " + page.Identifier + "...");
                    page.App.LoadView();
                    Logger.I("Loaded view for " + page.Identifier + ".");
                }
                if (!page.App.LoadedResources)
                {
                    Logger.I("Loading resources for " + page.Identifier + "...");
                    page.App.LoadResources();
                    Logger.I("Loaded resources for " + page.Identifier + ".");
                }
                else
                {
                    Logger.I("Reloading resources for " + page.Identifier + "...");
                    page.App.ReloadResources();
                    Logger.I("Reloaded resources for " + page.Identifier + ".");
                }

                frame.Navigate(page.App);

                //Enable all
                EnableAllItems();

                page.MenuItem.Button.IsEnabled = false;
            }
        }

        private void EnableAllItems()
        {
            foreach (KeyValuePair<PageIdentifier, AppPageItem> entry in pages)
            {
                entry.Value.MenuItem.Button.IsEnabled = true;
            }
        }

        private void OnPageClicked(object sender, PageClickedEventArgs e)
        {
            NavigateToPage(e.PageIdentifier);
        }

        private void OnActionClicked(object sender, ActionClickedEventArgs e)
        {
            switch (e.ActionIdentifier)
            {
                case ActionIdentifier.Logout:
                    Storage.Instance.Remove("email");
                    Storage.Instance.Remove("password");
                    Storage.Instance.Save();
                    Session.DeleteSession();
                    base.OnEvent(new LogoutEventArgs());
                    break;
            }
        }
    }
}