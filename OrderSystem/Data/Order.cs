using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data
{
    public class Order
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

        public static Order Parse(DataRow row)
        {
            int id = row.Field<int>("id");
            DateTime time = row.Field<DateTime>("time");
            DateTime created = row.Field<DateTime>("created");
            bool closed = row.Field<sbyte>("closed") == 1;
            DateTime closedTime = row.Field<DateTime>("closed_time");

            return new Order(id, time, created, null, closed, closedTime);
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
        /// Determines if the food order is already closed or not
        /// </summary>
        public bool Closed
        {
            get { return closed; }
        }

        /// <summary>
        /// The time the food order was created
        /// </summary>
        public DateTime Created
        {
            get { return created; }
        }

        /// <summary>
        /// The time the food order was closed
        /// </summary>
        public DateTime ClosedTime
        {
            get { return closedTime; }
        }
    }
}