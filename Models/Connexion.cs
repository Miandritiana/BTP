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
            List<string> tables = Connexion.allTables(connexion);
            foreach (var item in tables)
            {
                if (item != "uuser")
                {
                    var cmd = new SqlCommand ($"truncate table {item}", connexion.connection);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
