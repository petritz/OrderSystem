using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Annotations;

namespace OrderSystem.Data
{
    public class ProductLine : INotifyPropertyChanged
    {
        private int quantity;
        private Product product;

        public ProductLine(int quantity, Product product)
        {
            this.quantity = quantity;
            this.product = product;
        }

        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("PriceWithCurrency");
            }
        }

        public string ProductName
        {
            get { return product.Name; }
        }

        public Product Product
        {
            get { return product; }
        }

        public decimal PricePerItem
        {
            get { return product.Price; }
        }

        public decimal Price
        {
            get { return quantity*PricePerItem; }
        }

        public string PricePerItemWithCurrency
        {
            get { return string.Format("€ {0,00}", PricePerItem); }
        }

        public string PriceWithCurrency
        {
            get { return string.Format("€ {0,00}", Price); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}