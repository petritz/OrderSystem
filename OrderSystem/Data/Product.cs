using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data
{
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

        public int Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }

        public string NameWithPrice
        {
            get { return string.Format("{0} (€{1,00})", Name, Price); }
        }

        public decimal Price
        {
            get { return priceSell; }
        }

        public DateTime Created
        {
            get { return created; }
        }

        public DateTime Modified
        {
            get { return modified; }
        }
    }
}
