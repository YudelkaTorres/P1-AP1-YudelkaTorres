using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace P1_AP1_YudelkaTorres.Migrations
{
    /// <inheritdoc />
    public partial class Detalle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoId",
                table: "EntradasHuacales",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EntradasHuacalesTipos",
                columns: table => new
                {
                    TipoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descripción = table.Column<string>(type: "TEXT", nullable: false),
                    Existencia = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntradasHuacalesTipos", x => x.TipoId);
                });

            migrationBuilder.CreateTable(
                name: "EntradasHuacalesDetalle",
                columns: table => new
                {
                    DetalleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntradaId = table.Column<int>(type: "INTEGER", nullable: false),
                    TipoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Cantidad = table.Column<int>(type: "INTEGER", nullable: false),
                    Precio = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntradasHuacalesDetalle", x => x.DetalleId);
                    table.ForeignKey(
                        name: "FK_EntradasHuacalesDetalle_EntradasHuacalesTipos_TipoId",
                        column: x => x.TipoId,
                        principalTable: "EntradasHuacalesTipos",
                        principalColumn: "TipoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntradasHuacalesDetalle_EntradasHuacales_EntradaId",
                        column: x => x.EntradaId,
                        principalTable: "EntradasHuacales",
                        principalColumn: "EntradaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EntradasHuacalesTipos",
                columns: new[] { "TipoId", "Descripción", "Existencia" },
                values: new object[,]
                {
                    { 1, "Huacal Verde", 4 },
                    { 2, "Huacal Rojo", 5 },
                    { 3, "Huacal Azul", 3 },
                    { 4, "Huacal Amarrillo", 6 },
                    { 5, "Huacal Negro", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntradasHuacalesDetalle_EntradaId",
                table: "EntradasHuacalesDetalle",
                column: "EntradaId");

            migrationBuilder.CreateIndex(
                name: "IX_EntradasHuacalesDetalle_TipoId",
                table: "EntradasHuacalesDetalle",
                column: "TipoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntradasHuacalesDetalle");

            migrationBuilder.DropTable(
                name: "EntradasHuacalesTipos");

            migrationBuilder.DropColumn(
                name: "TipoId",
                table: "EntradasHuacales");
        }
    }
}
