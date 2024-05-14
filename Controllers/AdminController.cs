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
            
            var jsonData = System.Text.Json.JsonSerializer.Serialize(data.demandeList);
            if (jsonData != null)
            {
                Console.WriteLine("sdafg" + jsonData);
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

}
