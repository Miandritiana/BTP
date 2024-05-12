using System.Data.SqlClient;

namespace BTP.Models 
{
    public class Connexion 
    {
        public SqlConnection connection;
        public Connexion()
        {
            string connectionString = "Server=(Localdb)\\MSSQLLocalDB;Database=btp;Trusted_Connection=True;";
            connection = new SqlConnection(connectionString);
        }
        public SqlConnection connexion 
        {
            get; set;
        }
        public void ResetDatabase()
        {
            connection.Open();
            using (var cmd = new SqlCommand("SELECT name FROM sysobjects WHERE xtype='U'", connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string tableName = reader.GetString(0);
                        if (tableName != "Uuser")
                        {
                            using (var cmd2 = new SqlCommand($"TRUNCATE TABLE {tableName}", connection))
                            {
                                cmd2.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            connection.Close();
        }

    }
}
