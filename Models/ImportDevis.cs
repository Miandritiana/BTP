using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

namespace BTP.Models
{
    public class ImportDevis
    {
        public string client { get; set; }
        public string ref_devis { get; set; }
        public string type_maison { get; set; }
        public string finition { get; set; }
        public double taux_finition { get; set; }
        public DateTime date_devis { get; set; }
        public DateTime date_debut { get; set; }
        public string lieu { get; set; }

        public ImportDevis() { }

        public ImportDevis(string client, string ref_devis, string type_maison, string finition, double taux_finition, DateTime date_devis, DateTime date_debut, string lieu)
        {
            this.client = client;
            this.ref_devis = ref_devis;
            this.type_maison = type_maison;
            this.finition = finition;
            this.taux_finition = taux_finition;
            this.date_devis = date_devis;
            this.date_debut = date_debut;
            this.lieu = lieu;
        }

        public void Insert(Connexion coco, ImportDevis impo)
        {
            try
            {
                Console.WriteLine("dafsghdfdghtyjk");

                Uuser user = new();
                user.createClient(coco, new Uuser(impo.client));
                string idUser = user.lastId(coco);

                Devis devis = new();
                string devisID = devis.devisID(coco, impo.ref_devis);
                Console.WriteLine("sdfzghnm,"+devisID);

                Maison maison = new();
                string idMaison = maison.getIdMaison(coco, impo.type_maison, devisID);
                int durre = maison.durreIdMaison(coco, idMaison);
                DateTime dateFin = impo.date_debut.AddDays(durre);

                Finition finition = new();
                string idFinition = finition.getIdFinition(coco, impo.finition);
                finition.updatePourcent(coco, idFinition, impo.taux_finition);
                                                                // (@idUser, @dateDebut, @dateFin, @idMaison, @idFinition, @daty, @lieu)

                DemandeDevis demande = new();
                DemandeDevis insert = new DemandeDevis(idUser, impo.date_debut, dateFin, idMaison, idFinition, impo.date_devis, impo.lieu);
                demande.insertDemandeCSV(coco, insert);
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

                    // Change the format of processedValues[5] and processedValues[6] when parsing DateTime
                    DateTime date5;
                    DateTime date6;

                    // if (DateTime.TryParseExact(processedValues[5], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date5) &&
                    //     DateTime.TryParseExact(processedValues[6], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date6))
                    // {
                        ImportDevis impo = new ImportDevis(
                            processedValues[0],
                            processedValues[1],
                            processedValues[2],
                            processedValues[3],
                            double.Parse(processedValues[4]),
                            DateTime.Parse(processedValues[5]),
                            DateTime.Parse(processedValues[6]),
                            // DateTime.ParseExact(date5.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                            // DateTime.ParseExact(date6.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                            processedValues[7]
                        );

                        Console.WriteLine(impo.date_devis);
                        Console.WriteLine(impo.date_debut);
                                            Console.WriteLine(impo.client);
                    Console.WriteLine(impo.ref_devis);
                    Console.WriteLine(impo.type_maison);
                    Console.WriteLine(impo.finition);
                    Console.WriteLine(impo.taux_finition);
                    Console.WriteLine(impo.date_devis);
                    Console.WriteLine(impo.date_debut);
                    Console.WriteLine(impo.lieu);
                    
                    impo.Insert(coco, impo);

                    // }


                    

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