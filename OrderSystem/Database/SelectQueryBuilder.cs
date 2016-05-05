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
    public class SelectQueryBuilder : QueryBuilder
    {
        private List<string> selectList;
        private List<Tuple<string, string, CompareType>> whereList;
        private List<Tuple<string, OrderType>> orderList;

        public SelectQueryBuilder(string table) : base(table)
        {
            selectList = new List<string>();
            whereList = new List<Tuple<string, string, CompareType>>();
            orderList = new List<Tuple<string, OrderType>>();
        }

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

        public SelectQueryBuilder SelectColumn(string col)
        {
            return Select(NameWrap(col));
        }

        public SelectQueryBuilder Select(string value)
        {
            if (selectList.Contains("*"))
            {
                throw new QueryBuilderException("The * is present, so no more columns are allowed.");
            }

            selectList.Add(value);
            return this;
        }

        public SelectQueryBuilder Select(SelectQueryBuilder select)
        {
            return Select(string.Format("( {0} )", select.Statement));
        }

        public SelectQueryBuilder Select(params string[] cols)
        {
            foreach (string col in cols)
            {
                Select(col);
            }
            return this;
        }

        public SelectQueryBuilder Where(string col, object value, CompareType compare)
        {
            whereList.Add(new Tuple<string, string, CompareType>(col, value.ToString(), compare));
            return this;
        }

        public SelectQueryBuilder OrderBy(string col, OrderType order)
        {
            orderList.Add(new Tuple<string, OrderType>(col, order));
            return this;
        }

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
            
            return sb.ToString();
        }
    }
}
