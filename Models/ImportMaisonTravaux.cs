using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

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
                    List<string> processedValues = new List<string>();
                    StringBuilder currentValue = new StringBuilder();
                    bool insideQuotes = false;

                    foreach (char c in csvContent[i])
                    {
                        if (c == '"' && !insideQuotes)
                        {
                            insideQuotes = true;
                        }
                        else if (c == '"' && insideQuotes)
                        {
                            insideQuotes = false;
                        }
                        else if (c == ',' && !insideQuotes)
                        {
                            processedValues.Add(currentValue.ToString());
                            currentValue.Clear();
                        }
                        else
                        {
                            currentValue.Append(c);
                        }
                    }

                    processedValues.Add(currentValue.ToString());

                    for (int j = 0; j < processedValues.Count; j++)
                    {
                        if (double.TryParse(processedValues[j], out double result))
                        {
                            processedValues[j] = processedValues[j].Replace(',', '.');
                        }
                    }
                    // Create an instance of ImportMaisonTravaux using the processed values
                    ImportMaisonTravaux impo = new ImportMaisonTravaux(
                        processedValues[0],  // type_maison
                        processedValues[1],  // description
                        double.Parse(processedValues[2]),  // surface
                        processedValues[3],  // code_travaux
                        processedValues[4],  // type_travaux
                        processedValues[5],  // unite
                        double.Parse(processedValues[6]),  // prix_unitaire
                        double.Parse(processedValues[7]),  // quantite
                        int.Parse(processedValues[8])  // duree_travaux
                    );
                    impo.Insert(coco, impo);

                    Console.WriteLine(impo.type_maison);
                    Console.WriteLine(impo.description);
                    Console.WriteLine(impo.surface);
                    Console.WriteLine(impo.code_travaux);
                    Console.WriteLine(impo.type_travaux);
                    Console.WriteLine(impo.unite);
                    Console.WriteLine(impo.prix_unitaire);
                    Console.WriteLine(impo.quantite);

                Console.WriteLine("\n");

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