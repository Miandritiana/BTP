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

        public string description { get; set; }
        public double surface { get; set; }

        public int durre { get; set; }

        public Maison() { }

        public Maison(string idDevis, double montantTotal, string idMaison, string idType, string type, int nbrChambre, int nbrToilet, int nbrCuisine, int nbrLiving, string description, double surface)
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
            this.description = description;
            this.surface = surface;
        }

        public Maison(string type, int durre)
        {
            this.type = type;
            this.durre = durre;
        }

        public Maison(string idType, string description, double surface)
        {
            this.idType = idType;
            this.description = description;
            this.surface = surface;
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
                        (int)dataReader["nbrLiving"],
                        dataReader["description"].ToString(),
                        dataReader.GetDouble(dataReader.GetOrdinal("surface"))
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

        public void create(Connexion connexion, Maison maison)
        {
            try
            {
                // string query = "INSERT INTO typeMaison (designation, durre) VALUES (@designation, @durre)";
                string query = "INSERT INTO typeMaison (designation, durre) SELECT '"+maison.type+"', "+maison.durre+" WHERE NOT EXISTS (SELECT 1 FROM typeMaison WHERE designation = '"+maison.type+"' AND durre = "+maison.durre+")";
                Console.WriteLine(query);
                SqlCommand command = new SqlCommand(query, connexion.connection);
                // command.Parameters.AddWithValue("@designation", maison.type);
                // command.Parameters.AddWithValue("@durre", maison.durre);
                var result = command.ExecuteNonQuery();
                // if (result == 0)
                // {
                //     throw new Exception("Erreur lors de la creation de la typemaison");
                // }
            }
            catch (Exception ex)
            {
                throw new Exception ("Erreur lors de la creation de la typemaison");
            }
        }

        public string lastId(Connexion connexion)
        {
            string idTypeMaison = null;
            try
            {
                string query = "SELECT TOP 1 idType FROM typemaison ORDER BY idType DESC";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    idTypeMaison = dataReader.GetString(0);
                    dataReader.Close();
                    return idTypeMaison;
                }
                else
                {
                    dataReader.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void createMaison(Connexion connexion, Maison maison)
        {
            try
            {
                // string query = "INSERT INTO maison (idType, description, surface) VALUES (@idType, @description, @surface)";
                string query = "INSERT INTO maison (idType, description, surface) SELECT @idType, @description, @surface WHERE NOT EXISTS (SELECT 1 FROM maison WHERE idType = @idType AND description = @description AND surface = @surface)";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                command.Parameters.AddWithValue("@idType", maison.idType);
                command.Parameters.AddWithValue("@description", maison.description);
                command.Parameters.AddWithValue("@surface", maison.surface);
                var result = command.ExecuteNonQuery();
                // if (result == 0)
                // {
                //     throw new Exception("Erreur lors de la creation de la maison");
                // }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la creation de la maison");
            }
        }

    }
}
