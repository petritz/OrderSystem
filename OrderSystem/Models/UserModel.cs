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
    public class UserModel : MainModel
    {
        public UserModel() : base("user")
        {
        }

        public bool Login(string username, string password)
        {
            string md5Password = HashHelper.CreateMD5(password);
            return LoginMd5(username, md5Password);
        }

        public bool LoginMd5(string username, string password)
        {
            NameValueCollection col = new NameValueCollection();
            col.Add("email", Wrap(username));
            col.Add("password", Wrap(password));

            DataTable table = Where(col);
            if (table == null) return false;
            if (table.Rows.Count != 1) return false;

            UpdateUser(username);
            return true;
        }

        public bool Register(string firstname, string lastname, string email, string password)
        {
            NameValueCollection col = new NameValueCollection();
            col.Add("id", "NULL");
            col.Add("email", Wrap(email));
            col.Add("firstname", Wrap(firstname));
            col.Add("lastname", Wrap(lastname));
            col.Add("password", Wrap(HashHelper.CreateMD5(password)));
            col.Add("created", "NOW()");
            col.Add("ip", QueryGetIp());

            return Insert(col);
        }

        public bool ChangePassword(int id, string password)
        {
            NameValueCollection where = new NameValueCollection();
            where.Add("id", "" + id);

            NameValueCollection update = new NameValueCollection();
            update.Add("password", Wrap(HashHelper.CreateMD5(password)));

            return Update(update, where);
        }

        public int GetUserId(string email)
        {
            SelectQueryBuilder sb = new SelectQueryBuilder(base.table);
            sb.SelectColumn("id")
                .Where("email", QueryBuilder.ValueWrap(email));

            DataTable table = Run(sb.Statement);
            return int.Parse(table.Rows[0][0].ToString());
        }

        public User GetUser(int id)
        {
            DataTable table = Run(new SelectQueryBuilder(base.table).SelectAll().Where("id", id).Statement);
            return User.Parse(table.Rows[0]);
        }

        public void UpdateUser(string email)
        {
            NameValueCollection where = new NameValueCollection();
            where.Add("email", email);

            NameValueCollection data = new NameValueCollection();
            data.Add("ip", QueryGetIp());
            data.Add("last_login", "NOW()");

            Update(data, where);
        }

        public bool PasswordCheck(string password)
        {
            Regex regex = new Regex(@"(^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z]{8,}$)");
            Match match = regex.Match(password);
            return match.Success;
        }
    }
}