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
                // return RedirectToAction("ListBillet", "Super");
            }

            // return RedirectToAction("ListBillet", "Home");

        }else{
            TempData["error"] = "Misy diso ny user na mdp";
            return RedirectToAction("Index", "Home", new { error = true });
        }

        coco.connection.Close();

        return View();
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
}
