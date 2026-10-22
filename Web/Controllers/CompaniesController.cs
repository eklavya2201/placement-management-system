using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using PlacementManagementSystem.Data;
using PlacementManagementSystem.Models;

namespace PlacementManagementSystem.Web.Controllers;

public class CompaniesController : Controller
{
    private readonly CompanyRepository _repo = new();

    public IActionResult Index()
    {
        return View(_repo.GetAll());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Company company)
    {
        if (!ModelState.IsValid)
            return View(company);

        try
        {
            _repo.Add(company);
            TempData["Message"] = $"Company '{company.Name}' added.";
            return RedirectToAction(nameof(Index));
        }
        catch (MySqlException ex)
        {
            ModelState.AddModelError("", "Failed to add company: " + ex.Message);
            return View(company);
        }
    }
}
