using Backend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Konfigurera CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                           policy.AllowAnyOrigin()  // Tillåter alla domäner (React + andra)
                  .AllowAnyMethod()  // Tillåter GET, POST, PUT, DELETE osv.
                  .AllowAnyHeader(); // Tillåter alla headers
                      });
});

// Konfigurera SQLite-databasen
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); 

// Lägg till tjänster för controllers och Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins); // <--- lägg till detta

app.UseAuthorization();

app.MapControllers();

app.Run();
