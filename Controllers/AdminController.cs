using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BTP.Models;
using Newtonsoft.Json;
using System.Text.Json;

namespace BTP.Controllers;

public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;

    public AdminController(ILogger<AdminController> logger)
    {
        _logger = logger;
    }

    public IActionResult Log(bool error = false)
    {
        HttpContext.Session.Remove("sessionId");

        if (error)
        {
            ViewData["error"] = TempData["error"]?.ToString();
        }
        return View();
    }

    public IActionResult checkLog()
    {
        HttpContext.Session.Remove("sessionId");

        var username = Request.Form["username"].ToString();
        var passW = Request.Form["password"].ToString();

        Uuser uu = new Uuser();

        Connexion coco = new Connexion();
        coco.connection.Open();
        
        string idUser = uu.checkLogin(coco, username, passW);
        if (idUser != null)
        {
            HttpContext.Session.SetString("adminId", idUser);
            bool isAdmin = uu.isAdmin(coco, idUser);

            if (isAdmin)
            {
                return RedirectToAction("Index", "Admin");

            }else{

                TempData["error"] = "Misy diso ny user na mdp";
                return RedirectToAction("Log", "Admin", new { error = true });
            }

        }else{
            TempData["error"] = "Misy diso ny user na mdp";
            return RedirectToAction("Log", "Admin", new { error = true });
        }

        coco.connection.Close();

        return View();
    }

    public IActionResult Index()
    {
        HttpContext.Session.Remove("sessionId");

        if(HttpContext.Session.GetString("adminId") != null)
        {
            DemandeDevis d = new DemandeDevis();
            Data data = new Data();

            Connexion coco = new Connexion();
            coco.connection.Open();

            data.demandeList = d.findAllnoUser(coco);
            
            data.montantTotalEnCours = d.montantTotalEnCours(coco);
            data.montantDejaEffectue = d.montantDejaEffectue(coco);
            data.montantTotalDesDevis = d.montantTotalDesDevis(coco);

            coco.connection.Close();
            return View("Index", data);

        }else{

            return RedirectToAction("Log", "Admin");
        }
    }

    public IActionResult detailDemande(string idDevis)
    {
        HttpContext.Session.Remove("sessionId");

        if(HttpContext.Session.GetString("adminId") != null)
        {
            DemandeDevis dd = new();
            Data data = new Data();

            Connexion coco = new Connexion();
            coco.connection.Open();

            string lastId = dd.lastId(coco);
            Paiement p = new Paiement(lastId, 0);
            p.insert(coco, p);
            data.demandeList = dd.detailDevis(coco, idDevis);

            coco.connection.Close();
            return View("Detail", data);

        }else{

            return RedirectToAction("Index", "Home");
        }
    }

    public IActionResult dash()
    {
        HttpContext.Session.Remove("sessionId");

        if(HttpContext.Session.GetString("adminId") != null)
        {
            DemandeDevis d = new DemandeDevis();
            Chart c = new Chart();
            Data data = new Data();

            int year = DateTime.Now.Year;
            if (TempData["year"] != null)
            {
                year = int.Parse(TempData["year"].ToString());
            }

            Connexion coco = new Connexion();
            coco.connection.Open();

            data.listYear = d.listYear(coco);
            data.montantTotalDesDevis = d.montantTotalDesDevis(coco);
            data.chartList = c.chart(coco, year);

            coco.connection.Close();
            
            var jsonData = System.Text.Json.JsonSerializer.Serialize(data.chartList);
            if (jsonData != null)
            {
                ViewData["JsonData"] = jsonData;
            }
            else
            {
                // Handle the case where jsonData is null
                Console.WriteLine("jsonData is null");
            }

            foreach (var item in data.demandeList)
            {
                Console.WriteLine(item.month +" "+item.montant);
            }

            return View("dash", data);

        }else{

            return RedirectToAction("Log", "Admin");
        }
    }

    public IActionResult chart(int year)
    {
        HttpContext.Session.Remove("sessionId");

        if (HttpContext.Session.GetString("adminId") != null)
        {
            Console.WriteLine("sdfghn"+year);
            DemandeDevis d = new DemandeDevis();
            Data data = new Data();

            TempData["year"] = year;

            return RedirectToAction("dash");
        }
        else
        {
            return RedirectToAction("Log", "Admin");
        }
    }

    public IActionResult ResetDatabase()
    {
        HttpContext.Session.Remove("sessionId");
        if(HttpContext.Session.GetString("adminId") != null)
        {
            Connexion coco = new Connexion();
            coco.connection.Open();
                Connexion.ResetDatabase(coco);
            coco.connection.Close();
            return RedirectToAction("Index", "Admin");

        }else{

            return RedirectToAction("Log", "Admin");
        }
    }

    public IActionResult listTravaux()
    {
        HttpContext.Session.Remove("sessionId");

        if(HttpContext.Session.GetString("adminId") != null)
        {
            Tache ta = new Tache();
            Data data = new Data();

            Connexion coco = new Connexion();
            coco.connection.Open();

            data.tacheList = ta.get(coco);

            coco.connection.Close();
            return View("ListTravaux", data);

        }else{

            return RedirectToAction("Log", "Admin");
        }
    }

    public IActionResult pageModif(string idTache)
    {
        HttpContext.Session.Remove("sessionId");

        if (HttpContext.Session.GetString("adminId") != null)
        {
            Data data = new Data();

            Tache ta = new Tache();
            Connexion coco = new Connexion();
            coco.connection.Open();

            data.tache = ta.getById(coco, idTache);

            coco.connection.Close();

            return View("modif", data);
        }
        else
        {
            return RedirectToAction("Log", "Admin");
        }
    }

    public IActionResult goModif()
    {
        HttpContext.Session.Remove("sessionId");

        if (HttpContext.Session.GetString("adminId") != null)
        {
            var idTache = Request.Form["idTache"].ToString();
            var num = Request.Form["num"].ToString();
            var designation = Request.Form["designation"].ToString();
            var unite = Request.Form["unite"].ToString();
            double pu = Convert.ToDouble(Request.Form["pu"]);

            Data data = new Data();
            Tache ta = new Tache(idTache, num, designation, unite, pu);

            Connexion coco = new Connexion();
            coco.connection.Open();

            ta.update(coco, ta);

            coco.connection.Close();

            return RedirectToAction("listTravaux", "Admin");
        }
        else
        {
            return RedirectToAction("Log", "Admin");
        }
    }

    public IActionResult import()
    {
        HttpContext.Session.Remove("sessionId");

        if(HttpContext.Session.GetString("adminId") != null)
        {
            return View("Import");

        }else{

            return RedirectToAction("Log", "Admin");
        }
    }

    public IActionResult ImportMaisonTrav(IFormFile csvFile)
    {
        if (csvFile != null && csvFile.Length > 0)
        {
            try
            {
                ImportMaisonTravaux imp = new ImportMaisonTravaux();

                Connexion coco = new Connexion();
                coco.connection.Open();
                
                string message = imp.import(csvFile, coco);

                coco.connection.Close();
                
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

    public IActionResult ImportDevis(IFormFile csvFile)
    {
        if (csvFile != null && csvFile.Length > 0)
        {
            try
            {
                ImportDevis imp = new ImportDevis();

                Connexion coco = new Connexion();
                coco.connection.Open();

                string message = imp.import(csvFile, coco);

                coco.connection.Close();
                
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

    public IActionResult ImportPaiement(IFormFile csvFile)
    {
        if (csvFile != null && csvFile.Length > 0)
        {
            try
            {
                ImportPaiement imp = new ImportPaiement();

                Connexion coco = new Connexion();
                coco.connection.Open();

                string message = imp.import(csvFile, coco);

                coco.connection.Close();
                
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

}
