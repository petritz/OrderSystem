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
using OrderSystemLibrary.Data;
using OrderSystemLibrary.Enums;
using OrderSystemLibrary.Models;

namespace OrderSystem.Views.Pages
{
    /// <summary>
    /// The admin products page
    /// </summary>
    public partial class AdminProductsPage : AppPage
    {
        private ObservableCollection<Product> productTable;
        private ProductModel productModel;

        public AdminProductsPage()
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
            LoadProducts();
            LoadedResources = true;
        }

        public override void ReloadResources()
        {
            LoadProducts();
        }

        private void LoadMembers()
        {
            productTable = new ObservableCollection<Product>();
            dgProducts.DataContext = this;

            productModel = (ProductModel)ModelRegistry.Get(ModelIdentifier.Product);
        }

        private void LoadProducts()
        {
            productTable.Clear();

            foreach (Product product in productModel.GetReallyAll())
            {
                productTable.Add(product);
            }
        }

        public ObservableCollection<Product> ProductsTable
        {
            get { return productTable; }
        }

        private void OnCreateProduct(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = tbName.Text;
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("Der Name muss eingegeben werden.");
                }

                decimal priceBuy = duPriceBuy.Value ?? 0;
                decimal priceSell = duPriceSell.Value ?? 0;

                decimal profit = priceSell - priceBuy;

                if (profit <= 0)
                {
                    MessageBoxResult result = MessageBox.Show("Du machst keinen Gewinn. Willst du fortsetzen?", "Kein Gewinn",
                        MessageBoxButton.YesNoCancel);

                    if (result != MessageBoxResult.Yes)
                    {
                        throw new Exception("");
                    }
                }

                if (!productModel.Insert(name, priceBuy, priceSell))
                {
                    throw new Exception("Produkt konnte nicht hinzugefügt werden.");
                }

                ReloadResources();
                ClearProduct();
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ClearProduct()
        {
            tbName.Clear();
            duPriceBuy.Value = 0;
            duPriceSell.Value = 0;
            duProfit.Value = 0;
        }

        private void OnDeleteProduct(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgProducts.SelectedIndex == -1)
                {
                    throw new Exception("Es muss ein Produkt ausgewählt werden.");
                }

                Product p = productTable.ElementAt(dgProducts.SelectedIndex);
                MessageBoxResult result = MessageBox.Show($"Möchtest du '{p.Name}' wirklich löschen?", "Produkt löschen",
                    MessageBoxButton.YesNoCancel);

                if (result != MessageBoxResult.Yes)
                {
                    throw new Exception("");
                }

                if (productModel.RemoveProduct(p.Id))
                {
                    productTable.RemoveAt(dgProducts.SelectedIndex);
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void OnMakeProductUnavailable(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgProducts.SelectedIndex == -1)
                {
                    throw new Exception("Es muss ein Produkt ausgewählt werden.");
                }

                Product p = productTable.ElementAt(dgProducts.SelectedIndex);

                if (productModel.SetUnavailable(p.Id))
                {
                    p.Status = ProductStatus.Unavailable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnSetProductStatusOk(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgProducts.SelectedIndex == -1)
                {
                    throw new Exception("Es muss ein Produkt ausgewählt werden.");
                }

                Product p = productTable.ElementAt(dgProducts.SelectedIndex);

                if (productModel.SetOk(p.Id))
                {
                    p.Status = ProductStatus.Ok;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnPriceChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UpdateProfitPrice();
        }

        private void UpdateProfitPrice()
        {
            decimal priceBuy = duPriceBuy.Value ?? 0;
            decimal priceSell = duPriceSell.Value ?? 0;

            if (priceBuy != 0 && priceSell != 0)
            {
                decimal profit = priceSell - priceBuy;
                duProfit.Value = profit;
            }
        }
    }
}
