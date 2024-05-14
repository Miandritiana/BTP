using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BTP.Models;
using System.IO;
using Microsoft.AspNetCore.Mvc;
// using iText.Kernel.Pdf;
// using iText.Layout;
// using iText.Layout.Element;

namespace BTP.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(bool error = false)
    {
        if (error)
        {
            ViewData["error"] = TempData["error"]?.ToString();
        }
        return View();
    }

    public IActionResult ClearSession()
    {
        HttpContext.Session.Remove("sessionId");
        HttpContext.Session.Remove("adminId");
        return RedirectToAction("Index", "Home");
    }


    public IActionResult checkLog()
    {
        HttpContext.Session.Remove("adminId");

        var num = Request.Form["num"].ToString();

        Uuser uu = new Uuser();

        Connexion coco = new Connexion();
        coco.connection.Open();
        
        string idUser = uu.checkLoginNum(coco, num);
        if (idUser != null)
        {
            HttpContext.Session.SetString("sessionId", idUser);
            bool isSuperUser = uu.isAdmin(coco, idUser);

            if (isSuperUser)
            {
                coco.connection.Close();
                return RedirectToAction("Index", "Admin");
            }

            coco.connection.Close();
            return RedirectToAction("Homepage", "Home");

        }else{
            TempData["error"] = "Misy diso ny numero";
            coco.connection.Close();
            return RedirectToAction("Index", "Home", new { error = true });
        }

        coco.connection.Close();

        return View();
    }

    public IActionResult import()
    {
        HttpContext.Session.Remove("adminId");

        if(HttpContext.Session.GetString("sessionId") != null)
        {
            return View("Import");

        }else{

            return RedirectToAction("Index", "Home");
        }
    }

    public IActionResult ImportCSV(IFormFile csvFile)
    {
        if (csvFile != null && csvFile.Length > 0)
        {
            try
            {
                Import imp = new Import();
                string message = imp.ImportFunction(csvFile);
                
                if (message.Contains("Error") || message.Contains("Exception") || message.Contains("failed"))
                {
                    ViewBag.Error = message;
                }
                else
                {
                    ViewBag.Message = message;
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error processing CSV file: " + ex.Message;
            }
        }
        else
        {
            ViewBag.Error = "No file selected.";
        }

        return View("Import", ViewBag);
    }

    public IActionResult Homepage()
    {
        HttpContext.Session.Remove("adminId");

        if(HttpContext.Session.GetString("sessionId") != null)
        {
            Maison m = new Maison();
            Data data = new Data();
            
            Connexion coco = new Connexion();
            coco.connection.Open();

            data.maisonList = m.findAll(coco);

            coco.connection.Close();

            return View("Homepage", data);

        }else{

            return RedirectToAction("Index", "Home");
        }
    }

    public IActionResult Finition()
    {
        HttpContext.Session.Remove("adminId");

        if(HttpContext.Session.GetString("sessionId") != null)
        {
            var idMaison = Request.Form["idMaison"].ToString();
            if (idMaison != null)
            {
                HttpContext.Session.SetString("idMaison", idMaison);
                Finition f = new Finition();
                Data data = new Data();
                
                Connexion coco = new Connexion();
                coco.connection.Open();

                data.finiList = f.findAll(coco);

                coco.connection.Close();

                return View("Finition", data);
            }
        
            ViewData["error"] = "Tsy voray ny idMaison nosafidinao";
            return RedirectToAction("Homepage", "Home");

        }else{

            return RedirectToAction("Index", "Home");
        }
    }

    public IActionResult Demande()
    {
        HttpContext.Session.Remove("adminId");

        if(HttpContext.Session.GetString("sessionId") != null)
        {
            string idMaison = HttpContext.Session.GetString("idMaison");
            string sessionId = HttpContext.Session.GetString("sessionId");
            var idFinition = Request.Form["idFinition"].ToString();
            DateTime dateDebut = DateTime.Parse(Request.Form["date"].ToString());

            if (idFinition != null && dateDebut != null)
            {
                DemandeDevis d = new DemandeDevis();

                Connexion coco = new Connexion();
                coco.connection.Open();

                    DateTime dateFin = d.add(coco, dateDebut, idMaison);
                    DemandeDevis insert = new DemandeDevis(sessionId, dateDebut, dateFin, idMaison, idFinition);
                    if (insert.Insert(coco))
                    {
                        Paiement pInsert = new Paiement(d.lastId(coco), 0);
                        // pInsert.insert(coco);
                        coco.connection.Close();
                        return RedirectToAction("listDemande", "Home");
                    }else
                    {
                        ViewData["error"] = "Tsy mety inserer";
                        coco.connection.Close();
                        return RedirectToAction("Finition", "Home");
                    }

                coco.connection.Close();

                return View("ListDemande");
            }
        
            ViewData["error"] = "Tsy voray ny finition tianao na ny date";
            return RedirectToAction("Finition", "Home");

        }else{

            return RedirectToAction("Index", "Home");
        }
    }

    public IActionResult listDemande()
    {
        HttpContext.Session.Remove("adminId");

        if(HttpContext.Session.GetString("sessionId") != null)
        {
            DemandeDevis dd = new();
            Data data = new Data();

            Connexion coco = new Connexion();
            coco.connection.Open();

            string lastId = dd.lastId(coco);
            Paiement p = new Paiement(lastId, 0);
            p.insert(coco, p);
            data.demandeList = dd.findAll(coco, HttpContext.Session.GetString("sessionId"));

            coco.connection.Close();
            return View("ListDemande", data);

        }else{

            return RedirectToAction("Index", "Home");
        }
    }

    public IActionResult detailDemande(string idDevis)
    {
        HttpContext.Session.Remove("adminId");

        if(HttpContext.Session.GetString("sessionId") != null)
        {
            DemandeDevis dd = new();
            Data data = new Data();

            Connexion coco = new Connexion();
            coco.connection.Open();

            data.demandeList = dd.detailDevis(coco, idDevis);

            coco.connection.Close();
            return View("Detail", data);

        }else{

            return RedirectToAction("Index", "Home");
        }
    }
    
    public IActionResult payer(string idDemande)
    {
        HttpContext.Session.Remove("adminId");

        if(HttpContext.Session.GetString("sessionId") != null)
        {
            DemandeDevis dd = new();
            Data data = new Data();

            Connexion coco = new Connexion();
            coco.connection.Open();

            data.infopaye = dd.infoPaye(coco, idDemande);

            coco.connection.Close();
            return View("Payer", data);

        }else{

            return RedirectToAction("Index", "Home");
        }
    }

    public IActionResult paiement()
    {
        HttpContext.Session.Remove("adminId");

        if(HttpContext.Session.GetString("sessionId") != null)
        {
            try
            {
                double payer = (double)Convert.ToInt32(Request.Form["payer"]);
                var idDemande = Request.Form["idDemande"].ToString();
                DateTime date = new();

                if (Request.Form["date"].ToString() == null)
                {
                    date = DateTime.Now;

                }else
                {
                    date = DateTime.Parse(Request.Form["date"].ToString());
                }

                if (idDemande != null)
                {
                    Paiement pa = new Paiement(date, payer, idDemande);

                    Connexion coco = new Connexion();
                    coco.connection.Open();

                    pa.insert2(coco, pa);

                    coco.connection.Close();
                    return RedirectToAction("listDemande", "Home");
                }
                return RedirectToAction("ErrorPage", "Home");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                // ViewData["error"] = ex.Message;
                return RedirectToAction("ErrorPage", "Home");
            }
            
        }else{
            return RedirectToAction("Index", "Home");
        }
    }

    // public IActionResult pdf(string idDevis)
    // {
    //     try
    //     {
    //         if (HttpContext.Session.GetString("sessionId") != null)
    //         {
    //             // Your logic to fetch data based on idDevis
    //             string data = GetData(idDevis);

    //             // Generate PDF using iTextSharp
    //             byte[] pdfBytes = GeneratePDF(data);

    //             // Return the PDF as a file for download
    //             return File(pdfBytes, "application/pdf", "your_filename.pdf");
    //         }
    //         else
    //         {
    //             return RedirectToAction("Index", "Home");
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine("Error: {0}", ex.Message);
    //         return RedirectToAction("ErrorPage", "Home");
    //     }
    // }

    private string GetData(string idDevis)
    {
        HttpContext.Session.Remove("adminId");

        DemandeDevis dd = new DemandeDevis();
        Data data = new Data();

        Connexion coco = new Connexion();
        coco.connection.Open();

        data.demandeList = dd.detailDevis(coco, idDevis);

        coco.connection.Close();
        return $"Data related to {idDevis}";
    }

    // private byte[] GeneratePDF(string data)
    // {
    //     using (MemoryStream stream = new MemoryStream())
    //     {
    //         PdfWriter writer = new PdfWriter(stream);
    //         PdfDocument pdf = new PdfDocument(writer);
    //         Document document = new Document(pdf);

    //         // Add content to the PDF document
    //         document.Add(new Paragraph(data));

    //         // Close the document
    //         document.Close();

    //         return stream.ToArray();
    //     }
    // }

    public IActionResult ResetDatabase()
    {
        if(HttpContext.Session.GetString("sessionId") != null)
        {
            Connexion coco = new Connexion();
            coco.connection.Open();
                Connexion.ResetDatabase(coco);
            coco.connection.Close();
            return RedirectToAction("Homepage", "Home");

        }else{

            return RedirectToAction("Index", "Home");
        }
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // public IActionResult YourAction()
    // {
    //     try
    //     {
    //         // Code that uses your model method
    //         var model = new YourModel();
    //         var result = model.YourModelMethod();

    //         return View(result); // Assuming result is a model you want to pass to the view
    //     }
    //     catch (Exception ex)
    //     {
    //         ViewData["error"] = ex.Message;
    //         return View();
    //     }
    // }

}
