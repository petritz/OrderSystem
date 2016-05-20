using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystemLibrary.Data;

namespace OrderSystemLibrary.Database
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
                Logger.I("Database Executing Query: " + query);
                MySqlDataReader reader = command.ExecuteReader();

                table.Load(reader);
                return table;
            }
            catch (MySqlException ex)
            {
                Logger.W(ex);
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
            return UpdateRows(query) == 1;
        }

        /// <summary>
        /// Runs a insert/update/delete statement in the database
        /// </summary>
        /// <param name="query">The query to run</param>
        /// <returns>The number of rows that were manipulated.</returns>
        public int UpdateRows(string query)
        {
            MySqlCommand command = new MySqlCommand(query, dal.Connection);

            try
            {
                dal.Connection.Open();
                Logger.I("Database Executing Update: " + query);
                return command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Logger.W(ex);
            }
            finally
            {
                dal.Connection.Close();
            }

            return 0;
        }
    }
}