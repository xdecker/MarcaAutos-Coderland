using MarcaAutos.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MarcaAutos.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<MarcaAuto> MarcasAutos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MarcaAuto>(entity =>
        {
            entity.ToTable("MarcasAutos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.PaisOrigen).HasMaxLength(100);
        });

        modelBuilder.Entity<MarcaAuto>().HasData(
            new MarcaAuto { Id = 1, Nombre = "Toyota", Descripcion = "Marca japonesa", PaisOrigen = "Japón", AnioFundacion = 1937 },
            new MarcaAuto { Id = 2, Nombre = "Ford", Descripcion = "Marca estadounidense", PaisOrigen = "Estados Unidos", AnioFundacion = 1903 },
            new MarcaAuto { Id = 3, Nombre = "Volkswagen", Descripcion = "Marca alemana", PaisOrigen = "Alemania", AnioFundacion = 1937 }
        );
    }
}
