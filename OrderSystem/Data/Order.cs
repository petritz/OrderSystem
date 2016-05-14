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
    public class Order : INotifyPropertyChanged
    {
        private int id;
        private DateTime time;
        private DateTime created;
        private User admin;
        private bool closed;
        private DateTime closedTime;

        public Order(int id, DateTime time, DateTime created, User admin, bool closed, DateTime closedTime)
        {
            this.id = id;
            this.time = time;
            this.created = created;
            this.admin = admin;
            this.closed = closed;
            this.closedTime = closedTime;
        }

        public static Order Parse(DataRow row, bool parseAdmin = false)
        {
            int id = row.Field<int>("id");
            DateTime time = row.Field<DateTime>("time");
            DateTime created = row.Field<DateTime>("created");
            bool closed = row.Field<sbyte>("closed") == 1;
            DateTime closedTime = row.Field<DateTime>("closed_time");
            User adminUser = null;

            if (parseAdmin)
            {
                UserModel model = (UserModel)ModelRegistry.Get(ModelIdentifier.User);
                int admin = (int)row.Field<uint>("admin");
                adminUser = model.GetUser(admin);
            }

            return new Order(id, time, created, adminUser, closed, closedTime);
        }

        /// <summary>
        /// ID of the food order
        /// </summary>
        public int Id
        {
            get { return id; }
        }

        /// <summary>
        /// The time of the food order
        /// </summary>
        public DateTime Time
        {
            get { return time; }
        }

        /// <summary>
        /// The time of the food order, formatted by shortDateString and shortTimeString
        /// </summary>
        public string TimeFormatted
        {
            get { return time.ToShortDateString() + " " + time.ToShortTimeString(); }
        }

        /// <summary>
        /// The admin of this food order
        /// </summary>
        public User Admin
        {
            get { return admin; }
        }

        /// <summary>
        /// The name of the admin (firstname + lastname)
        /// </summary>
        public string AdminName
        {
            get
            {
                if (admin == null) return "";
                return admin.Firstname + " " + admin.Lastname;
            }
        }

        /// <summary>
        /// Determines if the food order is already closed or not
        /// </summary>
        public bool Closed
        {
            get { return closed; }
            set
            {
                closed = value;
                RefreshClosedTime();
                OnPropertyChanged("ClosedTimeFormatted");
            }
        }

        /// <summary>
        /// The time the food order was created
        /// </summary>
        public DateTime Created
        {
            get { return created; }
        }

        /// <summary>
        /// The creation time of the food order, formatted by shortDateString and shortTimeString
        /// </summary>
        public string CreatedFormatted
        {
            get { return created.ToShortDateString() + " " + created.ToShortTimeString(); }
        }

        private void RefreshClosedTime()
        {
            OrderModel model = (OrderModel) ModelRegistry.Get(ModelIdentifier.Order);
            model.UpdateClosed(Id, closed);
            closedTime = model.GetClosedTime(Id);
        }

        /// <summary>
        /// The time the food order was closed
        /// </summary>
        public DateTime ClosedTime
        {
            get { return closedTime; }
        }

        /// <summary>
        /// The closed time of the food order, formatted by shortDateString and shortTimeString
        /// </summary>
        public string ClosedTimeFormatted
        {
            get { return closedTime.ToShortDateString() + " " + closedTime.ToShortTimeString(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}