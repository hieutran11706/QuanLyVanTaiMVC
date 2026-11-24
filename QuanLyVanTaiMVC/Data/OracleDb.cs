using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data;

namespace QuanLyVanTaiMVC.Data
{
    public static class OracleDb
    {
        private static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["OracleDb"].ConnectionString; }
        }

        public static OracleConnection CreateConnection()
        {
            return new OracleConnection(ConnectionString);
        }

        public static DataTable Query(string sql, params OracleParameter[] parameters)
        {
            using (var connection = CreateConnection())
            using (var command = new OracleCommand(sql, connection))
            using (var adapter = new OracleDataAdapter(command))
            {
                command.BindByName = true;
                if (parameters != null) command.Parameters.AddRange(parameters);
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        public static int Execute(string sql, params OracleParameter[] parameters)
        {
            using (var connection = CreateConnection())
            using (var command = new OracleCommand(sql, connection))
            {
                command.BindByName = true;
                if (parameters != null) command.Parameters.AddRange(parameters);
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }
}
