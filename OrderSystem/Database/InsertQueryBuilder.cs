using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Exceptions;

namespace OrderSystem.Database
{
    /// <summary>
    /// The query builder for insert-statements. (completely chainable)
    /// </summary>
    public class InsertQueryBuilder : QueryBuilder
    {
        private Dictionary<string, string> insertDictionary;

        /// <summary>
        /// Builds the query builder to the specified table
        /// </summary>
        /// <param name="table">The table for the statement</param>
        /// <param name="wrap">If the table name should be wrapped with ` characters</param>
        public InsertQueryBuilder(string table, bool wrap = true) : base(table, wrap)
        {
            insertDictionary = new Dictionary<string, string>();
        }

        /// <summary>
        /// Adds the column and the value to the list
        /// </summary>
        /// <param name="column">The column to add</param>
        /// <param name="value">The value for the column</param>
        /// <returns>reference to this query builder</returns>
        public InsertQueryBuilder Insert(string column, object value)
        {
            column = NameWrap(column);

            if (insertDictionary.ContainsKey(column))
            {
                throw new QueryBuilderException("The column is already in the list.");
            }

            insertDictionary[column] = value.ToString();
            return this;
        }

        /// <summary>
        /// Adds all name-value pairs to the list
        /// </summary>
        /// <param name="collection">The collection to add</param>
        /// <returns>reference to this query builder</returns>
        public InsertQueryBuilder Insert(NameValueCollection collection)
        {
            foreach (string key in collection)
            {
                Insert(key, collection[key]);
            }

            return this;
        }

        /// <summary>
        /// Adds all key-value pairs to the list
        /// </summary>
        /// <param name="dictionary">The dictionary to add</param>
        /// <returns>reference to this query builder</returns>
        public InsertQueryBuilder Insert(Dictionary<string, string> dictionary)
        {
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                Insert(pair.Key, pair.Value);
            }

            return this;
        }

        /// <summary>
        /// The logic to compile the statement
        /// </summary>
        /// <returns>the statement</returns>
        protected override string CompileStatement()
        {
            StringBuilder sb = new StringBuilder();

            if (insertDictionary.Count == 0)
            {
                throw new QueryBuilderException("There must be some values to add.");
            }

            //INSERT INTO ... VALUES ...
            sb.Append(compiler.Insert(KeyValueDictToList(insertDictionary)));

            return sb.ToString();
        }
    }
}
