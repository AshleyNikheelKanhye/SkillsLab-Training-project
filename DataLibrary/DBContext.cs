using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public class DBContext :IDisposable
    {
        private SqlConnection connection;

        public DBContext(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            connection.Open();

        }

        public SqlConnection GetConn()
        {
            return connection;
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }

    }
}
