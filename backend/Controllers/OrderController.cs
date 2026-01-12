using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spark.Data;
using System.Security.Claims;
using spark.Models;

[ApiController]
[Route("api/orders")]
[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
public class OrdersController : ControllerBase
{
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
    {
        // 1️⃣ Get the current user ID
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        // 2️⃣ Load the computer
        var computer = await _context.Computers
            .FirstOrDefaultAsync(c => c.Id == dto.ComputerId);

        if (computer == null)
            return BadRequest("Invalid computer ID.");

        // 3️⃣ Load components
        var components = await _context.Components
            .Where(c => dto.ComponentIds.Contains(c.Id))
            .ToListAsync();

        if (components.Count != dto.ComponentIds.Count)
            return BadRequest("One or more component IDs are invalid.");

        // 4️⃣ Calculate total price (server-side)
        var totalPrice = computer.Price + components.Sum(c => c.Price);

        // 5️⃣ Create the order along with its components
        var order = new Order
        {
            UserId = userId,
            ComputerId = computer.Id,
            TotalPrice = totalPrice,
            OrderDate = DateTime.UtcNow,
            OrderComponents = components.Select(c => new OrderComponent
            {
                ComponentId = c.Id
            }).ToList()
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(); // EF handles both Order and OrderComponents

        // 6️⃣ Load the order with components to return
        var createdOrder = await _context.Orders
            .Include(o => o.Computer)
            .Include(o => o.OrderComponents)
                .ThenInclude(oc => oc.Component)
            .FirstOrDefaultAsync(o => o.Id == order.Id);

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, createdOrder);
    }


        // GET: api/orders → all orders for logged-in customer
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _context.Orders
                .Include(o => o.Computer)
                .Include(o => o.OrderComponents)
                    .ThenInclude(oc => oc.Component)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var ordersDto = orders.Select(o => new OrderResponseDto
            {
                Id = o.Id,
                TotalPrice = o.TotalPrice,
                OrderDate = o.OrderDate,
                Computer = new ComputerDto
                {
                    Id = o.Computer.Id,
                    Name = o.Computer.Name,
                    Price = o.Computer.Price,
                    ImageUrl = o.Computer.ImageUrl
                },
                Components = o.OrderComponents.Select(oc => new ComponentDto
                {
                    Id = oc.Component.Id,
                    Name = oc.Component.Name,
                    Price = oc.Component.Price,
                    Type = oc.Component.Type
                }).ToList()
            }).ToList();

            return Ok(ordersDto);
        }

        // GET: api/orders/{id} → single order of logged-in customer
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = await _context.Orders
                .Include(o => o.Computer)
                .Include(o => o.OrderComponents)
                    .ThenInclude(oc => oc.Component)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
                return NotFound();

            var orderDto = new OrderResponseDto
            {
                Id = order.Id,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,
                Computer = new ComputerDto
                {
                    Id = order.Computer.Id,
                    Name = order.Computer.Name,
                    Price = order.Computer.Price,
                    ImageUrl = order.Computer.ImageUrl
                },
                Components = order.OrderComponents.Select(oc => new ComponentDto
                {
                    Id = oc.Component.Id,
                    Name = oc.Component.Name,
                    Price = oc.Component.Price,
                    Type = oc.Component.Type
                }).ToList()
            };

            return Ok(orderDto);
        }

        // DELETE: api/orders/{id} → delete order of logged-in customer
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = await _context.Orders
                .Include(o => o.OrderComponents)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }
    }