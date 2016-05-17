using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        private PayType payType;

        public AdminOrderUserRow(int order, User user, decimal sum, bool paid, PayType payType)
        {
            this.order = order;
            this.user = user;
            this.sum = sum;
            this.paid = paid;
            this.payType = payType;
        }

        /// <summary>
        /// Parsed a database row into a admin order user row object
        /// </summary>
        /// <param name="row">The row to parse</param>
        /// <param name="order">The order id</param>
        /// <returns>The parsed object</returns>
        public static AdminOrderUserRow Parse(DataRow row, int order)
        {
            int userId = (int)row.Field<uint>("user");
            decimal sum = row.Field<decimal>("sum");
            long hasPaid = (long)row.Field<decimal>("paid");
            string payTypeStr = row.Field<string>("pay_type");

            UserModel model = (UserModel)ModelRegistry.Get(ModelIdentifier.User);
            Data.User user = model.GetUser(userId);
            bool paid = hasPaid == 1;
            PayType payType = StringToPayType(payTypeStr);

            return new AdminOrderUserRow(order, user, sum, paid, payType);
        }

        /// <summary>
        /// Converts string to pay type enum
        /// </summary>
        /// <param name="str">the string to convert</param>
        /// <returns>The pay type, .Admin if nothing else found</returns>
        public static PayType StringToPayType(string str)
        {
            switch (str)
            {
                case "admin":
                    return PayType.Admin;
                case "credit":
                    return PayType.Credit;
            }

            return PayType.Admin;
        }

        /// <summary>
        /// Converts pay type enum to string for database
        /// </summary>
        /// <param name="type">The type to convert</param>
        /// <returns>The string representation of the pay type, "" if nothing else found</returns>
        public static string PayTypeToString(PayType type)
        {
            switch (type)
            {
                case PayType.Admin:
                    return "admin";
                case PayType.Credit:
                    return "credit";
            }

            return "";
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
                try
                {
                    if (Paid && PayType == PayType.Credit)
                    {
                        throw new Exception("Der Benutzer hat schon über Credit bezahlt.");
                    }

                    ProductLineModel productLineModel = (ProductLineModel)ModelRegistry.Get(ModelIdentifier.ProductLine);
                    if (productLineModel.SetPaidOrder(Order, Session.Instance.CurrentUserId, value))
                    {
                        paid = value;
                        RefreshPayType();
                        OnPropertyChanged("PayType");
                        OnPropertyChanged("PayTypeName");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Get the pay type
        /// </summary>
        public PayType PayType
        {
            get { return payType; }
        }

        /// <summary>
        /// Get the pay type name
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
                        return "Credit";
                }

                return "";
            }
        }

        private void RefreshPayType()
        {
            ProductLineModel productLineModel = (ProductLineModel)ModelRegistry.Get(ModelIdentifier.ProductLine);
            payType = productLineModel.GetPayType(Order, Session.Instance.CurrentUserId);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
