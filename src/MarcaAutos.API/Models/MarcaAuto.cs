namespace MarcaAutos.API.Models;

public class MarcaAuto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? PaisOrigen { get; set; }
    public int? AnioFundacion { get; set; }
}
