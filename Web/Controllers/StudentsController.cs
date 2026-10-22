using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using PlacementManagementSystem.Data;
using PlacementManagementSystem.Models;

namespace PlacementManagementSystem.Web.Controllers;

public class StudentsController : Controller
{
    private readonly StudentRepository _repo = new();

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
    public IActionResult Create(Student student)
    {
        if (!ModelState.IsValid)
            return View(student);

        try
        {
            _repo.Add(student);
            TempData["Message"] = $"Student '{student.Name}' added.";
            return RedirectToAction(nameof(Index));
        }
        catch (MySqlException ex)
        {
            ModelState.AddModelError("", "Failed to add student: " + ex.Message);
            return View(student);
        }
    }
}
