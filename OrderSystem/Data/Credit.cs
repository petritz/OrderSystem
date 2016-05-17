using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Enums;
using OrderSystem.Models;

namespace OrderSystem.Data
{
    public class Credit
    {
        private int id;
        private User user;
        private decimal price;
        private CreditStatus status;
        private DateTime created;
        private DateTime modified;

        public Credit(int id, User user, decimal price, CreditStatus status, DateTime created, DateTime modified)
        {
            this.id = id;
            this.user = user;
            this.price = price;
            this.status = status;
            this.created = created;
            this.modified = modified;
        }

        public static Credit Parse(DataRow row, bool parseUser = false)
        {
            int id = row.Field<int>("id");
            int userId = (int)row.Field<uint>("user");
            decimal price = row.Field<decimal>("price");
            CreditStatus status = StringToStatus(row.Field<string>("status"));
            DateTime created = row.Field<DateTime>("created");
            DateTime modified = row.Field<DateTime>("modified");

            User user;

            if (parseUser)
            {
                UserModel model = (UserModel)ModelRegistry.Get(ModelIdentifier.User);
                user = model.GetUser(userId);
            }
            else
            {
                user = new User(userId, "", "", "", false);
            }

            return new Credit(id, user, price, status, created, modified);
        }

        /// <summary>
        /// Converts string to credit status enum
        /// </summary>
        /// <param name="str">The string to convert</param>
        /// <returns>the credit status, .Ok if nothing else found</returns>
        public static CreditStatus StringToStatus(string str)
        {
            switch (str)
            {
                case "ok":
                    return CreditStatus.Ok;
                case "pending":
                    return CreditStatus.Pending;
                case "deleted":
                    return CreditStatus.Deleted;
            }

            return CreditStatus.Ok;
        }

        /// <summary>
        /// Converts credit status enum to string for database
        /// </summary>
        /// <param name="status">The status to convert</param>
        /// <returns>The string representation of the status, "" if nothing else found</returns>
        public static string StatusToString(CreditStatus status)
        {
            switch (status)
            {
                case CreditStatus.Ok:
                    return "ok";
                case CreditStatus.Pending:
                    return "pending";
                case CreditStatus.Deleted:
                    return "deleted";
            }

            return "";
        }

        /// <summary>
        /// ID of the credit
        /// </summary>
        public int Id
        {
            get { return id; }
        }

        /// <summary>
        /// The user of the credit
        /// </summary>
        public User User
        {
            get { return user; }
        }

        /// <summary>
        /// The name of the user (full name)
        /// </summary>
        public string UserName
        {
            get { return $"{user.Firstname} {user.Lastname}";  }
        }

        /// <summary>
        /// Price of the credit
        /// </summary>
        public decimal Price
        {
            get { return price; }
        }

        /// <summary>
        /// THe price with currency; Format: € 0,00
        /// </summary>
        public string PriceWithCurrency
        {
            get { return string.Format("€ {0,00}", price); }
        }

        /// <summary>
        /// Status of the credit
        /// </summary>
        public CreditStatus Status
        {
            get { return status; }
        }

        /// <summary>
        /// Returns the status of the credit in a human-readable way
        /// </summary>
        public string StatusName
        {
            get
            {
                switch (Status)
                {
                    case CreditStatus.Ok:
                        return "OK";
                    case CreditStatus.Pending:
                        return "In Bearbeitung";
                    case CreditStatus.Deleted:
                        return "Gelöscht";
                }

                return "";
            }
        }

        /// <summary>
        /// The creation time of the credit
        /// </summary>
        public DateTime Created
        {
            get { return created; }
        }

        /// <summary>
        /// The creation time of the credit, formatted by shortDateString and shortTimeString
        /// </summary>
        public string CreatedFormatted
        {
            get { return created.ToShortDateString() + " " + created.ToShortTimeString(); }
        }

        /// <summary>
        /// The modified date of the credit
        /// </summary>
        public DateTime Modified
        {
            get { return modified; }
        }
    }
}
