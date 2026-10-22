using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlConnector;
using PlacementManagementSystem.Data;
using PlacementManagementSystem.Models;

namespace PlacementManagementSystem.Web.Controllers;

public class JobDrivesController : Controller
{
    private readonly JobDriveRepository _repo = new();
    private readonly CompanyRepository _companyRepo = new();

    public IActionResult Index()
    {
        return View(_repo.GetAllWithCompanyName());
    }

    public IActionResult Create()
    {
        ViewBag.Companies = new SelectList(_companyRepo.GetAll(), "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(JobDrive jobDrive)
    {
        if (_companyRepo.GetById(jobDrive.CompanyId) is null)
            ModelState.AddModelError(nameof(jobDrive.CompanyId), "Select a company.");

        if (!ModelState.IsValid)
        {
            ViewBag.Companies = new SelectList(_companyRepo.GetAll(), "Id", "Name", jobDrive.CompanyId);
            return View(jobDrive);
        }

        try
        {
            _repo.Add(jobDrive);
            TempData["Message"] = $"Job drive for '{jobDrive.Role}' added.";
            return RedirectToAction(nameof(Index));
        }
        catch (MySqlException ex)
        {
            ModelState.AddModelError("", "Failed to add job drive: " + ex.Message);
            ViewBag.Companies = new SelectList(_companyRepo.GetAll(), "Id", "Name", jobDrive.CompanyId);
            return View(jobDrive);
        }
    }
}
