using CookingAssistantBackend.Models.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CookingAssistantContext>(
    options => options.UseSqlServer("Server=tcp:tab-projekt.database.windows.net,1433;Initial Catalog=cooking-assistant;Persist Security Info=False;User ID=tab;Password=Projekt1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(
//        builder =>
//        {
//            builder.WithOrigins("http://localhost:4200")
//            .AllowAnyHeader()
//            .AllowAnyMethod();
//        });
//});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("https://rookiedoo.herokuapp.com")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();