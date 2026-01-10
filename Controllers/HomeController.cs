using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spark.Data;
using spark.Models;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        var customerName = HttpContext.Session.GetString("CustomerName");
        ViewBag.CustomerName = customerName;
        return View();
    }
    

    public IActionResult Contact()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Feedback()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Feedback(string feedback)
    {
        if (string.IsNullOrEmpty(feedback))
        {
            ModelState.AddModelError("", "Feedback cannot be empty.");
            return View();
        }

        var newFeedback = new Feedback
        {
            Content = feedback,
            SubmittedAt = DateTime.Now
        };

        _context.Feedbacks.Add(newFeedback);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index"); // Redirect to the home page or another page
    }

    public IActionResult Privacy()
    {
        return View();
    }
}





