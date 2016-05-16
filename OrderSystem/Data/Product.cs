using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Annotations;
using OrderSystem.Enums;
using OrderSystem.Models;

namespace OrderSystem.Data
{
    /// <summary>
    /// Class that represents the product table in the database
    /// </summary>
    public class Product : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private decimal priceBuy;
        private decimal priceSell;
        private DateTime created;
        private DateTime modified;
        private ProductStatus status;

        public Product(int id, string name, decimal priceBuy, decimal priceSell, DateTime created, DateTime modified, ProductStatus status = ProductStatus.Ok)
        {
            this.id = id;
            this.name = name;
            this.priceBuy = priceBuy;
            this.priceSell = priceSell;
            this.created = created;
            this.modified = modified;
            this.status = status;
        }

        /// <summary>
        /// Parses a database row into a product object
        /// </summary>
        /// <param name="row">The row to parse</param>
        /// <returns>The parsed object</returns>
        public static Product Parse(DataRow row)
        {
            int id = row.Field<int>("id");
            string name = row.Field<string>("name");
            decimal priceBuy = row.Field<decimal>("price_buy");
            decimal priceSell = row.Field<decimal>("price_sell");
            DateTime created = row.Field<DateTime>("created");
            DateTime modified = row.Field<DateTime>("modified");
            ProductStatus status = StringToStatus(row.Field<string>("status"));

            return new Product(id, name, priceBuy, priceSell, created, modified, status);
        }

        /// <summary>
        /// Converts string to product status enum
        /// </summary>
        /// <param name="str">the string to convert</param>
        /// <returns>the product status, .Ok if nothing else found</returns>
        public static ProductStatus StringToStatus(string str)
        {
            switch (str)
            {
                case "ok":
                    return ProductStatus.Ok;
                case "deleted":
                    return ProductStatus.Deleted;
                case "unavailable":
                    return ProductStatus.Unavailable;
            }

            return ProductStatus.Ok;
        }

        /// <summary>
        /// Converts product status enum to string for database
        /// </summary>
        /// <param name="status">The status to convert</param>
        /// <returns>The string representation of the status, "" if nothing else found</returns>
        public static string StatusToString(ProductStatus status)
        {
            switch (status)
            {
                case ProductStatus.Ok:
                    return "ok";
                case ProductStatus.Deleted:
                    return "deleted";
                case ProductStatus.Unavailable:
                    return "unavailable";
            }

            return "";
        }

        /// <summary>
        /// The ID of the product
        /// </summary>
        public int Id
        {
            get { return id; }
        }

        /// <summary>
        /// The name of the product
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                ProductModel model = (ProductModel)ModelRegistry.Get(ModelIdentifier.Product);
                if (model.UpdateName(Id, value))
                {
                    name = value;
                    OnPropertyChanged("NameWithPrice");
                }
            }
        }

        /// <summary>
        /// The name of the product with price; Format: Name (€ 0,00)
        /// </summary>
        public string NameWithPrice
        {
            get { return string.Format("{0} (€ {1,00})", Name, Price); }
        }

        /// <summary>
        /// The price of the product
        /// </summary>
        public decimal Price
        {
            get { return priceSell; }
            set
            {
                ProductModel model = (ProductModel)ModelRegistry.Get(ModelIdentifier.Product);
                if (model.UpdatePriceSell(Id, value))
                {
                    priceSell = value;
                    OnPropertyChanged("NameWithPrice");
                }
            }
        }

        /// <summary>
        /// The price of the product is bought
        /// </summary>
        public decimal PriceBuy
        {
            get { return priceBuy; }
            set
            {
                ProductModel model = (ProductModel)ModelRegistry.Get(ModelIdentifier.Product);
                if (model.UpdatePriceBuy(Id, value))
                {
                    priceBuy = value;
                }
            }
        }

        /// <summary>
        /// The time the product was created
        /// </summary>
        public DateTime Created
        {
            get { return created; }
        }

        /// <summary>
        /// The time the product was modified
        /// </summary>
        public DateTime Modified
        {
            get { return modified; }
        }

        /// <summary>
        /// The status of the product
        /// </summary>
        public ProductStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("StatusName");
            }
        }

        /// <summary>
        /// Returns the status of the product in a human-readable way
        /// </summary>
        public string StatusName
        {
            get
            {
                switch (Status)
                {
                    case ProductStatus.Ok:
                        return "OK";
                    case ProductStatus.Deleted:
                        return "Gelöscht";
                    case ProductStatus.Unavailable:
                        return "Nicht verfügbar";
                }

                return "";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}