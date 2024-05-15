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
        public double puDouble { get; set; }

        // public string code_travaux { get; set; }
        // public string type_travaux { get; set; }

        public Tache() { }

        public Tache(string idTache, string num, string designation, string unite, double puDouble)
        {
            this.idTache = idTache;
            this.num = num;
            this.designation = designation;
            this.unite = unite;
            this.puDouble = puDouble;
        }

        public Tache(string num, string designation, string unite, float pu)
        {
            this.num = num;
            this.designation = designation;
            this.unite = unite;
            this.pu = pu;
        }

        // public List<Tache> findAll(Connexion connexion)
        // {
        //     List<Tache> tacheList = new List<Tache>();
        //     try
        //     {
        //         string query = "SELECT * FROM tache";
        //         SqlCommand command = new SqlCommand(query, connexion.connection);
        //         SqlDataReader dataReader = command.ExecuteReader();
        //         while (dataReader.Read())
        //         {
        //             tacheList.Add(new Tache(
        //                 dataReader.GetString(0),
        //                 (int)dataReader.GetInt32(1),
        //                 dataReader.GetString(2),
        //                 dataReader.GetString(3),
        //                 dataReader.GetString(4),
        //                 (float)dataReader.GetDouble(5),
        //                 dataReader.GetString(6),
        //                 dataReader.GetString(7),
        //                 dataReader.GetString(8)
        //             ));
        //         }

        //         dataReader.Close();
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Error: {ex}");
        //     }
        //     return tacheList;
        // }
        public void create(Connexion connexion, Tache nouveauTache)
        {
            try
            {
                string query = "INSERT INTO tache (num, designation, unite, pu) VALUES (@num, @designation, @unite, @pu)";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                command.Parameters.AddWithValue("@num", nouveauTache.num);
                command.Parameters.AddWithValue("@designation", nouveauTache.designation);
                command.Parameters.AddWithValue("@unite", nouveauTache.unite);
                command.Parameters.AddWithValue("@pu", nouveauTache.pu);
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

        public string lastId(Connexion connexion)
        {
            string idTache = null;
            try
            {
                string query = "SELECT TOP 1 idTache FROM tache ORDER BY idTache DESC";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    idTache = dataReader.GetString(0);
                    dataReader.Close();
                    return idTache;
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

        public void update(Connexion connexion, Tache updatedTache)
        {
            try
            {
                string query = "UPDATE tache SET num = @num, designation = @designation, unite = @unite, pu = @pu WHERE idTache = @idTache";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                command.Parameters.AddWithValue("@num", updatedTache.num);
                command.Parameters.AddWithValue("@designation", updatedTache.designation);
                command.Parameters.AddWithValue("@unite", updatedTache.unite);
                command.Parameters.AddWithValue("@pu", updatedTache.pu);
                command.Parameters.AddWithValue("@idTache", updatedTache.idTache);
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

        public List<Tache> get(Connexion connexion)
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
                        dataReader["idTache"].ToString(),
                        dataReader["num"].ToString(),
                        dataReader["designation"].ToString(),
                        dataReader["unite"].ToString(),
                        dataReader.GetDouble(dataReader.GetOrdinal("pu"))
                    ));
                }
                dataReader.Close();
            }
            catch (Exception ex)    
            {
                throw ex;
            }
            return tacheList;
        }

        public Tache getById(Connexion connexion, string idTache)
        {
            Tache tache = new Tache();
            try
            {
                string query = "SELECT * FROM tache WHERE idTache = @idTache";
                SqlCommand command = new SqlCommand(query, connexion.connection);
                command.Parameters.AddWithValue("@idTache", idTache);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    tache = new Tache(
                        dataReader["idTache"].ToString(),
                        dataReader["num"].ToString(),
                        dataReader["designation"].ToString(),
                        dataReader["unite"].ToString(),
                        dataReader.GetDouble(dataReader.GetOrdinal("pu"))
                    );
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tache;
        }
    }
}


