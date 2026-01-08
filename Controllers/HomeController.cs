using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
// Controllers/HomeController.cs
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

public class ComputerController : Controller
{
    private readonly ApplicationDbContext _context;

    public ComputerController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult List()
    {
        // Retrieve list of computers from the database
        var computers = _context.Computers.ToList();
        return View(computers);
    }

    public IActionResult Customize(int id)
    {
        // Retrieve computer and components from the database
        var computer = _context.Computers
            .Include(c => c.Components) // Load related components
            .FirstOrDefault(c => c.Id == id);

        if (computer == null)
        {
            return NotFound();
        }

        var components = _context.Components.ToList(); // Get all components

        ViewBag.Computer = computer;
        return View(components);
    }

    [HttpPost]
    public async Task<IActionResult> Summary(int computerId, Dictionary<string, int> components)
    {
        var customerId = HttpContext.Session.GetString("CustomerId");

        if (string.IsNullOrEmpty(customerId))
        {
            return RedirectToAction("Login", "Account");
        }

        var computer = await _context.Computers.FindAsync(computerId);
        if (computer == null)
        {
            return NotFound();
        }

        var selectedComponentIds = components.Values.Distinct().ToList();
        var selectedComponents = await _context.Components
            .Where(c => selectedComponentIds.Contains(c.Id))
            .ToListAsync();

        var order = new Order
        {
            CustomerId = customerId,
            ComputerId = computerId,
            Computer = computer,
            TotalPrice = selectedComponents.Sum(c => c.Price) + computer.Price,
            OrderDate = DateTime.Now
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Create OrderComponents after Order has been saved
        var orderComponents = selectedComponents.Select(c => new OrderComponent
        {
            OrderId = order.Id,
            ComponentId = c.Id
        }).ToList();

        _context.OrderComponents.AddRange(orderComponents);
        await _context.SaveChangesAsync();

        return View(order);
    }




}



public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailSender _emailSender;

    public AccountController(ApplicationDbContext context, IEmailSender emailSender)
    {
        _context = context;
        _emailSender = emailSender;
    }
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(), // Set a new unique Id
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                SecurityQuestion = model.SecurityQuestion,
                SecurityAnswerHash = BCrypt.Net.BCrypt.HashPassword(model.SecurityAnswer)

            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(Login model)
    {
        if (ModelState.IsValid)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == model.Email);

            if (customer != null && BCrypt.Net.BCrypt.Verify(model.Password, customer.PasswordHash))
            {
                // Simulate a successful login
                HttpContext.Session.SetString("CustomerId", customer.Id);
                HttpContext.Session.SetString("CustomerEmail", customer.Email);
                HttpContext.Session.SetString("CustomerName", customer.FirstName);

                ViewBag.CustomerId = customer.Id;
                ViewBag.CustomerEmail = customer.Email;
                ViewBag.CustomerName = customer.FirstName;

                return RedirectToAction("Index", "Home");

            }

            ViewBag.LoginFailed = true;
        }

        return View(model);
    }
    public IActionResult Logout()
    {
        // Clear the session data
        HttpContext.Session.Clear();

        return RedirectToAction("Index", "Home"); // Redirect to home page
    }


    public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
    {
        if (Request.Method == HttpMethods.Get)
        {
            // Return an empty model for the initial GET request
            return View(new RecoverPasswordViewModel());
        }

        if (Request.Method == HttpMethods.Post)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError("", "Email is required.");
                return View(new RecoverPasswordViewModel());
            }

            // Check if the email is valid
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == model.Email);

            if (customer == null)
            {
                ModelState.AddModelError("", "Email not found.");
                return View(new RecoverPasswordViewModel()); // Return a new model
            }

            // Check if the security answer is provided and valid
            if (string.IsNullOrEmpty(model.SecurityAnswer) ||
                string.IsNullOrEmpty(customer.SecurityAnswerHash) ||
                !BCrypt.Net.BCrypt.Verify(model.SecurityAnswer, customer.SecurityAnswerHash))
            {
                ModelState.AddModelError("", "Security answer is incorrect.");
                return View(new RecoverPasswordViewModel
                {
                    Email = customer.Email,
                    SecurityQuestion = customer.SecurityQuestion
                }); // Return with security question
            }

            // Ensure NewPassword is not null or empty
            if (string.IsNullOrEmpty(model.NewPassword))
            {
                ModelState.AddModelError("", "New password cannot be empty.");
                return View(new RecoverPasswordViewModel
                {
                    Email = customer.Email,
                    SecurityQuestion = customer.SecurityQuestion
                }); // Return with security question
            }

            // Hash the new password
            customer.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                // Log exception if needed
                ModelState.AddModelError("", "An error occurred while updating the password.");
                return View(new RecoverPasswordViewModel
                {
                    Email = customer.Email,
                    SecurityQuestion = customer.SecurityQuestion
                }); // Return with security question
            }
        }

        // Handle unexpected method or case
        return View(new RecoverPasswordViewModel());
    }





}

public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrderController> _logger;

    public OrderController(ApplicationDbContext context, ILogger<OrderController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Orders
    public async Task<IActionResult> Index()
    {
        var customerId = HttpContext.Session.GetString("CustomerId");

        if (string.IsNullOrEmpty(customerId))
        {
            return RedirectToAction("Login", "Account");
        }

        var orders = await _context.Orders
            .Include(o => o.Computer)
            .Include(o => o.OrderComponents)
                .ThenInclude(oc => oc.Component)
            .Where(o => o.CustomerId == customerId)
            .ToListAsync();

        return View(orders);
    }

    // GET: Orders/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Computer)
            .Include(o => o.OrderComponents)
                .ThenInclude(oc => oc.Component)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    // GET: Orders/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderComponents)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        // Remove the order and its components if necessary
        _context.OrderComponents.RemoveRange(order.OrderComponents);
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    
    public async Task<IActionResult> Edit(int id)
    {
        if (Request.Method == "GET")
        {
            // Retrieve the order with its components
            var order = await _context.Orders
                .Include(o => o.OrderComponents)
                .ThenInclude(oc => oc.Component)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            // Create the view model
            var model = new EditOrderViewModel
            {
                OrderId = order.Id,
                ComputerId = order.ComputerId,
                SelectedComponents = order.OrderComponents
                    .ToDictionary(oc => oc.Component.Type, oc => oc.ComponentId),
                Computers = await _context.Computers.ToListAsync(),
                Components = await _context.Components.ToListAsync()
            };

            return View(model);
        }
        else if (Request.Method == "POST")
        {
            // Retrieve the selected computer
            var computerId = int.TryParse(Request.Form["ComputerId"], out var compId) ? compId : 0;

            // Retrieve the existing order
            var order = await _context.Orders
                .Include(o => o.OrderComponents)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                ViewBag.ErrorMessage = "Order not found.";
                return View(await GetEditOrderViewModel(id));
            }

            // Update the computer
            var computer = await _context.Computers.FindAsync(computerId);
            if (computer == null)
            {
                ViewBag.ErrorMessage = "Selected computer not found.";
                return View(await GetEditOrderViewModel(id));
            }

            order.ComputerId = computerId;
            order.Computer = computer;

            // Clear existing components
            _context.OrderComponents.RemoveRange(order.OrderComponents);

            // Retrieve the selected components from the form
            var selectedComponentIds = Request.Form
                .Where(f => f.Key.StartsWith("SelectedComponents["))
                .Select(f => int.TryParse(f.Value, out var componentId) ? componentId : (int?)null)
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .ToList();

            // Add new components to the order
            foreach (var componentId in selectedComponentIds)
            {
                order.OrderComponents.Add(new OrderComponent
                {
                    OrderId = order.Id,
                    ComponentId = componentId
                });
            }

            // Calculate the total price
            order.TotalPrice = order.Computer.Price + (decimal)(await _context.Components
    .Where(c => selectedComponentIds.Contains(c.Id))
    .SumAsync(c => (double)c.Price));

            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Order");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                return View(await GetEditOrderViewModel(id));
            }
        }

        return BadRequest(); // If neither GET nor POST, return bad request
    }



    private async Task<EditOrderViewModel> GetEditOrderViewModel(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderComponents)
            .ThenInclude(oc => oc.Component)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return null;
        }

        return new EditOrderViewModel
        {
            OrderId = order.Id,
            ComputerId = order.ComputerId,
            SelectedComponents = order.OrderComponents
                .ToDictionary(oc => oc.Component.Type, oc => oc.ComponentId),
            Computers = await _context.Computers.ToListAsync(),
            Components = await _context.Components.ToListAsync()
        };
    }



    // POST: Orders/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderComponents) // Load related OrderComponents
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        // Remove associated OrderComponents
        _context.OrderComponents.RemoveRange(order.OrderComponents);

        // Remove the order
        _context.Orders.Remove(order);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

}
