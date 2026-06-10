using MarcaAutos.API.Models.DTOs;

namespace MarcaAutos.API.Services;

public interface IMarcaAutoService
{
    Task<IEnumerable<MarcaAutoDto>> GetAllAsync();
    Task<MarcaAutoDto?> GetByIdAsync(int id);
    Task<MarcaAutoDto> CreateAsync(CreateMarcaAutoDto dto);
    Task<MarcaAutoDto?> UpdateAsync(int id, UpdateMarcaAutoDto dto);
    Task<bool> DeleteAsync(int id);
}
