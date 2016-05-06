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
    /// <summary>
    /// Class that represents the product_line table in the databse. Implements INotifyPropertyChanged -> notifys when quantity was changed and updates price
    /// </summary>
    public class ProductLine : INotifyPropertyChanged
    {
        private int quantity;
        private Product product;

        public ProductLine(int quantity, Product product)
        {
            this.quantity = quantity;
            this.product = product;
        }

        /// <summary>
        /// The quantity of the product line
        /// </summary>
        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
                OnPropertyChanged("PriceWithCurrency");
            }
        }

        /// <summary>
        /// The product name
        /// </summary>
        public string ProductName
        {
            get { return product.Name; }
        }

        /// <summary>
        /// The product
        /// </summary>
        public Product Product
        {
            get { return product; }
        }

        /// <summary>
        /// The price of the product
        /// </summary>
        public decimal PricePerItem
        {
            get { return product.Price; }
        }

        /// <summary>
        /// The price of the product times the quantity
        /// </summary>
        public decimal Price
        {
            get { return quantity * PricePerItem; }
        }

        /// <summary>
        /// The price per item with currency; Format: € 0,00
        /// </summary>
        public string PricePerItemWithCurrency
        {
            get { return string.Format("€ {0,00}", PricePerItem); }
        }

        /// <summary>
        /// THe price with currency; Format: € 0,00
        /// </summary>
        public string PriceWithCurrency
        {
            get { return string.Format("€ {0,00}", Price); }
        }

        /// <summary>
        /// Event for property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}