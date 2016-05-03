using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data
{
    public class OrderOverviewRow
    {
        private DateTime time;
        private int amount;
        private decimal sum;

        public OrderOverviewRow(DateTime time, int amount, decimal sum)
        {
            this.time = time;
            this.amount = amount;
            this.sum = sum;
        }

        public static OrderOverviewRow Parse(DataRow row)
        {
            DateTime time = row.Field<DateTime>("time");
            int amount = (int) row.Field<decimal>("amount");
            decimal sum = row.Field<decimal>("sum");

            return new OrderOverviewRow(time, amount, sum);
        }

        public DateTime Time
        {
            get { return time; }
        }

        public string TimeFormatted
        {
            get { return time.ToLongDateString() + " " + time.ToShortTimeString(); }
        }

        public int Amount
        {
            get { return amount; }
        }

        public decimal Sum
        {
            get { return sum; }
        }

        public string SumWithCurrency
        {
            get { return string.Format("€ {0,00}", sum); }
        }
    }
}
