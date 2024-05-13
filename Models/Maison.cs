using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BTP.Models
{
    public class Maison
    {
        public string idMaison { get; set; }
        public int id { get; set; }
        public string idType { get; set; }
        public string type { get; set; }
        public int nbrChambre { get; set; }
        public int nbrToilet { get; set; }
        public int nbrCuisine { get; set; }
        public int nbrLiving { get; set; }

        public string idDevis { get; set; }
        public double montantTotal { get; set; }

        public Maison() { }

        public Maison(string idDevis, double montantTotal, string idMaison, string idType, string type, int nbrChambre, int nbrToilet, int nbrCuisine, int nbrLiving)
        {
            this.idDevis = idDevis;
            this.montantTotal = montantTotal;
            this.idMaison = idMaison;
            this.idType = idType;
            this.type = type;
            this.nbrChambre = nbrChambre;
            this.nbrToilet = nbrToilet;
            this.nbrCuisine = nbrCuisine;
            this.nbrLiving = nbrLiving;
        }

        public List<Maison> findAll(Connexion connexion)
        {
            List<Maison> maisonList = new List<Maison>();
            try
            {
                string query = "SELECT * FROM v_info_maison_total_devis";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    maisonList.Add(new Maison(
                        dataReader["idDevis"].ToString(),
                        dataReader.GetDouble(dataReader.GetOrdinal("montantTotal")),
                        dataReader["idMaison"].ToString(),
                        dataReader["idType"].ToString(),
                        dataReader["type"].ToString(),
                        (int)dataReader["nbrChambre"],
                        (int)dataReader["nbrToilet"],
                        (int)dataReader["nbrCuisine"],
                        (int)dataReader["nbrLiving"]
                    ));
                }

                dataReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            return maisonList;
        }

        public int durreIdMaison(Connexion connexion, string idMaison)
        {
            try
            {
                string query = "select durre from maison m join typeMaison ty on ty.idType = m.idType where idMaison = '"+idMaison+"'";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    int durre = dataReader.GetInt32(0);
                    dataReader.Close();
                    return durre;
                }
                else
                {
                    dataReader.Close();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return 0;
            }
        }
    }
}
