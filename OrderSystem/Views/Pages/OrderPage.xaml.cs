using OrderSystem.Data;
using OrderSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Xml;

namespace OrderSystem.Views.Pages
{
    /// <summary>
    /// Interaktionslogik für OrderPage.xaml
    /// </summary>
    public partial class OrderPage : AppPage
    {
        private ObservableCollection<Product> productList;
        private ObservableCollection<ProductLine> productTable;
        private ObservableCollection<Order> orderList;

        private ProductModel productModel;
        private OrderModel orderModel;
        private ProductLineModel productLineModel;

        public OrderPage()
        {
        }

        public override void LoadView()
        {
            InitializeComponent();
            LoadedView = true;
        }

        public override void LoadResources()
        {
            InitMembers();
            LoadProducts();
            LoadOrder();
            LoadedResources = true;
        }

        public override void ReloadResources()
        {
            LoadProducts();
            LoadOrder();
        }

        private void InitMembers()
        {
            productList = new ObservableCollection<Product>();
            productTable = new ObservableCollection<ProductLine>();
            orderList = new ObservableCollection<Order>();

            cbProduct.DataContext = this;
            dgProducts.DataContext = this;
            cbTimes.DataContext = this;

            productModel = (ProductModel)ModelRegistry.Get("product");
            productLineModel = (ProductLineModel)ModelRegistry.Get("productLine");
            orderModel = (OrderModel)ModelRegistry.Get("order");
        }

        private void LoadProducts()
        {
            productList.Clear();

            foreach (Product p in productModel.GetAll())
            {
                productList.Add(p);
            }
        }

        private void LoadOrder()
        {
            orderList.Clear();

            foreach (Order o in orderModel.GetTimes())
            {
                orderList.Add(o);
            }
        }

        private void OnAddProductToTable(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbProduct.SelectedIndex == -1)
                {
                    throw new Exception("Es muss ein Produkt ausgewählt werden.");
                }

                Product product = productList.ElementAt(cbProduct.SelectedIndex);
                int quantity = tbProductAmount.Value ?? 0;

                if (quantity <= 0)
                {
                    throw new Exception("Es müssen mehr wie 0 Produkte bestellt werden.");
                }

                productTable.Add(new ProductLine(quantity, product));
                UpdateTotalPrice();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnRemoveProduct(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgProducts.SelectedIndex == -1)
                {
                    throw new Exception("Es muss ein Produkt ausgewählt werden.");
                }

                productTable.RemoveAt(dgProducts.SelectedIndex);
                UpdateTotalPrice();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateTotalPrice()
        {
            decimal price = 0;
            foreach (ProductLine line in productTable)
            {
                price += line.Price;
            }
            lbTotalPrice.Content = string.Format("Gesamt: € {0,00}", price);
        }

        private void OnCellValueChanged(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                //prevent endless loops
                dgProducts.RowEditEnding -= OnCellValueChanged;
                dgProducts.CommitEdit();
                UpdateTotalPrice();
                dgProducts.RowEditEnding += OnCellValueChanged;
            }
        }

        private void OnMakeOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbTimes.SelectedIndex == -1)
                {
                    throw new Exception("Bitte eine Uhrzeit auswählen.");
                }

                if (productTable.Count <= 0)
                {
                    throw new Exception("Es sind keine Produkte hinzugefügt worden.");
                }

                Order o = (Order)cbTimes.SelectedValue;

                if (productLineModel.HasAlreadyOrdered(Session.Instance.CurrentUserId, o.Id))
                {
                    throw new Exception("Du hast bereits eine Bestellung für diese Uhrzeit abgegeben.");
                }

                if (!productLineModel.Submit(Session.Instance.CurrentUserId, o.Id, productTable.ToList()))
                {
                    throw new Exception("Reservierung konnte nicht aufgegeben werden.");
                }

                MessageBox.Show(
                    string.Format(
                        "Bestellung wird von einem Administrator bearbeitet. Bitte bezahle vor {0} Uhr bei ihm.",
                     o.Time));
                ClearOrder();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearOrder()
        {
            cbTimes.SelectedIndex = -1;
            cbProduct.SelectedIndex = -1;
            tbProductAmount.Value = 1;
            productTable.Clear();
            UpdateTotalPrice();
        }

        private void OnTimesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (new Thread(() =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    try
                    {
                        Order o = (Order)cbTimes.SelectedValue;

                        if (productLineModel.HasAlreadyOrdered(Session.Instance.CurrentUserId, o.Id))
                        {
                            throw new Exception("Du hast bereits eine Bestellung für diese Uhrzeit abgegeben.");
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }));
            })).Start();
        }

        public ObservableCollection<Product> ProductList
        {
            get { return productList; }
        }

        public ObservableCollection<ProductLine> ProductTable
        {
            get { return productTable; }
        }

        public ObservableCollection<Order> OrderList
        {
            get { return orderList; }
        }
    }
}
