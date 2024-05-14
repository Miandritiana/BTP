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

        public static List<string> allTables(Connexion connexion)
        {
            List<string> val = new();
            var cmd = new SqlCommand("SELECT name FROM sysobjects WHERE xtype='U'", connexion.connection);
            var reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                val.Add(reader.GetString(0));
            }

            reader.Close();
            return val;
        }
        public static void ResetDatabase(Connexion connexion)
        {
            try
            {
                // Disable all foreign key constraints
                DisableAllForeignKeys(connexion);

                // Truncate tables (except 'uuser')
                List<string> tables = Connexion.allTables(connexion);
                foreach (var item in tables)
                {
                    if (item != "uuser")
                    {
                        var cmd = new SqlCommand($"TRUNCATE TABLE {item}", connexion.connection);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Re-enable foreign key constraints
                EnableAllForeignKeys(connexion);
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                Console.WriteLine($"Error: {ex}");
            }
        }

        // Helper method to disable all foreign key constraints
        private static void DisableAllForeignKeys(Connexion connexion)
        {
            var disableFkCmd = new SqlCommand("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'", connexion.connection);
            disableFkCmd.ExecuteNonQuery();
        }

        // Helper method to enable all foreign key constraints
        private static void EnableAllForeignKeys(Connexion connexion)
        {
            var enableFkCmd = new SqlCommand("EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'", connexion.connection);
            enableFkCmd.ExecuteNonQuery();
        }
    }
}
