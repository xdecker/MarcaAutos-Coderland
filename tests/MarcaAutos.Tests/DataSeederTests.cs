using MarcaAutos.API.Data;
using MarcaAutos.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MarcaAutos.Tests;

public class DataSeederTests
{
    private static AppDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task SeedAsync_SeedsData_WhenEmpty()
    {
        using var context = CreateContext(nameof(SeedAsync_SeedsData_WhenEmpty));

        await DataSeeder.SeedAsync(context);

        Assert.Equal(3, await context.MarcasAutos.CountAsync());
        Assert.NotNull(await context.MarcasAutos.FirstOrDefaultAsync(m => m.Nombre == "Toyota"));
        Assert.NotNull(await context.MarcasAutos.FirstOrDefaultAsync(m => m.Nombre == "Ford"));
        Assert.NotNull(await context.MarcasAutos.FirstOrDefaultAsync(m => m.Nombre == "Volkswagen"));
    }

    [Fact]
    public async Task SeedAsync_DoesNotSeed_WhenDataExists()
    {
        using var context = CreateContext(nameof(SeedAsync_DoesNotSeed_WhenDataExists));

        context.MarcasAutos.Add(new MarcaAuto { Nombre = "Honda", Activo = true, FechaCreacion = DateTime.UtcNow });
        await context.SaveChangesAsync();

        await DataSeeder.SeedAsync(context);

        Assert.Single(await context.MarcasAutos.ToListAsync());
        Assert.Null(await context.MarcasAutos.FirstOrDefaultAsync(m => m.Nombre == "Toyota"));
    }

    [Fact]
    public async Task SeedAsync_SetsAuditFields()
    {
        using var context = CreateContext(nameof(SeedAsync_SetsAuditFields));

        await DataSeeder.SeedAsync(context);

        var toyota = await context.MarcasAutos.FirstAsync(m => m.Nombre == "Toyota");
        Assert.True(toyota.Activo);
        Assert.NotEqual(default, toyota.FechaCreacion);
        Assert.Null(toyota.FechaActualizacion);
        Assert.Null(toyota.FechaEliminacion);
    }
}
