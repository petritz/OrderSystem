using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data
{
    /// <summary>
    /// Class that represents a row in the order overview page
    /// </summary>
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

        /// <summary>
        /// Parsed a row from the database into an object
        /// </summary>
        /// <param name="row">The row to parse</param>
        /// <returns>The created object</returns>
        public static OrderOverviewRow Parse(DataRow row)
        {
            DateTime time = row.Field<DateTime>("time");
            int amount = (int)row.Field<decimal>("amount");
            decimal sum = row.Field<decimal>("sum");

            return new OrderOverviewRow(time, amount, sum);
        }

        /// <summary>
        /// Time of the order
        /// </summary>
        public DateTime Time
        {
            get { return time; }
        }

        /// <summary>
        /// Time of the order formatted using LongDateString and ShortTimeString
        /// </summary>
        public string TimeFormatted
        {
            get { return time.ToLongDateString() + " " + time.ToShortTimeString(); }
        }

        /// <summary>
        /// The amount of products ordered
        /// </summary>
        public int Amount
        {
            get { return amount; }
        }

        /// <summary>
        /// The sum of the products
        /// </summary>
        public decimal Sum
        {
            get { return sum; }
        }

        /// <summary>
        /// The sum with currency; Format: € 0,00
        /// </summary>
        public string SumWithCurrency
        {
            get { return string.Format("€ {0,00}", sum); }
        }
    }
}