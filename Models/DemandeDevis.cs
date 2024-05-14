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

        public string travaux { get; set; }
        public string designation { get; set; }
        public double quantite { get; set; }
        public double pu { get; set; }
        public double montant { get; set; }
        public DateTime daty { get; set; }
        public int month { get; set; }

        public DemandeDevis() { }

        public DemandeDevis(string idUser, DateTime dateDebut, DateTime dateFin, string idMaison, string idFinition)
        {
            this.idUser = idUser;
            this.dateDebut = dateDebut;
            this.dateFin = dateFin;
            this.idMaison = idMaison;
            this.idFinition = idFinition;
        }

        public DemandeDevis(string idUser, string idDemande, string idMaison, string typeMaison, string idFinition, string finition, double pourcent, DateTime dateDebut, DateTime dateFin, string idDevis, double montantTotal, double reste, string etat)
        {
            this.idUser = idUser;
            this.idDemande = idDemande;
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

        public DemandeDevis(double montantTotal, double reste, string idDemande)
        {
            this.montantTotal = montantTotal;
            this.reste = reste;
            this.idDemande = idDemande;
        }

        public DemandeDevis(string idDevis, string travaux, string designation, double quantite, double pu, double montant)
        {
            this.idDevis = idDevis;
            this.travaux = travaux;
            this.designation = designation;
            this.quantite = quantite;
            this.pu = pu;
            this.montant = montant;
        }

        public DemandeDevis(int month, double montant)
        {
            this.month = month;
            this.montant = montant;
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
                        dataReader["idDemande"].ToString(),
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

        public List<DemandeDevis> findAllnoUser(Connexion connexion)
        {
            List<DemandeDevis> demandeDevisList = new List<DemandeDevis>();
            try
            {
                string query = "SELECT * FROM v_detailDemandeDevis_montant_reste_etat";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    demandeDevisList.Add(new DemandeDevis(
                        dataReader["idUser"].ToString(),
                        dataReader["idDemande"].ToString(),
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

        public DemandeDevis infoPaye(Connexion connexion, string idDemande)
        {
            DemandeDevis val = new DemandeDevis();
            try
            {
                string query = "SELECT montantTotal, reste, idDemande FROM v_detailDemandeDevis_montant_reste_etat where idDemande ='"+idDemande+"'";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    val = new DemandeDevis(
                        dataReader.GetDouble(dataReader.GetOrdinal("montantTotal")),
                        dataReader.GetDouble(dataReader.GetOrdinal("reste")),
                        dataReader["idDemande"].ToString()
                    );
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            return val;
        }

        public List<DemandeDevis> detailDevis(Connexion connexion, string idDevis)
        {
            List<DemandeDevis> demandeDevisList = new List<DemandeDevis>();
            try
            {
                string query = "select * from v_detail_devis where idDevis ='"+idDevis+"'";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    demandeDevisList.Add(new DemandeDevis(
                        dataReader["idDevis"].ToString(),
                        dataReader["travaux"].ToString(),
                        dataReader["designation"].ToString(),
                        dataReader.GetDouble(dataReader.GetOrdinal("quantite")),
                        dataReader.GetDouble(dataReader.GetOrdinal("pu")),
                        dataReader.GetDouble(dataReader.GetOrdinal("montant"))
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

        public double montantTotalEnCours(Connexion connexion)
        {
            double val = 0;
            try
            {
                string query = "SELECT sum(montantTotal) as total FROM v_detailDemandeDevis_montant_reste_etat where reste != 0 and idDemande not in (select idDemande from effectue)";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    val = dataReader.GetDouble(dataReader.GetOrdinal("total"));
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            return val;
        }

        public double montantDejaEffectue(Connexion connexion)
        {
            double val = 0;
            try
            {
                string query = "SELECT sum(montantTotal) as total FROM v_detailDemandeDevis_montant_reste_etat where reste = 0 and idDemande in (select idDemande from effectue)";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    val = dataReader["total"] != DBNull.Value ? dataReader.GetDouble(dataReader.GetOrdinal("total")) : 0;
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            return val;
        }

        public double montantTotalDesDevis(Connexion connexion)
        {
            double val = 0;
            try
            {
                string query = "select sum(montantTotal) as total from v_detailDemandeDevis_montant_reste_etat";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    val = dataReader["total"] != DBNull.Value ? dataReader.GetDouble(dataReader.GetOrdinal("total")) : 0;
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            return val;
        }

        public List<DemandeDevis> chart(Connexion connexion, int year)
        {
            List<DemandeDevis> demandeDevisList = new List<DemandeDevis>();
            try
            {
                string query = "SELECT DATEPART(MONTH, daty) AS Month, SUM(montantTotal) AS Montant FROM  v_detailDemandeDevis_montant_reste_etat WHERE  YEAR(daty) = "+year+" GROUP BY  DATEPART(MONTH, daty) ORDER BY Month";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    demandeDevisList.Add(new DemandeDevis(
                        (int)dataReader["Month"],
                        dataReader.GetDouble(dataReader.GetOrdinal("Montant"))
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

        public List<int> listYear(Connexion connexion)
        {
            List<int> intList = new List<int>();
            try
            {
                string query = "SELECT YEAR(daty) as YEAR FROM v_detailDemandeDevis_montant_reste_etat group by year(daty)";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    intList.Add((int)dataReader["YEAR"]);
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            return intList;
        }

    }
}
