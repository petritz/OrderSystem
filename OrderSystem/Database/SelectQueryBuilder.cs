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
        private List<Tuple<JoinType, string, string>> joinList;
        private List<string> groupByList; 
        private List<Tuple<string, OrderType>> orderList;
        private Tuple<long, long> limitTuple;

        /// <summary>
        /// Builds the query builder to the specified table
        /// </summary>
        /// <param name="table">The table for the statement</param>
        /// <param name="wrap">If the table name should be wrapped with ` characters</param>
        public SelectQueryBuilder(string table, bool wrap = true) : base(table, wrap)
        {
            selectList = new List<string>();
            whereList = new List<Tuple<string, string, CompareType>>();
            joinList = new List<Tuple<JoinType, string, string>>();
            groupByList = new List<string>();
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
        public SelectQueryBuilder Select(SelectQueryBuilder select, string front = "", string back = "")
        {
            return Select(string.Format("{0} ( {1}) {2}", front, select.Statement, back));
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
        /// Adds a join expression
        /// </summary>
        /// <param name="type">The type of the join.</param>
        /// <param name="table">The table to join.</param>
        /// <param name="condition">The condition for the join.</param>
        /// <returns>reference to this the query builder</returns>
        public SelectQueryBuilder Join(JoinType type, string table, string condition)
        {
            joinList.Add(new Tuple<JoinType, string, string>(type, table, condition));
            return this;
        }

        /// <summary>
        /// Adds a group-by column
        /// </summary>
        /// <param name="col">The column to group (will not be wrapped in `)</param>
        /// <returns>reference to this the query builder</returns>
        public SelectQueryBuilder GroupBy(string col)
        {
            if (groupByList.Contains(col))
            {
                throw new QueryBuilderException("The column is already defined in the group-by list.");
            }

            groupByList.Add(col);
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
        /// <returns>the statement</returns>
        protected override string CompileStatement()
        {
            StringBuilder sb = new StringBuilder();

            if (selectList.Count == 0)
            {
                throw new QueryBuilderException("There must be a column list defined.");
            }

            //SELECT and FROM
            sb.Append(compiler.Select(selectList));

            //JOIN
            sb.Append(compiler.Join(joinList));

            //WHERE and AND
            sb.Append(compiler.Where(whereList));

            //GROUP BY
            sb.Append(compiler.GroupBy(groupByList));

            //ORDER BY
            sb.Append(compiler.OrderBy(orderList));

            //LIMIT
            sb.Append(compiler.Limit(limitTuple));

            return sb.ToString();
        }
    }
}