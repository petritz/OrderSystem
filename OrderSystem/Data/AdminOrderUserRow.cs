using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Annotations;
using OrderSystem.Enums;
using OrderSystem.Models;

namespace OrderSystem.Data
{
    /// <summary>
    /// Class that represents row in the admin order view dialog
    /// </summary>
    public class AdminOrderUserRow : INotifyPropertyChanged
    {
        private int order;
        private User user;
        private decimal sum;
        private bool paid;

        public AdminOrderUserRow(int order, User user, decimal sum, bool paid)
        {
            this.order = order;
            this.user = user;
            this.sum = sum;
            this.paid = paid;
        }

        public static AdminOrderUserRow Parse(DataRow row, int order)
        {
            int userId = (int) row.Field<uint>("user");
            decimal sum = row.Field<decimal>("sum");
            long hasPaid = (long) row.Field<decimal>("paid");

            UserModel model = (UserModel) ModelRegistry.Get(ModelIdentifier.User);
            Data.User user = model.GetUser(userId);
            bool paid = hasPaid == 1;

            return new AdminOrderUserRow(order, user, sum, paid);
        }

        /// <summary>
        /// The id of the order
        /// </summary>
        public int Order
        {
            get { return order; }
        }

        /// <summary>
        /// The user
        /// </summary>
        public User User
        {
            get { return user; }
        }

        /// <summary>
        /// Full name of the user
        /// </summary>
        public string UserName
        {
            get { return $"{user.Firstname} {user.Lastname}"; }
        }

        /// <summary>
        /// The sum
        /// </summary>
        public decimal Sum
        {
            get { return sum; }
        }

        /// <summary>
        /// The sum with currency
        /// </summary>
        public string SumWithCurrency
        {
            get { return string.Format("€ {0,00}", Sum); }
        }

        /// <summary>
        /// If it was paid or not
        /// </summary>
        public bool Paid
        {
            get { return paid; }
            set
            {
                ProductLineModel productLineModel = (ProductLineModel) ModelRegistry.Get(ModelIdentifier.ProductLine);
                if (productLineModel.SetPaidOrder(Order, Session.Instance.CurrentUserId, value))
                {
                    paid = value;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
