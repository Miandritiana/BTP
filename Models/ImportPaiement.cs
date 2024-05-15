using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace BTP.Models
{
    public class ImportPaiement
    {
        public string ref_devis { get; set; }
        public string ref_paiement { get; set; }
        public DateTime date_paiement { get; set; }
        public double montant { get; set; }

        public ImportPaiement() { }

        public ImportPaiement(string ref_devis, string ref_paiement, DateTime date_paiement, double montant)
        {
            this.ref_devis = ref_devis;
            this.ref_paiement = ref_paiement;
            this.date_paiement = date_paiement;
            this.montant = montant;
        }

        public void Insert(Connexion coco, ImportPaiement impo)
        {
            try
            {
                DemandeDevis demande = new();
                string idDemande = demande.getIdDemandeIdDevis(coco, impo.ref_devis);

                Paiement paiement = new();
                paiement.insert_with_ref(coco, new Paiement(impo.date_paiement, impo.montant, idDemande, impo.ref_paiement));

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
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            csvContent.Add(line);
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
                        if (processedValues[j].Contains("%"))
                        {
                            string cleanedString = processedValues[j].Replace("%", "").Replace(',', '.');
                            if (double.TryParse(cleanedString, out double result))
                            {
                                processedValues[j] = result.ToString();
                            }
                        }
                    }
                    ImportPaiement impo = new ImportPaiement(
                        processedValues[0],
                        processedValues[1],
                        DateTime.Parse(processedValues[2]),
                        double.Parse(processedValues[3])
                    );

                    Console.WriteLine(impo.ref_devis);
                    Console.WriteLine(impo.ref_paiement);
                    Console.WriteLine(impo.date_paiement);
                    Console.WriteLine(impo.montant);

                    this.Insert(coco, impo);

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