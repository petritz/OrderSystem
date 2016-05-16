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
using OrderSystem.Enums;

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
            DataTable table = Run(new SelectQueryBuilder(base.table)
                .SelectAll()
                .Where("status", QueryBuilder.ValueWrap("ok"))
                .Statement);

            foreach (DataRow row in table.Rows)
            {
                list.Add(Product.Parse(row));
            }

            return list;
        }

        /// <summary>
        /// Get all products for the admin view
        /// </summary>
        /// <returns></returns>
        public List<Product> GetReallyAll()
        {
            List<Product> list = new List<Product>();
            DataTable table = Run(new SelectQueryBuilder(base.table)
                .SelectAll()
                .Where("status", QueryBuilder.ValueWrap("deleted"), CompareType.NotEqual)
                .Statement);

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

        /// <summary>
        /// Get a specific product at a specific time
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <param name="time">The time for the product price</param>
        /// <returns>The product</returns>
        public Product GetReal(int id, DateTime time)
        {
            SelectQueryBuilder sb = new SelectQueryBuilder("product p", false);
            sb.Select("p.id AS id", "p.name AS name", "p.status AS status", "p.created AS created",
                "p.modified AS modified");

            SelectQueryBuilder s1 = new SelectQueryBuilder("product_price_log lg", false);
            s1.Select("lg.price");
            s1.Where("lg.field", QueryBuilder.ValueWrap("price_buy"));
            s1.Where("lg.product_id", "p.id");
            s1.Where(QueryBuilder.ValueWrap(QueryHelper.ToDatabaseDate(time)), "lg.valid_from",
                CompareType.GreaterThanOrEqual);
            s1.Where(QueryBuilder.ValueWrap(QueryHelper.ToDatabaseDate(time)), "COALESCE(lg.valid_to, NOW())",
                CompareType.LessThanOrEqual);

            SelectQueryBuilder s2 = new SelectQueryBuilder("product_price_log lg", false);
            s2.Select("lg.price");
            s2.Where("lg.field", QueryBuilder.ValueWrap("price_sell"));
            s2.Where("lg.product_id", "p.id");
            s2.Where(QueryBuilder.ValueWrap(QueryHelper.ToDatabaseDate(time)), "lg.valid_from",
                CompareType.GreaterThanOrEqual);
            s2.Where(QueryBuilder.ValueWrap(QueryHelper.ToDatabaseDate(time)), "COALESCE(lg.valid_to, NOW())",
                CompareType.LessThanOrEqual);

            sb.Select(s1, "COALESCE(", ", 0) AS price_buy");
            sb.Select(s2, "COALESCE(", ", 0) price_sell");
            sb.Where("id", id);

            DataTable dt = Run(sb.Statement);
            if (dt.Rows.Count == 1)
            {
                return Product.Parse(dt.Rows[0]);
            }
            return null;
        }

        /// <summary>
        /// Updates the name of the product
        /// </summary>
        /// <param name="id">The product id</param>
        /// <param name="name">The new name</param>
        /// <returns>If it was successful or not</returns>
        public bool UpdateName(int id, string name)
        {
            UpdateQueryBuilder ub = new UpdateQueryBuilder(base.table);
            ub.Update("name", QueryBuilder.ValueWrap(name));
            ub.Where("id", id);

            return Update(ub.Statement);
        }

        /// <summary>
        /// Updates the sell price of the product
        /// </summary>
        /// <param name="id">The product id</param>
        /// <param name="value">The new price</param>
        /// <returns>If it was successful or not</returns>
        public bool UpdatePriceSell(int id, decimal value)
        {
            UpdateQueryBuilder ub = new UpdateQueryBuilder(base.table);
            ub.Update("price_sell", value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
            ub.Where("id", id);

            return Update(ub.Statement);
        }

        /// <summary>
        /// Updates the bought price of the price
        /// </summary>
        /// <param name="id">The product id</param>
        /// <param name="value">The new price</param>
        /// <returns>If it was successful or not</returns>
        public bool UpdatePriceBuy(int id, decimal value)
        {
            UpdateQueryBuilder ub = new UpdateQueryBuilder(base.table);
            ub.Update("price_buy", value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
            ub.Where("id", id);

            return Update(ub.Statement);
        }

        /// <summary>
        /// Sets the deleted flag of the product
        /// </summary>
        /// <param name="id">The product id</param>
        /// <returns>If it was successful or not</returns>
        public bool RemoveProduct(int id)
        {
            UpdateQueryBuilder ub = new UpdateQueryBuilder(base.table);
            ub.Update("status", QueryBuilder.ValueWrap("deleted"));
            ub.Where("id", id);

            return Update(ub.Statement);
        }

        /// <summary>
        /// Sets the unavailable flag of the product
        /// </summary>
        /// <param name="id">The product id</param>
        /// <returns>If it was successful or not</returns>
        public bool SetUnavailable(int id)
        {
            UpdateQueryBuilder ub = new UpdateQueryBuilder(base.table);
            ub.Update("status", QueryBuilder.ValueWrap("unavailable"));
            ub.Where("id", id);

            return Update(ub.Statement);
        }

        /// <summary>
        /// Sets the ok flag of the product
        /// </summary>
        /// <param name="id">The product id</param>
        /// <returns>If it was successful or not</returns>
        public bool SetOk(int id)
        {
            UpdateQueryBuilder ub = new UpdateQueryBuilder(base.table);
            ub.Update("status", QueryBuilder.ValueWrap("ok"));
            ub.Where("id", id);

            return Update(ub.Statement);
        }

        /// <summary>
        /// Adds the product to the database
        /// </summary>
        /// <param name="name">The name of the product</param>
        /// <param name="priceBuy">The price the product is bought</param>
        /// <param name="priceSell">The price the product will be selled</param>
        public bool Insert(string name, decimal priceBuy, decimal priceSell)
        {
            InsertQueryBuilder ib = new InsertQueryBuilder(base.table);
            ib.Insert("id", "NULL");
            ib.Insert("name", QueryBuilder.ValueWrap(name));
            ib.Insert("price_buy", priceBuy.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
            ib.Insert("price_sell", priceSell.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));

            return Update(ib.Statement);
        }
    }
}