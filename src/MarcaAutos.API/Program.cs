using MarcaAutos.API.Data;
using MarcaAutos.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString(
                        "DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddScoped<IMarcaAutoService, MarcaAutoService>();

var app = builder.Build();
app.MapControllers();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.Run();


