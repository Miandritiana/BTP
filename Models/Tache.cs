using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BTP.Models
{
    public class Tache
    {
        public string idTache { get; set; }
        public int id { get; set; }
        public string num { get; set; }
        public string designation { get; set; }
        public string unite { get; set; }
        public float pu { get; set; }
        public string idTrav { get; set; }

        public Tache() { }

        public Tache(string idTache, int id, string num, string designation, string unite, float pu, string idTrav)
        {
            this.idTache = idTache;
            this.id = id;
            this.num = num;
            this.designation = designation;
            this.unite = unite;
            this.pu = pu;
            this.idTrav = idTrav;
        }

        public List<Tache> findAll(Connexion connexion)
        {
            List<Tache> tacheList = new List<Tache>();
            try
            {
                string query = "SELECT * FROM tache";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    tacheList.Add(new Tache(
                        dataReader.GetString(0),
                        (int)dataReader.GetInt32(1),
                        dataReader.GetString(2),
                        dataReader.GetString(3),
                        dataReader.GetString(4),
                        (float)dataReader.GetDouble(5),
                        dataReader.GetString(6)
                    ));
                }

                dataReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            return tacheList;
        }
        public void create(Connexion connexion, Tache nouveauTache)
        {
            try
            {
                string query = "INSERT INTO tache (id, num, designation, unite, pu, idTrav) VALUES (@id, @num, @designation, @unite, @pu, @idTrav)";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                command.Parameters.AddWithValue("@id", nouveauTache.id);
                command.Parameters.AddWithValue("@num", nouveauTache.num);
                command.Parameters.AddWithValue("@designation", nouveauTache.designation);
                command.Parameters.AddWithValue("@unite", nouveauTache.unite);
                command.Parameters.AddWithValue("@pu", nouveauTache.pu);
                command.Parameters.AddWithValue("@idTrav", nouveauTache.idTrav);
                var result = command.ExecuteNonQuery();
                if (result == 0)
                {
                    throw new Exception("Erreur lors de la creation de la tache");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void update(Connexion connexion, Tache updatedTache)
        {
            try
            {
                string query = "UPDATE tache SET id = @id, num = @num, designation = @designation, unite = @unite, pu = @pu, idTrav = @idTrav WHERE idTache = @idTache";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                command.Parameters.AddWithValue("@idTache", updatedTache.idTache);
                command.Parameters.AddWithValue("@id", updatedTache.id);
                command.Parameters.AddWithValue("@num", updatedTache.num);
                command.Parameters.AddWithValue("@designation", updatedTache.designation);
                command.Parameters.AddWithValue("@unite", updatedTache.unite);
                command.Parameters.AddWithValue("@pu", updatedTache.pu);
                command.Parameters.AddWithValue("@idTrav", updatedTache.idTrav);
                var result = command.ExecuteNonQuery();
                if (result == 0)
                {
                    throw new Exception("Erreur lors de la mise a jour de la tache");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void delete (Connexion connexion, string idTacheToDelete)
        {
            try
            {
                string query = "DELETE FROM tache WHERE idTache = @idTacheToDelete";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                command.Parameters.AddWithValue("@idTacheToDelete", idTacheToDelete);
                var result = command.ExecuteNonQuery();
                if (result == 0)
                {
                    throw new Exception("Erreur lors de la suppression de la tache");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


