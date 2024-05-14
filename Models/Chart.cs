using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BTP.Models
{
    public class Chart
    {
        public double montant { get; set; }
        public int month { get; set; }

        public Chart(){}
        public Chart(int month, double montant)
        {
            this.month = month;
            this.montant = montant;
        }
        public List<Chart> chart(Connexion connexion, int year)
        {
            List<Chart> ChartList = new List<Chart>();
            try
            {
                string query = "SELECT DATEPART(MONTH, daty) AS Month, SUM(montantTotal) AS Montant FROM  v_detailDemandeDevis_montant_reste_etat WHERE  YEAR(daty) = "+year+" GROUP BY  DATEPART(MONTH, daty) ORDER BY Month";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    ChartList.Add(new Chart(
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
            return ChartList;
        }
    }
}