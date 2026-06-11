using MarcaAutos.API.Models.DTOs;
using MarcaAutos.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarcaAutos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarcasAutosController : ControllerBase
{
    private readonly IMarcaAutoService _service;

    public MarcasAutosController(IMarcaAutoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MarcaAutoDto>>> GetAll()
    {
        var marcas = await _service.GetAllAsync();
        return Ok(marcas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MarcaAutoDto>> GetById(int id)
    {
        var marca = await _service.GetByIdAsync(id);
        if (marca is null) return NotFound();
        return Ok(marca);
    }

    [HttpPost]
    public async Task<ActionResult<MarcaAutoDto>> Create(CreateMarcaAutoDto dto)
    {
        var marca = await _service.CreateAsync(dto);
        if (marca is null) return Conflict("Ya existe una marca con ese nombre");
        return CreatedAtAction(nameof(GetById), new { id = marca.Id }, marca);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MarcaAutoDto>> Update(int id, UpdateMarcaAutoDto dto)
    {
        var marca = await _service.UpdateAsync(id, dto);
        if (marca is null)
        {
            var existe = await _service.GetByIdAsync(id);
            return existe is null ? NotFound() : Conflict("Ya existe otra marca con ese nombre");
        }
        return Ok(marca);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
