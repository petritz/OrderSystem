using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
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
                .Where("food_order", orderId)
                .Where("status", QueryBuilder.ValueWrap("ok"));

            DataTable t = Run(sb.Statement);
            return t.Rows.Count >= 1;
        }

        /// <summary>
        /// Get the open orders the user.
        /// </summary>
        /// <param name="userId">The user</param>
        /// <returns>A list of open orders the user made.</returns>
        public List<OrderOverviewRow> GetOpenOrdersFromUser(int userId)
        {
            List<OrderOverviewRow> list = new List<OrderOverviewRow>();

            SelectQueryBuilder sb = new SelectQueryBuilder("food_orders f", false);
            sb.SelectAll()
                .Join(JoinType.Inner, "food_order o", "f.order = o.id")
                .Where("f.user", userId)
                .Where("o.closed", "0")
                .Where("o.time", "NOW()", CompareType.GreaterThanOrEqual)
                .OrderBy("f.time", OrderType.Descending);

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
                ulong products = (ulong)row.Field<decimal>(0);
                decimal sum = row.Field<decimal>(1);
                statistic.BoughtProducts = products;
                statistic.TotalPrice = sum;
            }

            return statistic;
        }

        /// <summary>
        /// Cancels the order of a user
        /// </summary>
        /// <param name="id">The food order id</param>
        public bool CancelOrder(int id)
        {
            UpdateQueryBuilder ub = new UpdateQueryBuilder(base.table);
            ub.Update("status", QueryBuilder.ValueWrap("cancelled"));
            ub.Where("food_order", id);
            int ret = UpdateRows(ub.Statement);
            return ret > 0;
        }

        /// <summary>
        /// Get product lines from a order
        /// </summary>
        /// <param name="order">The order id</param>
        /// <returns>list of product lines</returns>
        public List<ProductLine> GetOrder(int order, int user)
        {
            List<ProductLine> list = new List<ProductLine>();

            SelectQueryBuilder sb = new SelectQueryBuilder(base.table);
            sb.SelectAll()
                .Where("food_order", order)
                .Where("user", user)
                .Where("status", QueryBuilder.ValueWrap("ok"));

            DataTable dt = Run(sb.Statement);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ProductLine.Parse(row));
            }

            return list;
        }

        /// <summary>
        /// Returns a overview of open orders
        /// </summary>
        /// <returns>list of orders</returns>
        public List<AdminOpenOrderRow> GetAdminOpenOrders()
        {
            List<AdminOpenOrderRow> list = new List<AdminOpenOrderRow>();

            SelectQueryBuilder sb = new SelectQueryBuilder("food_orders o", false);
            sb.Select("o.order", "o.time");
            sb.Select(
                new SelectQueryBuilder("food_orders f", false)
                    .Select("COALESCE(SUM(f.sum), 0)")
                    .Where("f.paid", "0")
                    .Where("f.order", "o.order"), "", "AS sumToPay");
            sb.Select(
                new SelectQueryBuilder("food_orders f", false)
                    .Select("COALESCE(SUM(f.sum), 0)")
                    .Where("f.paid", "1")
                    .Where("f.order", "o.order"), "", "AS sumPaid");
            sb.Select(
                new SelectQueryBuilder("food_orders f", false)
                    .Select("COALESCE(COUNT(f.user), 0)")
                    .Where("f.paid", "0")
                    .Where("f.order", "o.order"), "", "AS users");
            sb.Join(JoinType.Inner, "food_order f", "o.order = f.id");
            sb.Where("f.closed", "0");
            sb.Where("f.time", "NOW()", CompareType.GreaterThanOrEqual);
            sb.GroupBy("o.order");
            sb.OrderBy("o.time", OrderType.Descending);

            DataTable dt = Run(sb.Statement);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(AdminOpenOrderRow.Parse(row));
            }

            return list;
        }

        /// <summary>
        /// Get the users that ordered a order and the pay status
        /// </summary>
        /// <param name="id">The order id</param>
        /// <returns>List of users</returns>
        public List<AdminOrderUserRow> GetUsersFromOrder(int id)
        {
            List<AdminOrderUserRow> list = new List<AdminOrderUserRow>();

            SelectQueryBuilder sb = new SelectQueryBuilder("food_orders");
            sb.SelectColumn("user")
                .SelectColumn("sum")
                .SelectColumn("paid")
                .SelectColumn("pay_type")
                .Where(QueryBuilder.NameWrap("order"), id)
                .GroupBy(QueryBuilder.NameWrap("user"));

            DataTable dt = Run(sb.Statement);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(AdminOrderUserRow.Parse(row, id));
            }

            return list;
        }

        /// <summary>
        /// Updates the paid flag of a food order
        /// </summary>
        /// <param name="order">The order id</param>
        /// <param name="userId">The user id</param>
        /// <param name="value">The value to set</param>
        /// <returns>if it was successful or not</returns>
        public bool SetPaidOrder(int order, int userId, bool value)
        {
            UpdateQueryBuilder ub = new UpdateQueryBuilder(base.table);
            ub.Update("paid", value ? 1 : 0);
            ub.Update("pay_type", QueryBuilder.ValueWrap("admin"));
            ub.Where("food_order", order);
            ub.Where("user", userId);

            return UpdateRows(ub.Statement) != 0;
        }

        /// <summary>
        /// Retrieves pay type flag of the order of a user
        /// </summary>
        /// <param name="order">The order id</param>
        /// <param name="userId">The user id</param>
        /// <returns>The flag, if the value could not be retrieved: .Admin</returns>
        public PayType GetPayType(int order, int userId)
        {
            SelectQueryBuilder sb = new SelectQueryBuilder(base.table);
            sb.SelectColumn("pay_type")
                .Where("food_order", order)
                .Where("user", userId)
                .GroupBy("food_order");

            DataTable dt = Run(sb.Statement);
            if (dt.Rows.Count == 1)
            {
                DataRow row = dt.Rows[0];
                string payType = row.Field<string>("pay_type");
                return AdminOrderUserRow.StringToPayType(payType);
            }

            return PayType.Admin;
        }
    }
}
