using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlConnector;
using PlacementManagementSystem.Data;
using PlacementManagementSystem.Models;

namespace PlacementManagementSystem.Web.Controllers;

public class ApplicationsController : Controller
{
    private readonly ApplicationRepository _repo = new();
    private readonly StudentRepository _studentRepo = new();
    private readonly JobDriveRepository _jobDriveRepo = new();

    public IActionResult Index()
    {
        ViewBag.Statuses = Enum.GetValues<ApplicationStatus>();
        return View(_repo.GetAllWithDetails());
    }

    public IActionResult Apply()
    {
        LoadDropdowns();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Apply(int studentId, int jobDriveId)
    {
        var student = _studentRepo.GetById(studentId);
        var drive = _jobDriveRepo.GetById(jobDriveId);

        if (student is null || drive is null)
        {
            ModelState.AddModelError("", "Select both a student and a job drive.");
        }
        else if (student.Cgpa < drive.MinCgpa)
        {
            ModelState.AddModelError("", $"Not eligible: this drive requires a minimum CGPA of {drive.MinCgpa}.");
        }
        else if (_repo.Exists(studentId, jobDriveId))
        {
            ModelState.AddModelError("", "This student has already applied to this job drive.");
        }

        if (!ModelState.IsValid)
        {
            LoadDropdowns();
            return View();
        }

        try
        {
            _repo.Add(new Application { StudentId = studentId, JobDriveId = jobDriveId });
            TempData["Message"] = "Application submitted.";
            return RedirectToAction(nameof(Index));
        }
        catch (MySqlException ex)
        {
            ModelState.AddModelError("", "Failed to submit application: " + ex.Message);
            LoadDropdowns();
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateStatus(int id, ApplicationStatus status)
    {
        if (_repo.UpdateStatus(id, status))
            TempData["Message"] = "Status updated.";
        else
            TempData["Message"] = "No such application.";

        return RedirectToAction(nameof(Index));
    }

    private void LoadDropdowns()
    {
        ViewBag.Students = new SelectList(_studentRepo.GetAll(), "Id", "Name");
        ViewBag.JobDrives = new SelectList(_jobDriveRepo.GetAllWithCompanyName(), "Id", "Display");
    }
}
