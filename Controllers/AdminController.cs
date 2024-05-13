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

    public IActionResult Index()
    {
        return View();
    }
}
