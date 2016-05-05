using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data
{
    /// <summary>
    /// Class that represents the product table in the database
    /// </summary>
    public class Product
    {
        private int id;
        private string name;
        private decimal priceBuy;
        private decimal priceSell;
        private DateTime created;
        private DateTime modified;

        public Product(int id, string name, decimal priceBuy, decimal priceSell, DateTime created, DateTime modified)
        {
            this.id = id;
            this.name = name;
            this.priceBuy = priceBuy;
            this.priceSell = priceSell;
            this.created = created;
            this.modified = modified;
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

            return new Product(id, name, priceBuy, priceSell, created, modified);
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
    }
}