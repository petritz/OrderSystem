using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Data;
using OrderSystem.Database;
using OrderSystem.Enums;

namespace OrderSystem.Models
{
    /// <summary>
    /// The model for the product_line tables
    /// </summary>
    public class ProductLineModel : MainModel
    {
        public ProductLineModel() : base("product_line")
        {
        }

        /// <summary>
        /// Adds a set of product lines to the database.
        /// </summary>
        /// <param name="userId">The user ordering the products</param>
        /// <param name="orderId">The order where the products are in</param>
        /// <param name="elements">The products</param>
        /// <returns></returns>
        public bool Submit(int userId, int orderId, List<ProductLine> elements)
        {
            foreach (ProductLine p in elements)
            {
                InsertQueryBuilder ib = new InsertQueryBuilder(base.table);
                ib.Insert("id", "NULL");
                ib.Insert("user", userId);
                ib.Insert("food_order", orderId);
                ib.Insert("product", p.Product.Id);
                ib.Insert("quantity", p.Quantity);
                ib.Insert("added", "NOW()");
                ib.Insert("paid", 0);

                if (!Update(ib.Statement))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines if the user has already a open order
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <param name="orderId">The order to check</param>
        /// <returns></returns>
        public bool HasAlreadyOrdered(int userId, int orderId)
        {
            SelectQueryBuilder sb = new SelectQueryBuilder(base.table);
            sb.SelectAll()
                .Where("user", userId)
                .Where("food_order", orderId);

            DataTable t = Run(sb.Statement);
            return t.Rows.Count >= 1;
        }

        /// <summary>
        /// Get the orders the user made.
        /// </summary>
        /// <param name="userId">The user</param>
        /// <returns>A list of orders the user made.</returns>
        public List<OrderOverviewRow> GetOrdersFromUser(int userId)
        {
            List<OrderOverviewRow> list = new List<OrderOverviewRow>();

            SelectQueryBuilder sb = new SelectQueryBuilder("food_orders");
            sb.SelectAll()
                .Where("user", userId)
                .OrderBy("time", OrderType.Descending)
                .Limit(10);

            DataTable d = Run(sb.Statement);
            foreach (DataRow row in d.Rows)
            {
                list.Add(OrderOverviewRow.Parse(row));
            }
            return list;
        }

        /// <summary>
        /// Get the statistic of orders from a specific user
        /// </summary>
        /// <param name="userId">The user</param>
        /// <returns>The statistic</returns>
        public OrderStatistic GetStatistic(int userId)
        {
            OrderStatistic statistic = new OrderStatistic();

            SelectQueryBuilder sb = new SelectQueryBuilder("food_orders");
            sb.Select("COALESCE(SUM(amount), 0)")
                .Select("COALESCE(SUM(sum), 0)")
                .Where("user", userId);

            DataTable d = Run(sb.Statement);
            statistic.BoughtProducts = 0;
            statistic.TotalPrice = 0;

            if (d.Rows.Count > 0)
            {
                DataRow row = d.Rows[0];
                int products = (int)row.Field<decimal>(0);
                decimal sum = row.Field<decimal>(1);
                statistic.BoughtProducts = products;
                statistic.TotalPrice = sum;
            }

            return statistic;
        }
    }
}