using Backend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// ✅ Lägg till CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {

            policy.WithOrigins("http://localhost:5173")

            policy.AllowAnyOrigin()

                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Routing måste komma FÖRE CORS
app.UseRouting();

// ✅ Använd CORS (oberoende av environment)
app.UseCors(MyAllowSpecificOrigins);

// ✅ Swagger vid behov
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// ✅ Initiera databas och data
using (var scope = app.Services.CreateScope())
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
    var dbHandler = new DatabaseHandler(connectionString);
    dbHandler.InitializeDatabase();
    dbHandler.InsertRestaurants();
    dbHandler.DisplayRestaurants();
}

app.Run();
