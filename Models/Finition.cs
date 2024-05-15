using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BTP.Models
{
    public class Finition
    {
        public string idFinition { get; set; }
        public string designation { get; set; }
        public double pourcent { get; set; }

        public Finition (){}
        
        public Finition (string idFinition, string designation, double pourcent)
        {
            this.idFinition = idFinition;
            this.designation = designation;
            this.pourcent = pourcent;
        }

        public List<Finition> findAll(Connexion connexion)
        {
            List<Finition> finitionList = new List<Finition>();
            try
            {
                string query = "SELECT * FROM Finition";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    finitionList.Add(new Finition(
                        dataReader.GetString(0),
                        dataReader.GetString(2),
                        dataReader.GetDouble(dataReader.GetOrdinal("pourcent"))
                    ));
                }

                dataReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            return finitionList;
        }

        public string getIdFinition(Connexion connexion, string designation)
        {
            string id = "";
            try
            {
                string query = "select idFinition from Finition where designation = '"+designation+"'";
                SqlCommand commandFinition = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReaderFini = commandFinition.ExecuteReader();
                while (dataReaderFini.Read())
                {
                    id = dataReaderFini.GetString(0);
                }
                    dataReaderFini.Close();
                    return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            return null;
        }

        public void updatePourcent(Connexion connexion, string idFinition, double pourcent)
        {
            try
            {
                string query = "UPDATE Finition SET pourcent = @pourcent WHERE idFinition = @idFinition";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                command.Parameters.AddWithValue("@pourcent", pourcent);
                command.Parameters.AddWithValue("@idFinition", idFinition);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }
    }
}