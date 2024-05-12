using System;
using System.Data.SqlClient;

namespace BTP.Models
{
    public class Travaux
    {
        public string idTrav { get; set; }
        public int id { get; set; }
        public string num { get; set; }
        public string designation { get; set; }

        public Travaux() { }

        public Travaux(string idTrav, int id, string num, string designation)
        {
            this.idTrav = idTrav;
            this.id = id;
            this.num = num;
            this.designation = designation;
        }

        public List<Travaux> findAll(Connexion connexion)
        {
            List<Travaux> travauxList = new List<Travaux>();
            try
            {
                string query = "SELECT * FROM travaux";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    travauxList.Add(new Travaux(
                        dataReader.GetString(0),
                        (int)dataReader.GetInt32(1),
                        dataReader.GetString(2),
                        dataReader.GetString(3)
                    ));
                }

                dataReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            return travauxList;
        }

        public void create(Connexion connexion, Travaux nouveauTrav)
        {
            try
            {
                string query = "INSERT INTO travaux (id, num, designation) VALUES (@id, @num, @designation)";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                command.Parameters.AddWithValue("@id", nouveauTrav.id);
                command.Parameters.AddWithValue("@num", nouveauTrav.num);
                command.Parameters.AddWithValue("@designation", nouveauTrav.designation);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }

        public void update(Connexion connexion, Travaux updatedTrav)
        {
            try
            {
                string query = "UPDATE travaux SET num = @num, designation = @designation WHERE id = @id";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                command.Parameters.AddWithValue("@id", updatedTrav.id);
                command.Parameters.AddWithValue("@num", updatedTrav.num);
                command.Parameters.AddWithValue("@designation", updatedTrav.designation);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }

        public void delete(Connexion connexion, string idTravToDelete)
        {
            try
            {
                string query = "DELETE FROM travaux WHERE idTrav = @idTravToDelete";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                command.Parameters.AddWithValue("@idTravToDelete", idTravToDelete);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }
    }
}

