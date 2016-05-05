using OrderSystem.Data;
using OrderSystem.Database;
using OrderSystem.Helper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrderSystem.Models
{
    public class ProductModel : MainModel
    {
        public ProductModel() : base("product")
        {
        }

        public List<Product> GetAll()
        {
            List<Product> list = new List<Product>();
            DataTable table = Run(new SelectQueryBuilder(base.table).SelectAll().Statement);

            foreach (DataRow row in table.Rows)
            {
                list.Add(Product.Parse(row));
            }

            return list;
        }
    }
}