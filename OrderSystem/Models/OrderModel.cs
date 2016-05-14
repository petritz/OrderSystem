using OrderSystem.Data;
using OrderSystem.Database;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Enums;

namespace OrderSystem.Models
{
    /// <summary>
    /// The model for the food_order table
    /// </summary>
    public class OrderModel : MainModel
    {
        public OrderModel() : base("food_order")
        {
        }

        /// <summary>
        /// Get the available orders
        /// </summary>
        /// <returns></returns>
        public List<Order> GetAvailableOrders()
        {
            List<Order> list = new List<Order>();

            SelectQueryBuilder sb = new SelectQueryBuilder(base.table);
            sb.SelectAll()
                .Where("closed", 0)
                .Where("time", "NOW()", CompareType.GreaterThanOrEqual);

            DataTable table = Run(sb.Statement);

            foreach (DataRow row in table.Rows)
            {
                list.Add(Order.Parse(row));
            }

            return list;
        }

        /// <summary>
        /// Checks if the order has been closed already or not
        /// </summary>
        /// <param name="order">The order to check</param>
        /// <returns></returns>
        public bool CanOrderBeCancelled(int order)
        {
            SelectQueryBuilder sb = new SelectQueryBuilder(base.table);
            sb.SelectAll()
                .Where("id", order)
                .Where("closed", 0)
                .Where("time", "NOW()", CompareType.GreaterThanOrEqual);

            DataTable dt = Run(sb.Statement);
            return dt.Rows.Count == 1;
        }

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <returns></returns>
        public List<Order> GetAllOrders()
        {
            List<Order> list = new List<Order>();

            SelectQueryBuilder sb = new SelectQueryBuilder(base.table);
            sb.SelectAll()
                .OrderBy("time", OrderType.Descending);

            DataTable table = Run(sb.Statement);

            foreach (DataRow row in table.Rows)
            {
                list.Add(Order.Parse(row, true));
            }

            return list;
        }

        /// <summary>
        /// Updates the closed-value of the order
        /// </summary>
        /// <param name="order">The order</param>
        /// <param name="closed">Closed</param>
        public void UpdateClosed(int order, bool closed)
        {
            UpdateQueryBuilder ub = new UpdateQueryBuilder(base.table);
            ub.Update("closed", closed ? 1 : 0);
            ub.Where("id", order);
            Update(ub.Statement);
        }

        /// <summary>
        /// Get the closed-time of the order
        /// </summary>
        /// <param name="order">The order</param>
        /// <returns>The closed time</returns>
        public DateTime GetClosedTime(int order)
        {
            SelectQueryBuilder sb = new SelectQueryBuilder(base.table);
            sb.SelectColumn("closed_time");
            sb.Where("id", order);

            DataTable dt = Run(sb.Statement);
            if (dt.Rows.Count == 1)
            {
                return dt.Rows[0].Field<DateTime>(0);
            }

            return new DateTime();
        }

        /// <summary>
        /// Creates a order
        /// </summary>
        /// <param name="time">The time of the order</param>
        /// <param name="admin">The admin of the order (mostly the creator)</param>
        /// <returns>If it was successfull or not</returns>
        public bool CreateOrder(DateTime time, int admin)
        {
            InsertQueryBuilder ib = new InsertQueryBuilder(base.table);
            ib.Insert("id", "NULL")
                .Insert("time", QueryBuilder.ValueWrap(time.ToString("yyyy-MM-dd H:mm:ss")))
                .Insert("created", "NOW()")
                .Insert("admin", admin)
                .Insert("closed", "0");

            return Update(ib.Statement);
        }
    }
}