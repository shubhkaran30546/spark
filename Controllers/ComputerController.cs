using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spark.Data;
using spark.Models;

[ApiController]
[Route("api/[controller]")]
public class ComputersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ComputersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/computers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Computer>>> GetAll()
    {
        var computers = await _context.Computers
            .Include(c => c.Components)
            .ToListAsync();

        return Ok(computers);
    }

    // GET: api/computers/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Computer>> GetById(int id)
    {
        var computer = await _context.Computers
            .Include(c => c.Components)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (computer == null)
            return NotFound();

        return Ok(computer);
    }
}
