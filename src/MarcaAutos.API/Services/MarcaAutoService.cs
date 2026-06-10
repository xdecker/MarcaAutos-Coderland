using MarcaAutos.API.Data;
using MarcaAutos.API.Models;
using MarcaAutos.API.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MarcaAutos.API.Services;

public class MarcaAutoService : IMarcaAutoService
{
    private readonly AppDbContext _context;

    public MarcaAutoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MarcaAutoDto>> GetAllAsync()
    {
        var marcas = await _context.MarcasAutos
            .Where(m => m.Activo)
            .ToListAsync();

        return marcas.Select(MapToDto);
    }

    public async Task<MarcaAutoDto?> GetByIdAsync(int id)
    {
        var marca = await _context.MarcasAutos
            .FirstOrDefaultAsync(m => m.Id == id && m.Activo);

        return marca is null ? null : MapToDto(marca);
    }

    public async Task<MarcaAutoDto?> CreateAsync(CreateMarcaAutoDto dto)
    {
        var existe = await _context.MarcasAutos
            .AnyAsync(m => m.Nombre.ToLower() == dto.Nombre.ToLower() && m.Activo);

        if (existe) return null;

        var marca = new MarcaAuto
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            PaisOrigen = dto.PaisOrigen,
            AnioFundacion = dto.AnioFundacion
        };

        _context.MarcasAutos.Add(marca);
        await _context.SaveChangesAsync();

        return MapToDto(marca);
    }

    public async Task<MarcaAutoDto?> UpdateAsync(int id, UpdateMarcaAutoDto dto)
    {
        var marca = await _context.MarcasAutos
            .FirstOrDefaultAsync(m => m.Id == id && m.Activo);

        if (marca is null) return null;

        var existe = await _context.MarcasAutos
            .AnyAsync(m => m.Nombre.ToLower() == dto.Nombre.ToLower() && m.Id != id && m.Activo);

        if (existe) return null;

        marca.Nombre = dto.Nombre;
        marca.Descripcion = dto.Descripcion;
        marca.PaisOrigen = dto.PaisOrigen;
        marca.AnioFundacion = dto.AnioFundacion;

        await _context.SaveChangesAsync();

        return MapToDto(marca);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var marca = await _context.MarcasAutos
            .FirstOrDefaultAsync(m => m.Id == id && m.Activo);

        if (marca is null) return false;

        marca.Activo = false;
        marca.FechaEliminacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    private static MarcaAutoDto MapToDto(MarcaAuto marca)
    {
        return new MarcaAutoDto
        {
            Id = marca.Id,
            Nombre = marca.Nombre,
            Descripcion = marca.Descripcion,
            PaisOrigen = marca.PaisOrigen,
            AnioFundacion = marca.AnioFundacion
        };
    }
}
