using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Database
{
    public class DAL
    {
        private static DAL instance;
        private MySqlConnection connection;

        // Init

        static DAL()
        {
        }

        private DAL()
        {
            connection = new MySqlConnection(Configuration.Instance.Database);
        }

        // Properties

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