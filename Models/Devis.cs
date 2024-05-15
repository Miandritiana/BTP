using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BTP.Models
{
    public class Devis
    {
        public string idDevis { get; set; }
        public int id { get; set; }
        public string designation { get; set; }
        public string idTypeMaison { get; set; }
        public double quantite { get; set; }
        public string idTache { get; set; }

        public Devis() { }

        public Devis(string idTypeMaison)
        {
            this.idTypeMaison = idTypeMaison;
        }

        public Devis(string idDevis, string idTache, double quantite)
        {
            this.idDevis = idDevis;
            this.idTache = idTache;
            this.quantite = quantite;
        }

        public void create(Connexion connexion, Devis devis)
        {
            try
            {
                string query = "INSERT INTO devis (designation, idTypeMaison) " +
                            "SELECT designation, idType " +
                            "FROM typeMaison " +
                            "WHERE idType = '"+devis.idTypeMaison+"' " +
                            "AND NOT EXISTS (SELECT 1 FROM devis WHERE idTypeMaison = '"+devis.idTypeMaison+"')";
                Console.WriteLine(query);
                SqlCommand command = new SqlCommand(query, connexion.connection);
                command.ExecuteNonQuery();
            }  
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }

        public string lastIdDevis(Connexion connexion)
        {
            string idDevis = null;
            try
            {
                string query = "SELECT TOP 1 idDevis FROM devis ORDER BY idDevis DESC";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    idDevis = dataReader.GetString(0);
                    dataReader.Close();
                    return idDevis;
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
            return idDevis;
        }

        public void createDetailDevis(Connexion connexion, Devis detailDevis)
        {
            try
            {
                string query = "INSERT INTO detailDevis (idDevis, idTache, quantite, pu) VALUES ('"+detailDevis.idDevis+"', '"+detailDevis.idTache+"', "+detailDevis.quantite+", (select pu from tache where idTache = '"+detailDevis.idTache+"'))";
                Console.WriteLine(query);
                SqlCommand command = new SqlCommand(query, connexion.connection);
                // command.Parameters.AddWithValue("@idDevis", detailDevis.idDevis);
                // command.Parameters.AddWithValue("@idTache", detailDevis.idTache);
                // command.Parameters.AddWithValue("@quantite", detailDevis.quantite);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }

        public string devisID(Connexion connexion, string idDevis)
        {
            string id = "";
            try
            {
                string paddedIdDevis = idDevis.StartsWith("D") ? "D" + int.Parse(idDevis.TrimStart('D')).ToString("00") : idDevis;
                string query = "SELECT idDevis FROM devis WHERE idDevis = '"+paddedIdDevis+"'";
                Console.WriteLine(query);
                SqlCommand command = new SqlCommand(query, connexion.connection);
                
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    id = dataReader.GetString(0);
                }
                    dataReader.Close();
                    return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            return id;
        }

    }
}