using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Data;
using OrderSystem.Database;
using OrderSystem.Enums;
using OrderSystem.Helper;

namespace OrderSystem.Models
{
    /// <summary>
    /// The model for the credit table
    /// </summary>
    public class CreditModel : MainModel
    {
        public CreditModel() : base("credit")
        {
        }

        /// <summary>
        /// Get pending credits
        /// </summary>
        /// <param name="user">´The user id</param>
        /// <returns>List of pending credits</returns>
        public List<Credit> GetPendingCredits(int user)
        {
            List<Credit> list = new List<Credit>();

            SelectQueryBuilder sb = new SelectQueryBuilder(base.table);
            sb.SelectAll()
                .Where("user", user)
                .Where("status", QueryBuilder.ValueWrap("pending"));

            DataTable dt = Run(sb.Statement);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(Credit.Parse(row));
            }

            return list;
        }

        /// <summary>
        /// Gets credit of a specific user
        /// </summary>
        /// <param name="user">The user id</param>
        /// <returns>The credit or 0</returns>
        public decimal GetCurrentCredit(int user)
        {
            SelectQueryBuilder sb = new SelectQueryBuilder(base.table);
            sb.Select("COALESCE(SUM(price), 0) as sum");
            sb.Where("user", user);
            sb.Where("status", QueryBuilder.ValueWrap("ok"));

            DataTable dt = Run(sb.Statement);
            if (dt.Rows.Count == 1)
            {
                DataRow row = dt.Rows[0];
                return row.Field<decimal>("sum");
            }

            return 0;
        }

        /// <summary>
        /// Add the credit to a specific user
        /// </summary>
        /// <param name="price">The price</param>
        /// <param name="userId">The user id</param>
        /// <returns>If it was successful or not</returns>
        public bool AddCredit(decimal price, int userId, bool ok = false)
        {
            InsertQueryBuilder ib = new InsertQueryBuilder(base.table);
            ib.Insert("id", "NULL")
                .Insert("user", userId)
                .Insert("price", price.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));

            if (ok || price < 0)
            {
                ib.Insert("status", QueryBuilder.ValueWrap("ok"));
            }

            return Update(ib.Statement);
        }

        /// <summary>
        /// Sets deleted flag of a specific credit
        /// </summary>
        /// <param name="id">The credit id</param>
        /// <returns>If it was successful or not</returns>
        public bool Delete(int id)
        {
            UpdateQueryBuilder ub = new UpdateQueryBuilder(base.table);
            ub.Update("status", QueryBuilder.ValueWrap("deleted"));
            ub.Where("id", id);

            return Update(ub.Statement);
        }

        /// <summary>
        /// Sets the ok flag of a specific credit
        /// </summary>
        /// <param name="id">The credit id</param>
        /// <returns>If it was successful or not</returns>
        public bool Accept(int id)
        {
            UpdateQueryBuilder ub = new UpdateQueryBuilder(base.table);
            ub.Update("status", QueryBuilder.ValueWrap("ok"));
            ub.Where("id", id);

            return Update(ub.Statement);
        }

        /// <summary>
        /// Get open credits that a admin has to accept
        /// </summary>
        /// <returns>list of credits</returns>
        public List<Credit> GetAllOpenCredits()
        {
            List<Credit> list = new List<Credit>();

            SelectQueryBuilder sb = new SelectQueryBuilder(base.table);
            sb.SelectAll()
                .Where("status", QueryBuilder.ValueWrap("pending"))
                .Where("price", 0, CompareType.GreaterThan);

            DataTable dt = Run(sb.Statement);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(Credit.Parse(row, true));
            }

            return list;
        }
    }
}