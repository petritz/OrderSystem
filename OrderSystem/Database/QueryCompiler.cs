using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Enums;

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
                if (i == selects.Count - 1 || selects[i].Equals("DISTINCT"))
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

        public string Where(List<Tuple<string, string, CompareType>> cols)
        {
            if (cols.Count == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            bool usedWhere = false;

            foreach (Tuple<string, string, CompareType> col in cols)
            {
                if (!usedWhere)
                {
                    sb.Append("WHERE ");
                    usedWhere = true;
                }
                else
                {
                    sb.Append("AND ");
                }

                sb.Append(col.Item1)
                    .Append(" ")
                    .Append(CompareToString(col.Item3))
                    .Append(" ")
                    .Append(col.Item2)
                    .Append(" ");
            }

            return sb.ToString();
        }

        public string CompareToString(CompareType type)
        {
            switch (type)
            {
                case CompareType.Equal:
                    return "=";
                case CompareType.GreaterThan:
                    return ">";
                case CompareType.GreaterThanOrEqual:
                    return ">=";
                case CompareType.Is:
                    return "IS";
                case CompareType.IsNot:
                    return "IS NOT";
                case CompareType.LessThan:
                    return "<";
                case CompareType.LessThanOrEqual:
                    return "<=";
                case CompareType.NotEqual:
                    return "<>";
                default:
                    return "";
            }
        }
    }
}
