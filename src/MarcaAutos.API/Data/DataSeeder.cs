using MarcaAutos.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MarcaAutos.API.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.MarcasAutos.AnyAsync())
        {
            return;
        }

        var marcas = new List<MarcaAuto>
        {
            new()
            {
                Nombre = "Toyota",
                Descripcion = "Marca japonesa",
                PaisOrigen = "Japón",
                AnioFundacion = 1937
            },
            new()
            {
                Nombre = "Ford",
                Descripcion = "Marca estadounidense",
                PaisOrigen = "Estados Unidos",
                AnioFundacion = 1903
            },
            new()
            {
                Nombre = "Volkswagen",
                Descripcion = "Marca alemana",
                PaisOrigen = "Alemania",
                AnioFundacion = 1937
            }
        };

        context.MarcasAutos.AddRange(marcas);
        await context.SaveChangesAsync();
    }
}
