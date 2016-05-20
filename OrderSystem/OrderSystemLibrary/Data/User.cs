using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystemLibrary.Data
{
    /// <summary>
    /// Class that represents the user table in the database.
    /// </summary>
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

        /// <summary>
        /// ID of the user
        /// </summary>
        public int Id
        {
            get { return id; }
        }

        /// <summary>
        /// Email of the user
        /// </summary>
        public string Email
        {
            get { return email; }
        }

        /// <summary>
        /// First name of the user
        /// </summary>
        public string Firstname
        {
            get { return firstname; }
        }

        /// <summary>
        /// Last name of the user
        /// </summary>
        public string Lastname
        {
            get { return lastname; }
        }

        /// <summary>
        /// Determines if the user is a admin or not
        /// </summary>
        public bool Admin
        {
            get { return admin; }
        }

        /// <summary>
        /// Parses the row from the database to the user object
        /// </summary>
        /// <param name="row">The row to parse</param>
        /// <returns>The parsed object</returns>
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