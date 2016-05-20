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
    /// Dialog to show admin order
    /// </summary>
    public partial class AdminOrderViewDialog : Window
    {
        private readonly AdminOpenOrderRow row;
        private ObservableCollection<AdminOrderUserRow> userTable;
        private ObservableCollection<ProductLine> productTable; 
        private ProductLineModel productLineModel;

        public AdminOrderViewDialog(AdminOpenOrderRow row)
        {
            InitializeComponent();
            this.row = row;

            InitMembers();
            InitUsers();
        }

        private void InitMembers()
        {
            lbTitle.Content = $"Bestellung: {row.TimeFormatted}";

            userTable = new ObservableCollection<AdminOrderUserRow>();
            productTable = new ObservableCollection<ProductLine>();
            dgUsers.DataContext = this;
            dgProducts.DataContext = this;
            productLineModel = (ProductLineModel) ModelRegistry.Get(ModelIdentifier.ProductLine);
        }

        private void InitUsers()
        {
            userTable.Clear();

            foreach (AdminOrderUserRow r in productLineModel.GetUsersFromOrder(row.Id))
            {
                userTable.Add(r);
            }
        }

        private void UpdateProducts(int userId)
        {
            productTable.Clear();

            foreach (ProductLine line in productLineModel.GetOrder(row.Id, userId))
            {
                productTable.Add(line);
            }
        }

        public ObservableCollection<AdminOrderUserRow> UserTable
        {
            get { return userTable; }
        }

        public ObservableCollection<ProductLine> ProductTable
        {
            get { return productTable; }
        }

        private void OnUserSelected(object sender, RoutedEventArgs e)
        {
            AdminOrderUserRow row = userTable[dgUsers.SelectedIndex];
            UpdateProducts(row.User.Id);
        }
        
    }
}
