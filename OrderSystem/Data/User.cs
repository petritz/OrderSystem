using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data
{
    public class User
    {
        private readonly int id;
        private readonly string email;
        private readonly string firstname;
        private readonly string lastname;
        private readonly bool admin;

        public User(int id, string email, string firstname, string lastname, bool admin)
        {
            this.id = id;
            this.email = email;
            this.firstname = firstname;
            this.lastname = lastname;
            this.admin = admin;
        }

        public int Id
        {
            get { return id; }
        }

        public string Email
        {
            get { return email; }
        }

        public string Firstname
        {
            get { return firstname; }
        }

        public string Lastname
        {
            get { return lastname; }
        }

        public bool Admin
        {
            get { return admin; }
        }

        // Database Parsing

        public static User Parse(DataRow row)
        {
            int id = row.Field<int>("id");
            string firstname = row.Field<string>("firstname");
            string lastname = row.Field<string>("lastname");
            string email = row.Field<string>("email");
            bool admin = row.Field<byte>("admin") == 1;

            return new User(id, email, firstname, lastname, admin);
        }
    }
}