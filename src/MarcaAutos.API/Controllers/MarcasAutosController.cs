using MarcaAutos.API.Models;
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
    public async Task<ActionResult<IEnumerable<MarcaAuto>>> GetAll()
    {
        var marcas = await _service.GetAllAsync();
        return Ok(marcas);
    }
}
