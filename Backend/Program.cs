using Backend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin()      // 🌍 Tillåter alla domäner (bra för test)
                  .AllowAnyMethod()      // ✅ GET, POST, PUT osv.
                  .AllowAnyHeader();     // ✅ Alla headers tillåtna
        });
});


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Swagger + CORS i utvecklingsläge
if (app.Environment.IsDevelopment())
{
    app.UseCors(MyAllowSpecificOrigins);
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

// ✅ Middleware
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.MapControllers();

// ✅ Kör databas-initialisering vid uppstart
using (var scope = app.Services.CreateScope())
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
    var dbHandler = new DatabaseHandler(connectionString);
    dbHandler.InitializeDatabase();
    dbHandler.InsertRestaurants();
    dbHandler.DisplayRestaurants();
}

app.Run();
