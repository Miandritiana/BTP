using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BTP.Models;

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
        return RedirectToAction("Index", "Home");
    }


    public IActionResult checkLog()
    {
        var username = Request.Form["username"].ToString();
        var passW = Request.Form["password"].ToString();

        Uuser uu = new Uuser();

        Connexion coco = new Connexion();
        coco.connection.Open();
        
        string idUser = uu.checkLogin(coco, username, passW);
        if (idUser != null)
        {
            HttpContext.Session.SetString("sessionId", idUser);
            bool isSuperUser = uu.isAdmin(coco, idUser);

            if (isSuperUser)
            {
                coco.connection.Close();
                // return RedirectToAction("ListBillet", "Super");
            }

            coco.connection.Close();
            return RedirectToAction("Homepage", "Home");

        }else{
            TempData["error"] = "Misy diso ny user na mdp";
            coco.connection.Close();
            return RedirectToAction("Index", "Home", new { error = true });
        }

        coco.connection.Close();

        return View();
    }

    public IActionResult import()
    {
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
        if(HttpContext.Session.GetString("sessionId") != null)
        {
            return View("Homepage");

        }else{

            return RedirectToAction("Index", "Home");
        }
    }

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
