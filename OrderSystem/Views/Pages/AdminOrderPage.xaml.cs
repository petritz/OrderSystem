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
    /// The admin order page
    /// </summary>
    public partial class AdminOrderPage : AppPage
    {
        private ObservableCollection<Order> orderTable;
        private OrderModel orderModel;

        public AdminOrderPage()
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
            orderTable = new ObservableCollection<Order>();
            dgOrders.DataContext = this;

            orderModel = (OrderModel)ModelRegistry.Get(ModelIdentifier.Order);
        }

        private void LoadOrders()
        {
            orderTable.Clear();

            foreach (Order order in orderModel.GetAllOrders())
            {
                orderTable.Add(order);
            }
        }

        public ObservableCollection<Order> OrderTable
        {
            get { return orderTable; }
        }

        private void OnCreateOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dpTime.Value == null)
                {
                    throw new Exception("Es wurde keine korrekte Zeit eingegeben.");
                }

                DateTime time = (DateTime)dpTime.Value;
                if (!orderModel.CreateOrder(time, Session.Instance.CurrentUserId))
                {
                    throw new Exception("Bestellung konnte nicht erstellt werden.");
                }

                ReloadResources();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
