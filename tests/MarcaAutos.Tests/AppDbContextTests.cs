using MarcaAutos.API.Data;
using MarcaAutos.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MarcaAutos.Tests;

public class AppDbContextTests
{
    private static AppDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task SaveChangesAsync_SetsFechaCreacion_OnAdd()
    {
        using var context = CreateContext(nameof(SaveChangesAsync_SetsFechaCreacion_OnAdd));
        var marca = new MarcaAuto { Nombre = "Test" };

        context.MarcasAutos.Add(marca);
        await context.SaveChangesAsync();

        Assert.NotEqual(default, marca.FechaCreacion);
    }

    [Fact]
    public async Task SaveChangesAsync_SetsActivoTrue_OnAdd()
    {
        using var context = CreateContext(nameof(SaveChangesAsync_SetsActivoTrue_OnAdd));
        var marca = new MarcaAuto { Nombre = "Test" };

        context.MarcasAutos.Add(marca);
        await context.SaveChangesAsync();

        Assert.True(marca.Activo);
    }

    [Fact]
    public async Task SaveChangesAsync_SetsFechaActualizacion_OnUpdate()
    {
        using var context = CreateContext(nameof(SaveChangesAsync_SetsFechaActualizacion_OnUpdate));
        var marca = new MarcaAuto { Nombre = "Test" };
        context.MarcasAutos.Add(marca);
        await context.SaveChangesAsync();

        marca.Nombre = "Updated";
        await context.SaveChangesAsync();

        Assert.NotNull(marca.FechaActualizacion);
    }

    [Fact]
    public async Task SaveChangesAsync_DoesNotSetFechaActualizacion_OnAdd()
    {
        using var context = CreateContext(nameof(SaveChangesAsync_DoesNotSetFechaActualizacion_OnAdd));
        var marca = new MarcaAuto { Nombre = "Test" };

        context.MarcasAutos.Add(marca);
        await context.SaveChangesAsync();

        Assert.Null(marca.FechaActualizacion);
    }

    [Fact]
    public async Task SaveChangesAsync_SetsFechaCreacion_OverwritingAnyPresetValue()
    {
        using var context = CreateContext(nameof(SaveChangesAsync_SetsFechaCreacion_OverwritingAnyPresetValue));
        var specificDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var marca = new MarcaAuto { Nombre = "Test", FechaCreacion = specificDate };

        context.MarcasAutos.Add(marca);
        await context.SaveChangesAsync();

        Assert.NotEqual(specificDate, marca.FechaCreacion);
        Assert.True(marca.FechaCreacion > specificDate);
    }

    [Fact]
    public async Task SaveChangesAsync_DoesNotSetFechaEliminacion_OnDelete()
    {
        using var context = CreateContext(nameof(SaveChangesAsync_DoesNotSetFechaEliminacion_OnDelete));
        var marca = new MarcaAuto { Nombre = "Test" };
        context.MarcasAutos.Add(marca);
        await context.SaveChangesAsync();

        marca.Activo = false;
        marca.FechaEliminacion = DateTime.UtcNow;
        await context.SaveChangesAsync();

        Assert.NotNull(marca.FechaEliminacion);
    }

    [Fact]
    public async Task OnModelCreating_ConfiguresTableName()
    {
        using var context = CreateContext(nameof(OnModelCreating_ConfiguresTableName));
        var entityType = context.Model.FindEntityType(typeof(MarcaAuto));

        Assert.NotNull(entityType);
        Assert.Equal("MarcasAutos", entityType.GetTableName());
    }
}
