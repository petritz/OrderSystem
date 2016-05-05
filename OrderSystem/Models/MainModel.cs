using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Database
{
    /// <summary>
    /// The main model class that makes the database connection and stores the table name.
    /// </summary>
    public class MainModel
    {
        protected DAL dal;
        protected string table;
        
        public MainModel(string table)
        {
            this.table = table;
            this.dal = DAL.Instance;
        }
        
        public DataTable SelectAll()
        {
            return Run(QuerySelectAll());
        }

        public DataTable SelectWhere(List<string> select, NameValueCollection where)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(QuerySelect(select));
            sb.Append(QueryWhere(where));
            return Run(sb.ToString());
        }

        public DataTable Where(NameValueCollection columns)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(QuerySelectAll());
            sb.Append(QueryWhere(columns));
            return Run(sb.ToString());
        }

        public bool Insert(NameValueCollection columns)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(QueryInsert());
            sb.Append(QueryInsertColumns(columns));
            sb.Append(QueryInsertValues(columns));
            return Update(sb.ToString());
        }

        public bool Update(NameValueCollection data, NameValueCollection where)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(QueryUpdate());
            sb.Append(QueryUpdateData(data));
            sb.Append(QueryWhere(where));
            return Update(sb.ToString());
        }

        /// <summary>
        /// Runs the sql query in the database and returns the data table.
        /// </summary>
        /// <param name="query">The query to run</param>
        /// <returns>The data table</returns>
        public DataTable Run(string query)
        {
            MySqlCommand command = new MySqlCommand(query, dal.Connection);
            DataTable table = new DataTable();

            try
            {
                dal.Connection.Open();
                Console.WriteLine("Executing: " + query); //TODO write logger
                MySqlDataReader reader = command.ExecuteReader();

                table.Load(reader);
                return table;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                dal.Connection.Close();
            }

            return null;
        }

        /// <summary>
        /// Runs a insert/update/delete statement in the database
        /// </summary>
        /// <param name="query">The query to run</param>
        /// <returns>If the number of rows returned was 1. (mostly this means success)</returns>
        public bool Update(string query)
        {
            MySqlCommand command = new MySqlCommand(query, dal.Connection);

            try
            {
                dal.Connection.Open();
                Console.WriteLine("Executing Update: " + query);
                int ret = command.ExecuteNonQuery();
                return ret == 1;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                dal.Connection.Close();
            }

            return false;
        }

        // "Query Builder (deprecated)

        protected string QuerySelectAll()
        {
            List<string> selects = new List<string>();
            selects.Add("*");
            return QuerySelect(selects);
        }

        protected string QuerySelect(List<string> selects)
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
            sb.Append("FROM ").Append(table);
            return sb.ToString();
        }

        protected string QueryWhere(NameValueCollection columns)
        {
            StringBuilder sb = new StringBuilder();
            bool usedWhere = false;

            foreach (string key in columns)
            {
                if (!usedWhere)
                {
                    sb.Append(" WHERE ");
                    usedWhere = true;
                }
                else
                {
                    sb.Append(" AND ");
                }
                sb.Append(key).Append(" = ").Append(columns[key]);
            }
            return sb.ToString();
        }

        protected string QueryOrderBy(string col, string type)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ORDER BY ");
            sb.Append(col).Append(" ").Append(type).Append(" ");
            return sb.ToString();
        }

        protected string QueryInsert()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO ").Append(table);
            return sb.ToString();
        }

        protected string QueryUpdate()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE ").Append(table);
            return sb.ToString();
        }

        protected string QueryInsertColumns(NameValueCollection columns)
        {
            List<string> list = NameToList(columns);

            StringBuilder sb = new StringBuilder();
            sb.Append(" (");
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]).Append(", ");
                }
            }
            sb.Append(")");
            return sb.ToString();
        }

        protected string QueryInsertValues(NameValueCollection columns)
        {
            List<string> list = ValuesToList(columns);

            StringBuilder sb = new StringBuilder();
            sb.Append(" VALUES (");
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]).Append(", ");
                }
            }
            sb.Append(")");
            return sb.ToString();
        }

        protected string QueryUpdateData(NameValueCollection columns)
        {
            List<Tuple<string, string>> list = NameValuesToList(columns);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SET ");
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i].Item1).Append("=").Append(list[i].Item2);
                }
                else
                {
                    sb.Append(list[i].Item1).Append("=").Append(list[i].Item2).Append(", ");
                }
            }
            sb.Append(" ");
            return sb.ToString();
        }

        protected string QueryGetIp()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            sb.Append("SELECT host");
            sb.Append(" FROM information_schema.processlist");
            sb.Append(" WHERE ID = connection_id()");
            sb.Append(")");
            return sb.ToString();
        }

        protected string Wrap(string name)
        {
            return string.Format("'{0}'", name);
        }
        
        private List<string> NameToList(NameValueCollection columns)
        {
            List<string> list = new List<string>();
            foreach (string key in columns)
            {
                list.Add(key);
            }
            return list;
        }

        private List<string> ValuesToList(NameValueCollection columns)
        {
            List<string> list = new List<string>();
            foreach (string key in columns)
            {
                list.Add(columns[key]);
            }
            return list;
        }

        private List<Tuple<string, string>> NameValuesToList(NameValueCollection columns)
        {
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            foreach (string key in columns)
            {
                list.Add(new Tuple<string, string>(key, columns[key]));
            }
            return list;
        }
    }
}