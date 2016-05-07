using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Enums;
using OrderSystem.Exceptions;

namespace OrderSystem.Database
{
    /// <summary>
    /// The query builder for update-statements. (completely chainable)
    /// </summary>
    public class UpdateQueryBuilder : QueryBuilder
    {
        private Dictionary<string, string> updateDictionary;
        private List<Tuple<string, string, CompareType>> whereList;
        private List<Tuple<string, OrderType>> orderList;
        private Tuple<long, long> limitTuple;

        /// <summary>
        /// Builds the query builder to the specified table
        /// </summary>
        /// <param name="table">The table for the statement</param>
        /// <param name="wrap">If the table name should be wrapped with ` characters</param>
        public UpdateQueryBuilder(string table, bool wrap = true) : base(table, wrap)
        {
            updateDictionary = new Dictionary<string, string>();
            whereList = new List<Tuple<string, string, CompareType>>();
            orderList = new List<Tuple<string, OrderType>>();
        }

        /// <summary>
        /// Adds the column and the value to the list
        /// </summary>
        /// <param name="column">The column to add</param>
        /// <param name="value">The vlaue for the column</param>
        /// <returns>reference to this query builder</returns>
        public UpdateQueryBuilder Update(string column, object value)
        {
            column = NameWrap(column);

            if (updateDictionary.ContainsKey(column))
            {
                throw new QueryBuilderException("The column is already in the list.");
            }

            updateDictionary[column] = value.ToString();
            return this;
        }

        /// <summary>
        /// Adds all name-value pairs to the list
        /// </summary>
        /// <param name="collection">The collection to add</param>
        /// <returns>reference to this query builder</returns>
        public UpdateQueryBuilder Insert(NameValueCollection collection)
        {
            foreach (string key in collection)
            {
                Update(key, collection[key]);
            }

            return this;
        }

        /// <summary>
        /// Adds all key-value pairs to the list
        /// </summary>
        /// <param name="dictionary">The dictionary to add</param>
        /// <returns>reference to this query builder</returns>
        public UpdateQueryBuilder Insert(Dictionary<string, string> dictionary)
        {
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                Update(pair.Key, pair.Value);
            }

            return this;
        }

        /// <summary>
        /// Adds a where condition.
        /// </summary>
        /// <param name="col">The column for the condition.</param>
        /// <param name="value">The value for the condition.</param>
        /// <param name="compare">The compare type of the condition.</param>
        /// <returns>reference to this the query builder</returns>
        public UpdateQueryBuilder Where(string col, object value, CompareType compare = CompareType.Equal)
        {
            whereList.Add(new Tuple<string, string, CompareType>(col, value.ToString(), compare));
            return this;
        }

        /// <summary>
        /// Adds a order by expression.
        /// </summary>
        /// <param name="col">The column to order.</param>
        /// <param name="order">The type of the order</param>
        /// <returns>reference to this the query builder</returns>
        public UpdateQueryBuilder OrderBy(string col, OrderType order = OrderType.Ascending)
        {
            orderList.Add(new Tuple<string, OrderType>(col, order));
            return this;
        }

        /// <summary>
        /// Adds a limit keyword to the query.
        /// </summary>
        /// <param name="limit">The limit row size.</param>
        /// <param name="offset">The offset size.</param>
        /// <returns>reference to this the query builder</returns>
        public UpdateQueryBuilder Limit(long limit, long offset = 0)
        {
            limitTuple = new Tuple<long, long>(limit, offset);
            return this;
        }

        /// <summary>
        /// The logic to compile the statement
        /// </summary>
        /// <returns>the statement</returns>
        protected override string CompileStatement()
        {
            StringBuilder sb = new StringBuilder();

            if (updateDictionary.Count == 0)
            {
                throw new QueryBuilderException("There must be a column list defined.");
            }

            //UPDATE and SET
            sb.Append(compiler.Update(KeyValueDictToList(updateDictionary)));

            //WHERE and AND
            sb.Append(compiler.Where(whereList));

            //ORDER BY
            sb.Append(compiler.OrderBy(orderList));
            
            //LIMIT
            sb.Append(compiler.Limit(limitTuple));

            return sb.ToString();
        }
    }
}
