using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using spark.Data;
using spark.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// =========================
// Database
// =========================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// =========================
// Identity (THIS IS THE KEY)
// =========================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// =========================
// Controllers & Swagger
// =========================
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        // Prevent JSON serializer errors from navigation property cycles
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =========================
// App
// =========================
var app = builder.Build();

// DB init
using (var scope = app.Services.CreateScope())
{
    DbInitializer.Initialize(scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🔥 REQUIRED
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
