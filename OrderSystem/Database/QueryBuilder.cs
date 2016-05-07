using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Database
{
    /// <summary>
    /// The base class for all other query builders. You have to init every query builder with a table name.
    /// </summary>
    public abstract class QueryBuilder
    {
        protected string table;
        protected QueryCompiler compiler;

        public QueryBuilder(string table, bool wrap)
        {
            this.table = wrap ? NameWrap(table) : table;
            this.compiler = new QueryCompiler(this.table);
        }

        /// <summary>
        /// Wraps the name with ` characters to show that they are columns.
        /// </summary>
        /// <param name="name">The column to wrap</param>
        /// <returns>The wrapped column</returns>
        public static string NameWrap(string name)
        {
            if (name.StartsWith("`") && name.EndsWith("`"))
            {
                return name;
            }
            return string.Format("`{0}`", name);
        }

        /// <summary>
        /// Wraps the value with ' characters to show that they are strings (varchar).
        /// </summary>
        /// <param name="value">The value to wrap</param>
        /// <returns>The wrapped value</returns>
        public static string ValueWrap(string value)
        {
            if (value.StartsWith("'") && value.EndsWith("'"))
            {
                return value;
            }
            return string.Format("'{0}'", value);
        }

        /// <summary>
        /// Converts string-string dictionary to a list of tuples
        /// </summary>
        /// <param name="dictionary">The dictionary</param>
        /// <returns>the statement</returns>
        protected List<Tuple<string, string>> KeyValueDictToList(Dictionary<string, string> dictionary)
        {
            return dictionary.Select(pair => new Tuple<string, string>(pair.Key, pair.Value)).ToList();
        }

        /// <summary>
        /// The method every query builder has to implement
        /// </summary>
        /// <returns>The compiled statement</returns>
        protected abstract string CompileStatement();

        /// <summary>
        /// The statement the query builder has compiled
        /// </summary>
        public string Statement
        {
            get { return CompileStatement(); }
        }
    }
}