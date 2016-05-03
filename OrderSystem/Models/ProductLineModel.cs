using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Data;
using OrderSystem.Database;

namespace OrderSystem.Models
{
    public class ProductLineModel : MainModel
    {
        public ProductLineModel() : base("product_line")
        {
            
        }

        public bool Submit(int userId, int orderId, List<ProductLine> elements)
        {
            foreach (ProductLine p in elements)
            {
                NameValueCollection col = new NameValueCollection();
                col.Add("id", "NULL");
                col.Add("user", "" + userId);
                col.Add("food_order", "" + orderId);
                col.Add("product", "" + p.Product.Id);
                col.Add("quantity", ""+ p.Quantity);
                col.Add("added", "NOW()");
                col.Add("paid", "0");

                if (!Insert(col))
                {
                    return false;
                }
            }

            return true;
        }

        public bool HasAlreadyOrdered(int userId, int orderId)
        {
            List<string> select = new List<string>();
            select.Add("*");

            NameValueCollection where = new NameValueCollection();
            where.Add("user", userId + "");
            where.Add("food_order", orderId + "");

            DataTable t = SelectWhere(select, where);
            return t.Rows.Count >= 1;
        }

        public List<OrderOverviewRow> GetOrdersFromUser(int userId)
        {
            List<OrderOverviewRow> list = new List<OrderOverviewRow>();
            string query = "SELECT f.time, SUM(l.quantity) \"amount\", SUM(l.quantity * p.price_sell) \"sum\" " +
                           "FROM product_line l " +
                           "INNER JOIN product p ON l.product = p.id " +
                           "INNER JOIN food_order f ON l.food_order = f.id " +
                           "WHERE l.user = " + userId + " " +
                           "GROUP BY l.food_order " +
                           "ORDER BY f.time DESC " +
                           "LIMIT 10";
            DataTable d = Run(query);
            foreach (DataRow row in d.Rows)
            {
                list.Add(OrderOverviewRow.Parse(row));
            }
            return list;
        }
    }
}
