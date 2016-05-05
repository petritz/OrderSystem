using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Database
{
    public class QueryCompiler
    {
        private string table;

        public QueryCompiler(string table)
        {
            this.table = table;
        }

        public string Select(List<string> selects)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            for (int i = 0; i < selects.Count; i++)
            {
                if (i == selects.Count - 1)
                {
                    sb.Append(selects[i]).Append(" ");
                }
                else
                {
                    sb.Append(selects[i]).Append(", ");
                }
            }
            sb.Append("FROM ").Append(table).Append(" ");
            return sb.ToString();
        }
    }
}
