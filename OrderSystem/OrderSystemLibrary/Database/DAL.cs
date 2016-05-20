using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystemLibrary.Data;

namespace OrderSystemLibrary.Database
{
    /// <summary>
    /// DAL class to connect to the database.
    /// </summary>
    public class DAL
    {
        private static DAL instance;
        private MySqlConnection connection;

        private DAL()
        {
            connection = new MySqlConnection(Configuration.Instance.Database);
        }

        /// <summary>
        /// Returns the connection to the database
        /// </summary>
        public MySqlConnection Connection
        {
            get { return connection; }
            private set { connection = value; }
        }

        public static DAL Instance
        {
            get
            {
                if (instance == null) instance = new DAL();
                return instance;
            }
        }
    }
}