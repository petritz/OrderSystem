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
    }
}