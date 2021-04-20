using MySql.Data.MySqlClient;
using System.Data;

namespace SGP.Util
{
    public class DAL
    {
        private static string server = "localhost";
        private static string database = "sgp";
        private static string user = "root";
        private static string password = "";
        private static string connectionString = $"Server={server};Database={database};Uid={user};Pwd={password}; convert zero datetime=True";
        private MySqlConnection connection;

        //Executa conexão
        public DAL()
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        //Executa SELECT
        public DataTable RetDataTable(string sql)
        {
            var dataTable = new DataTable();
            var command = new MySqlCommand(sql, connection);
            var da = new MySqlDataAdapter(command);
            da.Fill(dataTable);

            return dataTable;
        }

        //Eecuta INSERT, UPDATE e DELETE
        public void ExecutarComandoSQL(string sql)
        {
            var command = new MySqlCommand(sql, connection);
            command.ExecuteNonQuery();
        }       
    }
}
