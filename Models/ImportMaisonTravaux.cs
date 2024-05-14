using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Data.SqlClient;

namespace BTP.Models
{
    public class ImportMaisonTravaux
    {
        public string type_maison { get; set; }
        public string description { get; set; }
        public double surface { get; set; }
        public string code_travaux { get; set; }
        public string type_travaux { get; set; }
        public string unite { get; set; }
        public double prix_unitaire { get; set; }
        public double quantite { get; set; }
        public int duree_travaux { get; set; }

        public ImportMaisonTravaux() { }

        public ImportMaisonTravaux(string type_maison, string description, double surface, string code_travaux, string type_travaux, string unite, double prix_unitaire, double quantite, int duree_travaux)
        {
            this.type_maison = type_maison;
            this.description = description;
            this.surface = surface;
            this.code_travaux = code_travaux;
            this.type_travaux = type_travaux;
            this.unite = unite;
            this.prix_unitaire = prix_unitaire;
            this.quantite = quantite;
            this.duree_travaux = duree_travaux;
        }

        public void Insert(Connexion coco, ImportMaisonTravaux impo)
        {
            try
            {
                Tache tache = new();
                tache.create(coco, new Tache(impo.code_travaux, impo.type_travaux, impo.unite, (float)impo.prix_unitaire));

                Maison maison = new();
                maison.create(coco, new Maison(impo.type_maison, (int)impo.duree_travaux));
                string lastIdTypeM = maison.lastId(coco);

                maison.createMaison(coco, new Maison(lastIdTypeM, impo.description, impo.surface));

                Devis devis = new();
                devis.create(coco, new Devis(lastIdTypeM));

                devis.createDetailDevis(coco, new Devis(devis.lastIdDevis(coco), tache.lastId(coco), impo.quantite));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string import(IFormFile csvFile, Connexion coco)
        {
            try
            {
                var csvContent = new List<string>();
                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                {
                    // bool isFirstLine = true;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            // if (!isFirstLine)
                            // {
                                csvContent.Add(line);
                            // }
                        }
                    }
                }
                for (int i = 1; i < csvContent.Count; i++)
                {
                    List<string> processedValues = ParseCsvLine(csvContent[i]);

                    if (processedValues.Count >= 9)
                    {
                        ImportMaisonTravaux impo = new ImportMaisonTravaux(
                            processedValues[0],
                            processedValues[1],
                            double.Parse(processedValues[2]),
                            processedValues[3],
                            processedValues[4],
                            processedValues[5],
                            double.Parse(processedValues[6]),
                            double.Parse(processedValues[7]),
                            int.Parse(processedValues[8])
                        );

                        Console.WriteLine(impo.quantite);
                        Console.WriteLine(impo.prix_unitaire);
                        // impo.Insert(coco, impo);
                    }
                    else
                    {
                        Console.WriteLine("Error: Insufficient data in CSV line");
                    }

                    Console.WriteLine("\n");
                }

                // Function to parse a CSV line into processed values
                List<string> ParseCsvLine(string line)
                {
                    List<string> processedValues = new List<string>();
                    StringBuilder currentValue = new StringBuilder();
                    bool insideQuotes = false;

                    foreach (char c in line)
                    {
                        if (c == '"' && !insideQuotes)
                        {
                            insideQuotes = true; // Entering a quoted section
                        }
                        else if (c == '"' && insideQuotes)
                        {
                            insideQuotes = false; // Exiting a quoted section
                        }
                        else if (c == ',' && !insideQuotes)
                        {
                            double parsedValue;
                            if (double.TryParse(currentValue.ToString().Replace(',', '.'), out parsedValue))
                            {
                                processedValues.Add(parsedValue.ToString()); // Add the completed value as string
                            }
                            else
                            {
                                // Handle parsing error or use a default value
                                processedValues.Add("0"); // Default value example
                            }
                            currentValue.Clear(); // Clear StringBuilder for the next value
                        }
                        else
                        {
                            currentValue.Append(c); // Append characters to the current value
                        }
                    }

                    processedValues.Add(currentValue.ToString());

                    return processedValues;
                }


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