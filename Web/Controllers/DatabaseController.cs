using Microsoft.AspNetCore.Mvc;
using PlacementManagementSystem.Data;

namespace PlacementManagementSystem.Web.Controllers;

public class DatabaseController : Controller
{
    private readonly DatabaseRepository _repo = new();

    public IActionResult Index(string table = "students")
    {
        if (!DatabaseRepository.Tables.Contains(table)) table = "students";
        (ViewBag.Version, ViewBag.Database) = _repo.GetServerInfo();
        return View(_repo.GetTable(table));
    }
}
