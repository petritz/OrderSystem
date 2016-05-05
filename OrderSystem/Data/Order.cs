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

        public int Id
        {
            get { return id; }
        }

        public DateTime Time
        {
            get { return time; }
        }

        public string TimeFormatted
        {
            get { return time.ToShortDateString() + " " + time.ToShortTimeString(); }
        }

        public User Admin
        {
            get { return admin; }
        }

        public bool Closed
        {
            get { return closed; }
        }

        public DateTime Created
        {
            get { return created; }
        }

        public DateTime ClosedTime
        {
            get { return closedTime; }
        }
    }
}