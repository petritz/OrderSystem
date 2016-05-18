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
using OrderSystem.Views.Dialog;

namespace OrderSystem.Views.Pages
{
    /// <summary>
    /// The statistic page
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
            LoadedView = true;
        }

        public override void LoadResources()
        {
            LoadMembers();
            LoadOrders();
            LoadStatistics();
            LoadedResources = true;
        }

        public override void ReloadResources()
        {
            LoadOrders();
            LoadStatistics();
        }

        private void LoadMembers()
        {
            orderTable = new ObservableCollection<OrderOverviewRow>();
            dgOrders.DataContext = this;

            productLineModel = (ProductLineModel)ModelRegistry.Get(ModelIdentifier.ProductLine);
        }

        private void LoadOrders()
        {
            orderTable.Clear();

            foreach (OrderOverviewRow row in productLineModel.GetOpenOrdersFromUser(Session.Instance.CurrentUserId))
            {
                orderTable.Add(row);
            }
        }

        private void LoadStatistics()
        {
            OrderStatistic stat = productLineModel.GetStatistic(Session.Instance.CurrentUserId);
            lbTotalAmount.Content = string.Format("Produkte gekauft: {0}", stat.BoughtProducts);
            lbTotalSpent.Content = string.Format("Gesamt ausgegeben: € {0,00}", stat.TotalPrice);
        }

        public ObservableCollection<OrderOverviewRow> OrderTable
        {
            get { return orderTable; }
        }

        private void OnCancelOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                OrderOverviewRow row = ((FrameworkElement)sender).DataContext as OrderOverviewRow;

                MessageBoxResult result = MessageBox.Show(string.Format("Willst du die Bestellung von {0} wirklich stornieren?", row.TimeFormatted), "Stornierung der Bestellung", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (row.PayType == PayType.Admin && row.Paid)
                    {
                        throw new Exception("Die Bestellung ist bereits bezahlt.");
                    }

                    if (row.PayType == PayType.Credit && row.Paid)
                    {
                        if (productLineModel.SetPaidOrder(row.Id, Session.Instance.CurrentUserId, false, PayType.Credit))
                        {
                            CreditModel creditModel = (CreditModel) ModelRegistry.Get(ModelIdentifier.Credit);
                            if (creditModel.AddCredit(row.Sum, Session.Instance.CurrentUserId, true))
                            {
                                ReloadResources();
                                throw new Exception("Bestellung wurde storniert und das Guthaben wurde zurückerstattet.");
                            }

                            throw new Exception("Das Guthaben konnte nicht zurückerstatt werden. Bitte melden Sie sich bei einem Administrator.");
                        }

                        throw new Exception("Die Bestellung konnte nicht storniert werden.");
                    }

                    OrderModel orderModel = (OrderModel)ModelRegistry.Get(ModelIdentifier.Order);

                    if (!orderModel.CanOrderBeCancelled(row.Id))
                    {
                        throw new Exception("Die Bestellung konnte nicht storniert werden da diese Bestellung bereits abgelaufen ist.");
                    }

                    if (!productLineModel.CancelOrder(row.Id, Session.Instance.CurrentUserId))
                    {
                        throw new Exception("Die Bestellung konnte nicht storniert werden.");
                    }

                    ReloadResources();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnViewOrder(object sender, RoutedEventArgs e)
        {
            OrderOverviewRow row = ((FrameworkElement)sender).DataContext as OrderOverviewRow;
            OrderViewDialog dlg = new OrderViewDialog(row);
            dlg.Show();
        }

        private void OnPayWithCredit(object sender, RoutedEventArgs e)
        {
            try
            {
                OrderOverviewRow row = ((FrameworkElement)sender).DataContext as OrderOverviewRow;

                MessageBoxResult result = MessageBox.Show(string.Format("Willst du die Bestellung von {0} wirklich mit Guthaben bezahlen?", row.TimeFormatted), "Bezahlen mit Guthaben", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (row.Paid)
                    {
                        throw new Exception("Die Bestellung ist bereits bezahlt.");
                    }

                    CreditModel creditModel = (CreditModel)ModelRegistry.Get(ModelIdentifier.Credit);

                    if (creditModel.GetCurrentCredit(Session.Instance.CurrentUserId) < row.Sum)
                    {
                        throw new Exception("Nicht ausreichend Guthaben vorhanden.");
                    }

                    if (!productLineModel.SetPaidOrder(row.Id, Session.Instance.CurrentUserId, true, PayType.Credit))
                    {
                        throw new Exception("Bestellungen konnte nicht über Guthaben bezahlt werden.");
                    }

                    if (creditModel.AddCredit(-row.Sum, Session.Instance.CurrentUserId))
                    {
                        MessageBox.Show("Bestellung wurde mit Guthaben erfolgreich bezahlt.");
                    }

                    ReloadResources();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}