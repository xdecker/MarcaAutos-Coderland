using MarcaAutos.API.Controllers;
using MarcaAutos.API.Models.DTOs;
using MarcaAutos.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MarcaAutos.Tests;

public class MarcasAutosControllerTests
{
    private readonly Mock<IMarcaAutoService> _mockService;
    private readonly MarcasAutosController _controller;

    public MarcasAutosControllerTests()
    {
        _mockService = new Mock<IMarcaAutoService>();
        _controller = new MarcasAutosController(_mockService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithList()
    {
        var marcas = new List<MarcaAutoDto>
        {
            new() { Id = 1, Nombre = "Toyota" },
            new() { Id = 2, Nombre = "Ford" }
        };
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(marcas);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var data = Assert.IsAssignableFrom<IEnumerable<MarcaAutoDto>>(okResult.Value);
        Assert.Equal(2, data.Count());
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyList_WhenNoData()
    {
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync([]);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var data = Assert.IsAssignableFrom<IEnumerable<MarcaAutoDto>>(okResult.Value);
        Assert.Empty(data);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        var marca = new MarcaAutoDto { Id = 1, Nombre = "Toyota" };
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(marca);

        var result = await _controller.GetById(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var data = Assert.IsType<MarcaAutoDto>(okResult.Value);
        Assert.Equal("Toyota", data.Nombre);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotFound()
    {
        _mockService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((MarcaAutoDto?)null);

        var result = await _controller.GetById(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenSuccessful()
    {
        var dto = new CreateMarcaAutoDto { Nombre = "Honda" };
        var created = new MarcaAutoDto { Id = 1, Nombre = "Honda" };
        _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

        var result = await _controller.Create(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(1, ((MarcaAutoDto)createdResult.Value!).Id);
    }

    [Fact]
    public async Task Create_ReturnsConflict_WhenDuplicateName()
    {
        var dto = new CreateMarcaAutoDto { Nombre = "Toyota" };
        _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync((MarcaAutoDto?)null);

        var result = await _controller.Create(dto);

        var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.Equal("Ya existe una marca con ese nombre", conflictResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsOk_WhenSuccessful()
    {
        var dto = new UpdateMarcaAutoDto { Nombre = "Toyota Updated" };
        var updated = new MarcaAutoDto { Id = 1, Nombre = "Toyota Updated" };
        _mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(updated);
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(updated);

        var result = await _controller.Update(1, dto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal("Toyota Updated", ((MarcaAutoDto)okResult.Value!).Nombre);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenRecordDoesNotExist()
    {
        var dto = new UpdateMarcaAutoDto { Nombre = "Nuevo" };
        _mockService.Setup(s => s.UpdateAsync(999, dto)).ReturnsAsync((MarcaAutoDto?)null);
        _mockService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((MarcaAutoDto?)null);

        var result = await _controller.Update(999, dto);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Update_ReturnsConflict_WhenDuplicateName()
    {
        var dto = new UpdateMarcaAutoDto { Nombre = "Ford" };
        _mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync((MarcaAutoDto?)null);
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new MarcaAutoDto { Id = 1 });

        var result = await _controller.Update(1, dto);

        var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.Equal("Ya existe otra marca con ese nombre", conflictResult.Value);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenSuccessful()
    {
        _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenNotExists()
    {
        _mockService.Setup(s => s.DeleteAsync(999)).ReturnsAsync(false);

        var result = await _controller.Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }
}
