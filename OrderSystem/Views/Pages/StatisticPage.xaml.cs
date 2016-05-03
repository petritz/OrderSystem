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
using OrderSystem.Models;

namespace OrderSystem.Views.Pages
{
    /// <summary>
    /// Interaktionslogik für StatisticPage.xaml
    /// </summary>
    public partial class StatisticPage : AppPage
    {
        private ObservableCollection<OrderOverviewRow> orderTable;
        private ProductLineModel productLineModel; 

        public StatisticPage()
        {
        }

        public override void LoadView()
        {
            InitializeComponent();
        }

        public override void LoadResources()
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            orderTable = new ObservableCollection<OrderOverviewRow>();
            dgOrders.DataContext = this;

            productLineModel = (ProductLineModel) ModelRegistry.Get("productLine");
            foreach (OrderOverviewRow row in productLineModel.GetOrdersFromUser(Session.Instance.CurrentUserId))
            {
                orderTable.Add(row);
            }
        }

        public ObservableCollection<OrderOverviewRow> OrderTable
        {
            get { return orderTable; }
        }
    }
}
