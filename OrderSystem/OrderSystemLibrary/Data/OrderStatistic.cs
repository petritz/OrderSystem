using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystemLibrary.Data
{
    /// <summary>
    /// A holder class for statistic values
    /// </summary>
    public class OrderStatistic
    {
        /// <summary>
        /// The total price a user has spent
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// The total amount of products a user bought
        /// </summary>
        public ulong BoughtProducts { get; set; }
    }
}