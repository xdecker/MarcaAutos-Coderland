using MarcaAutos.API.Data;
using MarcaAutos.API.Models;
using MarcaAutos.API.Models.DTOs;
using MarcaAutos.API.Services;
using Microsoft.EntityFrameworkCore;

namespace MarcaAutos.Tests;

public class MarcaAutoServiceTests
{
    private static AppDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new AppDbContext(options);
    }

    private static async Task<AppDbContext> SeedWithDataAsync(string dbName)
    {
        var context = CreateContext(dbName);

        context.MarcasAutos.AddRange(
            new MarcaAuto
            {
                Nombre = "Toyota",
                Descripcion = "Marca japonesa",
                PaisOrigen = "Japón",
                AnioFundacion = 1937,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            },
            new MarcaAuto
            {
                Nombre = "Ford",
                Descripcion = "Marca estadounidense",
                PaisOrigen = "Estados Unidos",
                AnioFundacion = 1903,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            },
            new MarcaAuto
            {
                Nombre = "Mitsubishi",
                Descripcion = "Marca japonesa",
                PaisOrigen = "Japón",
                AnioFundacion = 1970,
                Activo = false,
                FechaCreacion = DateTime.UtcNow,
                FechaEliminacion = DateTime.UtcNow
            }
        );

        await context.SaveChangesAsync();
        return context;
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOnlyActiveRecords()
    {
        using var context = await SeedWithDataAsync(nameof(GetAllAsync_ReturnsOnlyActiveRecords));
        var service = new MarcaAutoService(context);

        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Count());
        Assert.DoesNotContain(result, m => m.Nombre == "Mitsubishi");
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoActiveRecords()
    {
        using var context = CreateContext(nameof(GetAllAsync_ReturnsEmptyList_WhenNoActiveRecords));
        var service = new MarcaAutoService(context);

        var result = await service.GetAllAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsRecord_WhenActive()
    {
        using var context = await SeedWithDataAsync(nameof(GetByIdAsync_ReturnsRecord_WhenActive));
        var service = new MarcaAutoService(context);

        var result = await service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Toyota", result.Nombre);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenRecordIsInactive()
    {
        using var context = await SeedWithDataAsync(nameof(GetByIdAsync_ReturnsNull_WhenRecordIsInactive));
        var service = new MarcaAutoService(context);

        var result = await service.GetByIdAsync(3);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenRecordDoesNotExist()
    {
        using var context = await SeedWithDataAsync(nameof(GetByIdAsync_ReturnsNull_WhenRecordDoesNotExist));
        var service = new MarcaAutoService(context);

        var result = await service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_AddsRecordSuccessfully()
    {
        using var context = CreateContext(nameof(CreateAsync_AddsRecordSuccessfully));
        var service = new MarcaAutoService(context);
        var dto = new CreateMarcaAutoDto
        {
            Nombre = "Honda",
            Descripcion = "Marca japonesa",
            PaisOrigen = "Japón",
            AnioFundacion = 1948
        };

        var result = await service.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("Honda", result.Nombre);
        Assert.Equal(1948, result.AnioFundacion);
    }

    [Fact]
    public async Task CreateAsync_SetsFechaCreacionAndActivo()
    {
        using var context = CreateContext(nameof(CreateAsync_SetsFechaCreacionAndActivo));
        var service = new MarcaAutoService(context);
        var dto = new CreateMarcaAutoDto { Nombre = "Honda" };

        await service.CreateAsync(dto);

        var marca = await context.MarcasAutos.FirstAsync();
        Assert.True(marca.Activo);
        Assert.NotEqual(default, marca.FechaCreacion);
        Assert.Null(marca.FechaEliminacion);
    }

    [Fact]
    public async Task CreateAsync_ReturnsNull_WhenDuplicateNameExists()
    {
        using var context = await SeedWithDataAsync(nameof(CreateAsync_ReturnsNull_WhenDuplicateNameExists));
        var service = new MarcaAutoService(context);
        var dto = new CreateMarcaAutoDto { Nombre = "toyota" };

        var result = await service.CreateAsync(dto);

        Assert.Null(result);
        Assert.Equal(2, await context.MarcasAutos.CountAsync(m => m.Activo));
    }

    [Fact]
    public async Task CreateAsync_AllowsDuplicateName_WhenExistingIsInactive()
    {
        using var context = await SeedWithDataAsync(nameof(CreateAsync_AllowsDuplicateName_WhenExistingIsInactive));
        var service = new MarcaAutoService(context);
        var dto = new CreateMarcaAutoDto { Nombre = "mitsubishi" };

        var result = await service.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("mitsubishi", result.Nombre, ignoreCase: true);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesRecordSuccessfully()
    {
        using var context = await SeedWithDataAsync(nameof(UpdateAsync_UpdatesRecordSuccessfully));
        var service = new MarcaAutoService(context);
        var dto = new UpdateMarcaAutoDto
        {
            Nombre = "Toyota Actualizado",
            Descripcion = "Descripción actualizada"
        };

        var result = await service.UpdateAsync(1, dto);

        Assert.NotNull(result);
        Assert.Equal("Toyota Actualizado", result.Nombre);
        Assert.Equal("Descripción actualizada", result.Descripcion);
    }

    [Fact]
    public async Task UpdateAsync_SetsFechaActualizacion()
    {
        using var context = await SeedWithDataAsync(nameof(UpdateAsync_SetsFechaActualizacion));
        var service = new MarcaAutoService(context);
        var dto = new UpdateMarcaAutoDto { Nombre = "Toyota" };

        await service.UpdateAsync(1, dto);

        var marca = await context.MarcasAutos.FindAsync(1);
        Assert.NotNull(marca!.FechaActualizacion);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenRecordNotFound()
    {
        using var context = await SeedWithDataAsync(nameof(UpdateAsync_ReturnsNull_WhenRecordNotFound));
        var service = new MarcaAutoService(context);
        var dto = new UpdateMarcaAutoDto { Nombre = "Nuevo" };

        var result = await service.UpdateAsync(999, dto);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenRecordIsInactive()
    {
        using var context = await SeedWithDataAsync(nameof(UpdateAsync_ReturnsNull_WhenRecordIsInactive));
        var service = new MarcaAutoService(context);
        var dto = new UpdateMarcaAutoDto { Nombre = "Mitsubishi" };

        var result = await service.UpdateAsync(3, dto);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenDuplicateNameExists()
    {
        using var context = await SeedWithDataAsync(nameof(UpdateAsync_ReturnsNull_WhenDuplicateNameExists));
        var service = new MarcaAutoService(context);
        var dto = new UpdateMarcaAutoDto { Nombre = "Ford" };

        var result = await service.UpdateAsync(1, dto);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_AllowsSameName_WhenUpdatingOwnRecord()
    {
        using var context = await SeedWithDataAsync(nameof(UpdateAsync_AllowsSameName_WhenUpdatingOwnRecord));
        var service = new MarcaAutoService(context);
        var dto = new UpdateMarcaAutoDto { Nombre = "Toyota" };

        var result = await service.UpdateAsync(1, dto);

        Assert.NotNull(result);
        Assert.Equal("Toyota", result.Nombre);
    }

    [Fact]
    public async Task DeleteAsync_SoftDeletesRecord()
    {
        using var context = await SeedWithDataAsync(nameof(DeleteAsync_SoftDeletesRecord));
        var service = new MarcaAutoService(context);

        var result = await service.DeleteAsync(1);

        Assert.True(result);
        var marca = await context.MarcasAutos.FindAsync(1);
        Assert.False(marca!.Activo);
        Assert.NotNull(marca.FechaEliminacion);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenRecordNotFound()
    {
        using var context = await SeedWithDataAsync(nameof(DeleteAsync_ReturnsFalse_WhenRecordNotFound));
        var service = new MarcaAutoService(context);

        var result = await service.DeleteAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenRecordAlreadyInactive()
    {
        using var context = await SeedWithDataAsync(nameof(DeleteAsync_ReturnsFalse_WhenRecordAlreadyInactive));
        var service = new MarcaAutoService(context);

        var result = await service.DeleteAsync(3);

        Assert.False(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_AfterSoftDelete()
    {
        using var context = await SeedWithDataAsync(nameof(GetByIdAsync_ReturnsNull_AfterSoftDelete));
        var service = new MarcaAutoService(context);

        await service.DeleteAsync(1);
        var result = await service.GetByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_DoesNotReturnDeletedRecords()
    {
        using var context = await SeedWithDataAsync(nameof(GetAllAsync_DoesNotReturnDeletedRecords));
        var service = new MarcaAutoService(context);

        await service.DeleteAsync(1);
        var result = await service.GetAllAsync();

        Assert.Single(result);
        Assert.DoesNotContain(result, m => m.Nombre == "Toyota");
    }
}
