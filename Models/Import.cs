using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Data.SqlClient;

namespace BTP.Models
{
    public class Import
    {
        public string ImportFunction(IFormFile csvFile)
        {
            try
            {
                var csvContent = new List<string>();
                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            csvContent.Add(line);
                        }
                    }
                }

                if (csvContent.Count < 2)
                {
                    return "CSV file is empty or does not contain valid data.";
                }

                var columns = csvContent[0].Split(",");
                var dataTypes = new List<string>();
                foreach (var column in columns)
                {
                    dataTypes.Add("NVARCHAR(MAX)");
                }

                var createTableQuery = $"CREATE TABLE ImportedData ({string.Join(",", columns.Zip(dataTypes, (col, type) => $"{col} {type}"))})";

                Connexion coco = new Connexion();
                coco.connection.Open();

                using (var commandTable = new SqlCommand(createTableQuery, coco.connection))
                {
                    commandTable.ExecuteNonQuery();
                }

                List<string> insertDataQuerys = new List<string>();
                for (int i = 1; i < csvContent.Count; i++)
                {
                    var dataRow = csvContent[i].Split(",");
                    var insertQuery = $"INSERT INTO ImportedData ({string.Join(",", columns)}) VALUES ('{string.Join("','", dataRow)}')";
                    insertDataQuerys.Add(insertQuery);
                }

                var command = new SqlCommand();
                command.Connection = coco.connection;
                foreach (var insert in insertDataQuerys)
                {
                    command.CommandText = insert;
                    command.ExecuteNonQuery();
                }

                coco.connection.Close();
                return "CSV file uploaded and data imported into the database.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return "Error importing CSV file: " + ex.Message;
            }
        }
    }
}

