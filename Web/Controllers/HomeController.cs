using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PlacementManagementSystem.Data;
using PlacementManagementSystem.Web.Models;

namespace PlacementManagementSystem.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var applications = new ApplicationRepository().GetAllWithDetails();
        ViewBag.Students = new StudentRepository().GetAll().Count;
        ViewBag.Companies = new CompanyRepository().GetAll().Count;
        ViewBag.Drives = new JobDriveRepository().GetAll().Count;
        ViewBag.Selected = applications.Count(a => a.Status == "Selected");
        return View(applications.OrderByDescending(a => a.Id).Take(5).ToList());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
