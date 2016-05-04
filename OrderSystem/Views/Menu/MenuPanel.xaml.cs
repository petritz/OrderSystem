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
using OrderSystem.Data;
using OrderSystem.Enums;
using OrderSystem.Models;

namespace OrderSystem.Views.Menu
{
    /// <summary>
    /// Interaktionslogik für MenuPanel.xaml
    /// </summary>
    public partial class MenuPanel : StackPanel
    {
        private MenuRegistry menu;

        public MenuPanel()
        {
            InitializeComponent();
            InitMembers();
            InitItems();
        }

        private void InitMembers()
        {
            //This adds all the menu items
            menu = new MenuRegistry();
        }

        private void InitItems()
        {
            foreach (AbstractMenuItem item in menu.Items)
            {
                if (item.Type == MenuItemType.Group)
                {
                    AddGroup((MenuItemGroup)item);
                }
                else if (item.Type == MenuItemType.Page || item.Type == MenuItemType.Action)
                {
                    AddButton((MenuItemButton)item);
                }
                else if (item.Type == MenuItemType.Splitter)
                {
                    AddSplitter((MenuItemSplitter)item);
                }
            }
        }

        private void AddGroup(MenuItemGroup group)
        {
            Label info = new Label();
            info.Content = group.Name;
            info.Margin = new Thickness(8, 5, 0, 0);

            group.Label = info;
            rootPanel.Children.Add(info);
        }

        private void AddButton(MenuItemButton page)
        {
            Button button = new Button();
            button.Height = 40;
            button.Margin = new Thickness(10);

            DockPanel panel = new DockPanel();

            Image image = new Image();
            image.Source = new BitmapImage(new Uri(page.ImageResource, UriKind.Relative));
            image.Margin = new Thickness(0, 0, 8, 0);
            image.Height = 20;
            DockPanel.SetDock(image, Dock.Left);

            TextBlock text = new TextBlock();
            text.VerticalAlignment = VerticalAlignment.Center;
            text.Text = page.Name;
            DockPanel.SetDock(text, Dock.Right);

            panel.Children.Add(image);
            panel.Children.Add(text);

            button.Content = panel;
            page.Button = button;
            rootPanel.Children.Add(button);
        }

        private void AddSplitter(MenuItemSplitter splitter)
        {
            Separator seperator = new Separator();
            seperator.Height = 10;
            seperator.Margin = new Thickness(0);

            splitter.Seperator = seperator;
            rootPanel.Children.Add(seperator);
        }

        private void OnItemCicked(AbstractMenuItem sender, RoutedEventArgs e)
        {

        }
    }
}
