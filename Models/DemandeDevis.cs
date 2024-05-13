using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BTP.Models
{
    public class DemandeDevis
    {
        public string idDemande { get; set; }
        public string idUser { get; set; }
        public DateTime dateDebut { get; set; }
        public DateTime dateFin { get; set; }
        public string idMaison { get; set; }
        public string idFinition { get; set; }

        public DemandeDevis() { }

        public DemandeDevis(string idUser, DateTime dateDebut, DateTime dateFin, string idMaison, string idFinition)
        {
            this.idUser = idUser;
            this.dateDebut = dateDebut;
            this.dateFin = dateFin;
            this.idMaison = idMaison;
            this.idFinition = idFinition;
        }

        public DateTime add(Connexion connexion, DateTime dateDebut, string idMaison)
        {
            DateTime val = new();
            Maison m = new Maison();
            int durre = m.durreIdMaison(connexion, idMaison);
            val = dateDebut.AddDays(durre);
            return val;
        }

        public bool Insert(Connexion connexion)
        {
            try
            {
                string query = "INSERT INTO demandeDevis (idUser, dateDebut, dateFin, idMaison, idFinition) VALUES (@idUser, @dateDebut, @dateFin, @idMaison, @idFinition)";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                command.Parameters.AddWithValue("@idUser", idUser);
                command.Parameters.AddWithValue("@dateDebut", dateDebut);
                command.Parameters.AddWithValue("@dateFin", dateFin);
                command.Parameters.AddWithValue("@idMaison", idMaison);
                command.Parameters.AddWithValue("@idFinition", idFinition);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return false;
            }
        }
    }
}
