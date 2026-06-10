using MarcaAutos.API.Models;

namespace MarcaAutos.API.Services;

public interface IMarcaAutoService
{
    Task<IEnumerable<MarcaAuto>> GetAllAsync();
}
