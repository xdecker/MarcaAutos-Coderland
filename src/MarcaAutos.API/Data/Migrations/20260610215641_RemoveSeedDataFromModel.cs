using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MarcaAutos.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSeedDataFromModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MarcasAutos",
                columns: new[] { "Id", "Activo", "AnioFundacion", "Descripcion", "FechaActualizacion", "FechaCreacion", "FechaEliminacion", "Nombre", "PaisOrigen" },
                values: new object[,]
                {
                    { 1, true, 1937, "Marca japonesa", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Toyota", "Japón" },
                    { 2, true, 1903, "Marca estadounidense", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ford", "Estados Unidos" },
                    { 3, true, 1937, "Marca alemana", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Volkswagen", "Alemania" }
                });
        }
    }
}
