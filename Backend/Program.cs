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
            policy.WithOrigins("http://localhost:5174") // Endast frontend
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials(); // Om du använder authentication (t.ex. JWT, cookies)
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
    app.UseCors(MyAllowSpecificOrigins); // Lägg detta innan Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}
 
app.UseCors(MyAllowSpecificOrigins); // Se till att CORS används innan Authorization
 
app.UseAuthorization();
app.MapControllers();
 
app.Run();