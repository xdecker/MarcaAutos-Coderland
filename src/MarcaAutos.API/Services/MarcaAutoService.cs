using MarcaAutos.API.Data;
using MarcaAutos.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MarcaAutos.API.Services;

public class MarcaAutoService : IMarcaAutoService
{
    private readonly AppDbContext _context;

    public MarcaAutoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MarcaAuto>> GetAllAsync()
    {
        return await _context.MarcasAutos.ToListAsync();
    }
}
