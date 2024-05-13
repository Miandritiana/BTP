using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BTP.Models;

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
        if (error)
        {
            ViewData["error"] = TempData["error"]?.ToString();
        }
        return View();
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
        return View();
    }
}
