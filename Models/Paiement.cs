using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BTP.Models
{
    public class Paiement
    {
        public string idPaye { get; set; }
        public string idDemande { get; set; }

        public DateTime datePaye { get; set; }
        public double paye { get; set; }

        public Paiement() { }

        public Paiement(string idDemande)
        {
            this.idDemande = idDemande;
        }

        public void insert(Connexion connexion)
        {
            string query = "INSERT INTO paiement (idDemande) VALUES (@idDemande)";
            SqlCommand command = new SqlCommand(query, connexion.connection);
            command.Parameters.AddWithValue("@idDemande", this.idDemande);
            command.ExecuteNonQuery();
        }

        public string lastId(Connexion connexion)
        {
            string idPaye = "";
            try
            {
                string query = "SELECT TOP 1 idPaye FROM paiement ORDER BY idPaye DESC";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    idPaye = dataReader.GetString(0);
                    dataReader.Close();
                    return idPaye;
                }
                else
                {
                    dataReader.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            return idPaye;

        }
    }
}
