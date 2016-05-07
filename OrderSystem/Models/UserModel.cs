using OrderSystem.Data;
using OrderSystem.Database;
using OrderSystem.Helper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrderSystem.Models
{
    /// <summary>
    /// The model for the user table.
    /// </summary>
    public class UserModel : MainModel
    {
        public UserModel() : base("user")
        {
        }

        /// <summary>
        /// Checks if the user is present in the database and has supplied a valid password. Also updates the timestamps.
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password (will be hashed)</param>
        /// <returns>If it was succesful or not.</returns>
        public bool Login(string username, string password)
        {
            string md5Password = HashHelper.CreateMD5(password);
            return LoginMd5(username, md5Password);
        }

        /// <summary>
        /// Checks if the user is present in the database and has supplied a valid password. Also updates the timestamps.
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password (already hashed)</param>
        /// <returns>If it was succesful or not.</returns>
        public bool LoginMd5(string username, string password)
        {
            SelectQueryBuilder sb = new SelectQueryBuilder(base.table);
            sb.SelectAll()
                .Where("email", QueryBuilder.ValueWrap(username))
                .Where("password", QueryBuilder.ValueWrap(password));

            DataTable table = Run(sb.Statement);
            if (table == null) return false;
            if (table.Rows.Count != 1) return false;

            UpdateUser(username);
            return true;
        }

        /// <summary>
        /// Adds the user to the database.
        /// </summary>
        /// <param name="firstname">The first name of the user</param>
        /// <param name="lastname">The last name of the user</param>
        /// <param name="email">The email of the user</param>
        /// <param name="password">The password of the user (will be hashed)</param>
        /// <returns>If it was successful or not.</returns>
        public bool Register(string firstname, string lastname, string email, string password)
        {
            InsertQueryBuilder ib = new InsertQueryBuilder(base.table);
            ib.Insert("id", "NULL");
            ib.Insert("email", QueryBuilder.ValueWrap(email));
            ib.Insert("firstname", QueryBuilder.ValueWrap(firstname));
            ib.Insert("lastname", QueryBuilder.ValueWrap(lastname));
            ib.Insert("password", QueryBuilder.ValueWrap(HashHelper.CreateMD5(password)));
            ib.Insert("created", "NOW()");
            ib.Insert("ip", QueryHelper.GetIpQuery());

            return Update(ib.Statement);
        }

        /// <summary>
        /// Changes the password of the user.
        /// </summary>
        /// <param name="id">The user</param>
        /// <param name="password">The new password</param>
        /// <returns>If it was successfull or not.</returns>
        public bool ChangePassword(int id, string password)
        {
            UpdateQueryBuilder ub = new UpdateQueryBuilder(base.table);
            ub.Update("password", QueryBuilder.ValueWrap(HashHelper.CreateMD5(password)));
            ub.Where("id", id);

            return Update(ub.Statement);
        }

        /// <summary>
        /// Get the user id of the user by the given email
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>The id of the user</returns>
        public int GetUserId(string email)
        {
            SelectQueryBuilder sb = new SelectQueryBuilder(base.table);
            sb.SelectColumn("id")
                .Where("email", QueryBuilder.ValueWrap(email));

            DataTable table = Run(sb.Statement);
            return int.Parse(table.Rows[0][0].ToString());
        }

        /// <summary>
        /// Gets the user object
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <returns>The object of the user</returns>
        public User GetUser(int id)
        {
            DataTable table = Run(new SelectQueryBuilder(base.table).SelectAll().Where("id", id).Statement);
            return User.Parse(table.Rows[0]);
        }

        /// <summary>
        /// Updates user timestamp and ip address.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        public void UpdateUser(string email)
        {
            UpdateQueryBuilder ub = new UpdateQueryBuilder(base.table);
            ub.Update("ip", QueryHelper.GetIpQuery());
            ub.Update("last_login", "NOW()");
            ub.Where("email", QueryBuilder.ValueWrap(email));

            Update(ub.Statement);
        }

        /// <summary>
        /// Checks if the password is secure enough.
        /// </summary>
        /// <param name="password">The password</param>
        /// <returns>If it is secure or not.</returns>
        public bool PasswordCheck(string password)
        {
            Regex regex = new Regex(@"(^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z]{8,}$)");
            Match match = regex.Match(password);
            return match.Success;
        }
    }
}