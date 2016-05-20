using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystemLibrary.Data
{
    /// <summary>
    /// Class that represents a row in the admin open orders page
    /// </summary>
    public class AdminOpenOrderRow
    {
        private int id;
        private DateTime time;
        private decimal sumToPay;
        private decimal sumPaid;
        private ulong notPaidUsers;

        public AdminOpenOrderRow(int id, DateTime time, decimal sumToPay, decimal sumPaid, ulong notPaidUsers)
        {
            this.id = id;
            this.time = time;
            this.sumToPay = sumToPay;
            this.sumPaid = sumPaid;
            this.notPaidUsers = notPaidUsers;
        }

        /// <summary>
        /// Parses a row from the database into an object
        /// </summary>
        /// <param name="row">The row to parse</param>
        /// <returns>The created object</returns>
        public static AdminOpenOrderRow Parse(DataRow row)
        {
            int id = (int) row.Field<uint>("order");
            DateTime time = row.Field<DateTime>("time");
            decimal sumToPay = row.Field<decimal>("sumToPay");
            decimal sumPaid = row.Field<decimal>("sumPaid");
            ulong notPaidUsers = (ulong) row.Field<long>("users");

            return new AdminOpenOrderRow(id, time, sumToPay, sumPaid, notPaidUsers);
        }

        /// <summary>
        /// Id of the order
        /// </summary>
        public int Id
        {
            get { return id; }
        }

        /// <summary>
        /// Time of the order formatted using LongDateString and ShortTimeString
        /// </summary>
        public string TimeFormatted
        {
            get { return time.ToLongDateString() + " " + time.ToShortTimeString(); }
        }

        /// <summary>
        /// Time of the order
        /// </summary>
        public DateTime Time
        {
            get { return time; }
        }

        /// <summary>
        /// Sum that users have to pay
        /// </summary>
        public decimal SumToPay
        {
            get { return sumToPay; }
        }

        /// <summary>
        /// Sum that users have to pay with currency
        /// </summary>
        public string SumToPayWithCurrency
        {
            get { return string.Format("€ {0,00}", SumToPay); }
        }

        /// <summary>
        /// Sum that users paid
        /// </summary>
        public decimal SumPaid
        {
            get { return sumPaid; }
        }

        /// <summary>
        /// Sum that users paid with currency
        /// </summary>
        public string SumPaidWithCurrency
        {
            get { return string.Format("€ {0,00}", SumPaid); }
        }

        /// <summary>
        /// Amount of users that didnt paid
        /// </summary>
        public ulong NotPaidUsers
        {
            get { return notPaidUsers; }
        }
    }
}
