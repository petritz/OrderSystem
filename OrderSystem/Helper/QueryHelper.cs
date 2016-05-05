using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Database;
using OrderSystem.Enums;

namespace OrderSystem.Helper
{
    public class QueryHelper
    {
        public static SelectQueryBuilder GetIpQuery()
        {
            SelectQueryBuilder query = new SelectQueryBuilder("information_schema.processlist", false);
            query.SelectColumn("host").Where("ID", "connection_id()");
            return query;
        }
    }
}
