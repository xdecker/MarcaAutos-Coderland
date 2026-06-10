namespace MarcaAutos.API.Models.DTOs;

public class UpdateMarcaAutoDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? PaisOrigen { get; set; }
    public int? AnioFundacion { get; set; }
}
