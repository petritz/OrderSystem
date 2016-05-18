using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Enums;

namespace OrderSystem.Data
{
    /// <summary>
    /// Class that represents a row in the order overview page
    /// </summary>
    public class OrderOverviewRow
    {
        private int id;
        private DateTime time;
        private ulong amount;
        private decimal sum;
        private bool paid;
        private PayType payType;

        public OrderOverviewRow(int id, DateTime time, ulong amount, decimal sum, bool paid = false, PayType payType = PayType.Admin)
        {
            this.id = id;
            this.time = time;
            this.amount = amount;
            this.sum = sum;
            this.paid = paid;
            this.payType = payType;
        }

        /// <summary>
        /// Parsed a row from the database into an object
        /// </summary>
        /// <param name="row">The row to parse</param>
        /// <returns>The created object</returns>
        public static OrderOverviewRow Parse(DataRow row)
        {
            int id = (int) row.Field<uint>("order");
            DateTime time = row.Field<DateTime>("time");
            ulong amount = (ulong)row.Field<decimal>("amount");
            decimal sum = row.Field<decimal>("sum");
            decimal paidDecimal = row.Field<decimal>("paid");
            bool paid = paidDecimal == 1;
            PayType payType = AdminOrderUserRow.StringToPayType(row.Field<string>("pay_type"));

            return new OrderOverviewRow(id, time, amount, sum, paid, payType);
        }

        /// <summary>
        /// Id of the order
        /// </summary>
        public int Id
        {
            get { return id; }
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
        public ulong Amount
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

        /// <summary>
        /// If the user had paid the order or not
        /// </summary>
        public bool Paid
        {
            get { return paid; }
        }

        /// <summary>
        /// Pay type of order
        /// </summary>
        public PayType PayType
        {
            get { return payType; }
        }

        /// <summary>
        /// Name of the pay type
        /// </summary>
        public string PayTypeName
        {
            get
            {
                switch (PayType)
                {
                    case PayType.Admin:
                        return "Admin";
                    case PayType.Credit:
                        return "Guthaben";
                }

                return "";
            }
        }
    }
}