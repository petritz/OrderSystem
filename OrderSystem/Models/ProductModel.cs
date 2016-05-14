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
    /// <summary>
    /// The model for the product table.
    /// </summary>
    public class ProductModel : MainModel
    {
        public ProductModel() : base("product")
        {
        }

        /// <summary>
        /// Get all products from the database.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get a specific product
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <returns>The product</returns>
        public Product Get(int id)
        {
            SelectQueryBuilder sb = new SelectQueryBuilder(base.table)
                .SelectAll()
                .Where("id", id);
            DataTable dt = Run(sb.Statement);
            if (dt.Rows.Count == 1)
            {
                return Product.Parse(dt.Rows[0]);
            }
            return null;
        }
    }
}