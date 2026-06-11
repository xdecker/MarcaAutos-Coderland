using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarcaAutos.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class _02_AddAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "MarcasAutos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActualizacion",
                table: "MarcasAutos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "MarcasAutos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEliminacion",
                table: "MarcasAutos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Activo", "FechaActualizacion", "FechaCreacion", "FechaEliminacion" },
                values: new object[] { true, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null });

            migrationBuilder.UpdateData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Activo", "FechaActualizacion", "FechaCreacion", "FechaEliminacion" },
                values: new object[] { true, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null });

            migrationBuilder.UpdateData(
                table: "MarcasAutos",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Activo", "FechaActualizacion", "FechaCreacion", "FechaEliminacion" },
                values: new object[] { true, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "MarcasAutos");

            migrationBuilder.DropColumn(
                name: "FechaActualizacion",
                table: "MarcasAutos");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "MarcasAutos");

            migrationBuilder.DropColumn(
                name: "FechaEliminacion",
                table: "MarcasAutos");
        }
    }
}
