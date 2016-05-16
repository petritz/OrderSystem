using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace OrderSystem.Views.Pages
{
    /// <summary>
    /// The admin open orders page
    /// </summary>
    public partial class AdminOpenOrdersPage : AppPage
    {
        private ObservableCollection<AdminOpenOrderRow> orderTable;
        private ProductLineModel productLineModel; 

        public AdminOpenOrdersPage()
        {
        }

        public override void LoadView()
        {
            InitializeComponent();
            LoadedView = true;
        }

        public override void LoadResources()
        {
            LoadMembers();
            LoadOrders();
            LoadedResources = true;
        }

        public override void ReloadResources()
        {
            LoadOrders();
        }

        private void LoadMembers()
        {
            orderTable = new ObservableCollection<AdminOpenOrderRow>();
            dgOrders.DataContext = this;

            productLineModel = (ProductLineModel) ModelRegistry.Get(ModelIdentifier.ProductLine);
        }

        private void LoadOrders()
        {
            orderTable.Clear();

            foreach (AdminOpenOrderRow row in productLineModel.GetAdminOpenOrders())
            {
                orderTable.Add(row);
            }
        }

        public ObservableCollection<AdminOpenOrderRow> OrderTable
        {
            get { return orderTable; }
        }

        private void OnShowDetails(object sender, RoutedEventArgs e)
        {
            AdminOpenOrderRow row = ((FrameworkElement)sender).DataContext as AdminOpenOrderRow;
            //TODO
        }
    }
}
