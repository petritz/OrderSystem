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
    /// The query builder for select-statements. (completely chainable)
    /// </summary>
    public class SelectQueryBuilder : QueryBuilder
    {
        private List<string> selectList;
        private List<Tuple<string, string, CompareType>> whereList;
        private List<Tuple<string, OrderType>> orderList;
        private Tuple<long, long> limitTuple;

        public SelectQueryBuilder(string table, bool wrap = true) : base(table, wrap)
        {
            selectList = new List<string>();
            whereList = new List<Tuple<string, string, CompareType>>();
            orderList = new List<Tuple<string, OrderType>>();
        }

        /// <summary>
        /// Adds the astericks to select all available columns
        /// </summary>
        /// <returns>reference to this the query builder</returns>
        public SelectQueryBuilder SelectAll()
        {
            if (!selectList.Contains("DISTINCT") && selectList.Count > 0)
            {
                throw new QueryBuilderException("You cannot add *, if select cols are present.");
            }
            if (selectList.Contains("*"))
            {
                throw new QueryBuilderException("The * is already present.");
            }

            selectList.Add("*");
            return this;
        }

        /// <summary>
        /// Adds the DISTINCT keyword to the select list
        /// </summary>
        /// <returns>reference to this the query builder</returns>
        public SelectQueryBuilder SelectDistinct()
        {
            if (selectList.Count > 0)
            {
                throw new QueryBuilderException("The DISTINCT quantifier must be the first value.");
            }
            if (selectList.Contains("DISTINCT"))
            {
                throw new QueryBuilderException("DISTINCT is already present.");
            }

            selectList.Add("DISTINCT");
            return this;
        }

        /// <summary>
        /// Adds the specified column to the select list. The column is wrapped by `.
        /// </summary>
        /// <param name="col">The column to add</param>
        /// <returns>reference to this the query builder</returns>
        public SelectQueryBuilder SelectColumn(string col)
        {
            return Select(NameWrap(col));
        }

        /// <summary>
        /// Adds the specified value to the select list.
        /// </summary>
        /// <param name="value">The value to add</param>
        /// <returns>reference to this the query builder</returns>
        public SelectQueryBuilder Select(string value)
        {
            if (selectList.Contains("*"))
            {
                throw new QueryBuilderException("The * is present, so no more columns are allowed.");
            }

            selectList.Add(value);
            return this;
        }

        /// <summary>
        /// Adds another select statement into this select statement
        /// </summary>
        /// <param name="select"></param>
        /// <returns>reference to this the query builder</returns>
        public SelectQueryBuilder Select(SelectQueryBuilder select)
        {
            return Select(string.Format("( {0})", select.Statement));
        }

        /// <summary>
        /// Adds a list of columns to the select list
        /// </summary>
        /// <param name="cols">The columns to add. (not wrapped)</param>
        /// <returns>reference to this the query builder</returns>
        public SelectQueryBuilder Select(params string[] cols)
        {
            foreach (string col in cols)
            {
                Select(col);
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
        public SelectQueryBuilder Where(string col, object value, CompareType compare = CompareType.Equal)
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
        public SelectQueryBuilder OrderBy(string col, OrderType order = OrderType.Ascending)
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
        public SelectQueryBuilder Limit(long limit, long offset = 0)
        {
            limitTuple = new Tuple<long, long>(limit, offset);
            return this;
        }

        /// <summary>
        /// The logic to compile the statement.
        /// </summary>
        /// <returns></returns>
        protected override string CompileStatement()
        {
            StringBuilder sb = new StringBuilder();

            if (selectList.Count == 0)
            {
                throw new QueryBuilderException("There must be a column list defined.");
            }

            //SELECT and FROM
            sb.Append(compiler.Select(selectList));

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