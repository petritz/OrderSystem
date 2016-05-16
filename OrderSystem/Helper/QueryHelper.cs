using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Database;
using OrderSystem.Enums;

namespace OrderSystem.Helper
{
    /// <summary>
    /// The helper class for the query builder.
    /// </summary>
    public class QueryHelper
    {
        /// <summary>
        /// Returns the predefined code to get the ip address of the user.
        /// </summary>
        /// <returns>The sql statement to get the ip address of the user.</returns>
        public static SelectQueryBuilder GetIpQuery()
        {
            SelectQueryBuilder query = new SelectQueryBuilder("information_schema.processlist", false);
            query.SelectColumn("host").Where("ID", "connection_id()");
            return query;
        }

        /// <summary>
        /// Converts DateTime object to a string that can be used in db queries
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The formatted date</returns>
        public static string ToDatabaseDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd H:mm:ss");
        }
    }
}