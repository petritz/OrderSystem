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
using System.Windows.Shapes;
using OrderSystem.Data;
using OrderSystem.Enums;
using OrderSystem.Models;
using OrderSystemLibrary.Data;
using OrderSystemLibrary.Enums;
using OrderSystemLibrary.Models;

namespace OrderSystem.Views.Dialog
{
    /// <summary>
    /// Dialog to show a order
    /// </summary>
    public partial class OrderViewDialog : Window
    {
        private readonly OrderOverviewRow row;
        private ObservableCollection<ProductLine> productTable;
        private ProductLineModel productLineModel;

        public OrderViewDialog(OrderOverviewRow row)
        {
            InitializeComponent();
            this.row = row;

            InitMembers();
            LoadProducts();
        }

        private void InitMembers()
        {
            lbTitle.Content = $"Bestellung: {row.TimeFormatted}";

            productTable = new ObservableCollection<ProductLine>();
            dgProducts.DataContext = this;
            productLineModel = (ProductLineModel) ModelRegistry.Get(ModelIdentifier.ProductLine);
        }

        private void LoadProducts()
        {
            productTable.Clear();

            foreach (ProductLine p in productLineModel.GetOrder(row.Id, Session.Instance.CurrentUserId))
            {
                productTable.Add(p);
            }
        }

        public ObservableCollection<ProductLine> ProductTable
        {
            get { return productTable; }
        }
    }
}
