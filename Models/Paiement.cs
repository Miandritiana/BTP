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

        public Paiement(string idDemande, double paye)
        {
            this.idDemande = idDemande;
            this.paye = paye;
        }

        public Paiement(DateTime datePaye, double paye, string idDemande)
        {
            this.datePaye = datePaye;
            this.paye = paye;
            this.idDemande = idDemande;
        }

        public void insert(Connexion connexion, Paiement paiement)
        {
            string query = "INSERT INTO histo (paye, idDemande) VALUES (@paye, @idDemande)";
            SqlCommand command = new SqlCommand(query, connexion.connection);
            command.Parameters.AddWithValue("@idDemande", paiement.idDemande);
            command.Parameters.AddWithValue("@paye", paiement.paye);
            command.ExecuteNonQuery();
        }

        public void insert2(Connexion connexion, Paiement paiement)
        {
            string query = "INSERT INTO histo (datePaye, paye, idDemande) VALUES (@datePaye, @paye, @idDemande)";
            SqlCommand command = new SqlCommand(query, connexion.connection);
            command.Parameters.AddWithValue("@datePaye", paiement.datePaye);
            command.Parameters.AddWithValue("@idDemande", paiement.idDemande);
            command.Parameters.AddWithValue("@paye", paiement.paye);
            command.ExecuteNonQuery();
        }
    }
}
