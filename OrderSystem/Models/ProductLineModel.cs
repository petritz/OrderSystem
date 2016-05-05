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
                col.Add("quantity", "" + p.Quantity);
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
            SelectQueryBuilder sb = new SelectQueryBuilder(base.table);
            sb.SelectAll()
                .Where("user", userId)
                .Where("food_order", orderId);

            DataTable t = Run(sb.Statement);
            return t.Rows.Count >= 1;
        }

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
                int products = (int) row.Field<decimal>(0);
                decimal sum = row.Field<decimal>(1);
                statistic.BoughtProducts = products;
                statistic.TotalPrice = sum;
            }

            return statistic;
        }
    }
}