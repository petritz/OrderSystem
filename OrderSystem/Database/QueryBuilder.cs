using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Database
{
    public abstract class QueryBuilder
    {
        protected string table;
        protected QueryCompiler compiler;

        public QueryBuilder(string table, bool wrap)
        {
            this.table = wrap ? NameWrap(table) : table;
            this.compiler = new QueryCompiler(this.table);
        }

        public static string NameWrap(string name)
        {
            if (name.StartsWith("`") && name.EndsWith("`"))
            {
                return name;
            }
            return string.Format("`{0}`", name);
        }

        public static string ValueWrap(string value)
        {
            if (value.StartsWith("'") && value.EndsWith("'"))
            {
                return value;
            }
            return string.Format("'{0}'", value);
        }

        protected abstract string CompileStatement();

        public string Statement
        {
            get { return CompileStatement(); }
        }
    }
}