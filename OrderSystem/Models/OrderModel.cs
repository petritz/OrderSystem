using OrderSystem.Data;
using OrderSystem.Database;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Models
{
    public class OrderModel : MainModel
    {
        public OrderModel() : base("food_order")
        {

        }

        public List<Order> GetTimes()
        {
            List<Order> list = new List<Order>();

            List<string> select = new List<string>();
            select.Add("*");

            NameValueCollection where = new NameValueCollection();
            where.Add("closed", "0");

            DataTable table = SelectWhere(select, where);

            foreach (DataRow row in table.Rows)
            {
                list.Add(Order.Parse(row));
            }

            return list;
        }
    }
}
