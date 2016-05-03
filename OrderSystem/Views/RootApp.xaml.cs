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

namespace OrderSystem.Views
{
    /// <summary>
    /// Interaktionslogik für RootApp.xaml
    /// </summary>
    public partial class RootApp : MainPage
    {
        public Page currentPage;
        public Dictionary<PageIdentifiers, AppPageItem> pages;

        public RootApp()
        {
            InitializeComponent();
            InitPages();
            NavigateToPage(PageIdentifiers.OrderPage);
        }

        private void InitPages()
        {
            pages = new Dictionary<PageIdentifiers, AppPageItem>();

            AppPageItem orderPage = new AppPageItem(PageIdentifiers.OrderPage, new OrderPage(), btOrder);
            pages.Add(orderPage.Identifier, orderPage);

            AppPageItem statisticPage = new AppPageItem(PageIdentifiers.StatisticPage, new StatisticPage(), btStatistic);
            pages.Add(statisticPage.Identifier, statisticPage);

            AppPageItem profilePage = new AppPageItem(PageIdentifiers.ProfilePage, new ProfilePage(), btProfil);
            pages.Add(profilePage.Identifier, profilePage);
        }

        private void NavigateToPage(PageIdentifiers identifier)
        {
            AppPageItem page = pages[identifier];

            if (!page.App.LoadedView)
            {
                Console.WriteLine("Loading view for " + page.Identifier + "...");
                page.App.LoadView();
                Console.WriteLine("Loaded view for " + page.Identifier + ".");
            }
            if (!page.App.LoadedResources)
            {
                Console.WriteLine("Loading resources for " + page.Identifier + "...");
                page.App.LoadResources();
                Console.WriteLine("Loaded resources for " + page.Identifier + ".");
            }

            frame.Navigate(page.App);

            //Enable all
            EnableAllItems();

            page.MenuItem.IsEnabled = false;
        }

        private void EnableAllItems()
        {
            foreach (KeyValuePair<PageIdentifiers, AppPageItem> entry in pages)
            {
                entry.Value.MenuItem.IsEnabled = true;
            }
        }

        private void OnOrderClicked(object sender, RoutedEventArgs e)
        {
            NavigateToPage(PageIdentifiers.OrderPage);
        }

        private void OnStatisticClicked(object sender, RoutedEventArgs e)
        {
            NavigateToPage(PageIdentifiers.StatisticPage);
        }

        private void OnProfileClicked(object sender, RoutedEventArgs e)
        {
            NavigateToPage(PageIdentifiers.ProfilePage);
        }

        private void OnLogoutClicked(object sender, RoutedEventArgs e)
        {
            Storage.Instance.Remove("email");
            Storage.Instance.Remove("password");
            Storage.Instance.Save();
            Session.DeleteSession();
            base.OnEvent(new LogoutEventArgs());
        }
    }
}
