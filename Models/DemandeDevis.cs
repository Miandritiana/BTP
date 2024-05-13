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
        public string typeMaison  {get; set; }
        public string idFinition { get; set; }
        public string finition { get; set; }
        public double pourcent { get; set; }
        public string idDevis { get; set; }
        public double montantTotal { get; set; }
        public double reste { get; set; }
        public string etat { get; set; }

        public DemandeDevis() { }

        public DemandeDevis(string idUser, DateTime dateDebut, DateTime dateFin, string idMaison, string idFinition)
        {
            this.idUser = idUser;
            this.dateDebut = dateDebut;
            this.dateFin = dateFin;
            this.idMaison = idMaison;
            this.idFinition = idFinition;
        }

        public DemandeDevis(string idUser, string idMaison, string typeMaison, string idFinition, string finition, double pourcent, DateTime dateDebut, DateTime dateFin, string idDevis, double montantTotal, double reste, string etat)
        {
            this.idUser = idUser;
            this.idMaison = idMaison;
            this.typeMaison = typeMaison;
            this.idFinition = idFinition;
            this.finition = finition;
            this.pourcent = pourcent;
            this.dateDebut = dateDebut;
            this.dateFin = dateFin;
            this.idDevis = idDevis;
            this.montantTotal = montantTotal;
            this.reste = reste;
            this.etat = etat;
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

        public List<DemandeDevis> findAll(Connexion connexion, string idUser)
        {
            List<DemandeDevis> demandeDevisList = new List<DemandeDevis>();
            try
            {
                string query = "SELECT * FROM v_detailDemandeDevis_montant_reste_etat where idUser ='"+idUser+"'";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    demandeDevisList.Add(new DemandeDevis(
                        dataReader["idUser"].ToString(),
                        dataReader["idMaison"].ToString(),
                        dataReader["type"].ToString(),
                        dataReader["idFinition"].ToString(),
                        dataReader["designation"].ToString(),
                        dataReader.GetDouble(dataReader.GetOrdinal("pourcent")),
                        DateTime.Parse(dataReader["dateDebut"].ToString()),
                        DateTime.Parse(dataReader["dateFin"].ToString()),
                        dataReader["idDevis"].ToString(),
                        dataReader.GetDouble(dataReader.GetOrdinal("montantTotal")),
                        dataReader.GetDouble(dataReader.GetOrdinal("reste")),
                        dataReader["etatDePaiement"].ToString()
                    ));
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            return demandeDevisList;
        }

        public string lastId(Connexion connexion)
        {
            string idDemande = "";
            try
            {
                string query = "SELECT TOP 1 idDemande FROM demandeDevis ORDER BY idDemande DESC";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    idDemande = dataReader.GetString(0);
                    dataReader.Close();
                    return idDemande;
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
            return idDemande;
        }
    }
}
