using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Migrations
{
    /// <inheritdoc />
    public partial class ProyParadigmas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jugadores",
                columns: table => new
                {
                    JugadorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    CreadoUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jugadores", x => x.JugadorId);
                });

            migrationBuilder.CreateTable(
                name: "Movimientos",
                columns: table => new
                {
                    MovimientoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartidaId = table.Column<int>(type: "int", nullable: false),
                    JugadorId = table.Column<int>(type: "int", nullable: false),
                    IndicePrimero = table.Column<int>(type: "int", nullable: false),
                    IndiceSegundo = table.Column<int>(type: "int", nullable: false),
                    FuePareja = table.Column<bool>(type: "bit", nullable: false),
                    CreadoUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimientos", x => x.MovimientoId);
                    table.ForeignKey(
                        name: "FK_Movimientos_Jugadores_JugadorId",
                        column: x => x.JugadorId,
                        principalTable: "Jugadores",
                        principalColumn: "JugadorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartidaJugadores",
                columns: table => new
                {
                    PartidaId = table.Column<int>(type: "int", nullable: false),
                    JugadorId = table.Column<int>(type: "int", nullable: false),
                    OrdenTurno = table.Column<int>(type: "int", nullable: false),
                    Puntaje = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartidaJugadores", x => new { x.PartidaId, x.JugadorId });
                    table.ForeignKey(
                        name: "FK_PartidaJugadores_Jugadores_JugadorId",
                        column: x => x.JugadorId,
                        principalTable: "Jugadores",
                        principalColumn: "JugadorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Partidas",
                columns: table => new
                {
                    PartidaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalaId = table.Column<int>(type: "int", nullable: false),
                    GanadorId = table.Column<int>(type: "int", nullable: true),
                    PuntajeGanador = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Dificultad = table.Column<int>(type: "int", nullable: false),
                    Filas = table.Column<int>(type: "int", nullable: false),
                    Columnas = table.Column<int>(type: "int", nullable: false),
                    JugadorActualId = table.Column<int>(type: "int", nullable: true),
                    NumeroTurno = table.Column<int>(type: "int", nullable: false),
                    IndicePrimerVolteo = table.Column<int>(type: "int", nullable: true),
                    DuracionSegundos = table.Column<int>(type: "int", nullable: false),
                    IniciadaUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PuntosPorPareja = table.Column<int>(type: "int", nullable: false),
                    FinalizadaUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partidas", x => x.PartidaId);
                    table.ForeignKey(
                        name: "FK_Partidas_Jugadores_GanadorId",
                        column: x => x.GanadorId,
                        principalTable: "Jugadores",
                        principalColumn: "JugadorId");
                    table.ForeignKey(
                        name: "FK_Partidas_Jugadores_JugadorActualId",
                        column: x => x.JugadorActualId,
                        principalTable: "Jugadores",
                        principalColumn: "JugadorId");
                });

            migrationBuilder.CreateTable(
                name: "Salas",
                columns: table => new
                {
                    SalaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoIngreso = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    AnfitrionId = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    MaxJugadores = table.Column<int>(type: "int", nullable: false),
                    PartidaActualId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salas", x => x.SalaId);
                    table.ForeignKey(
                        name: "FK_Salas_Jugadores_AnfitrionId",
                        column: x => x.AnfitrionId,
                        principalTable: "Jugadores",
                        principalColumn: "JugadorId");
                    table.ForeignKey(
                        name: "FK_Salas_Partidas_PartidaActualId",
                        column: x => x.PartidaActualId,
                        principalTable: "Partidas",
                        principalColumn: "PartidaId");
                });

            migrationBuilder.CreateTable(
                name: "Tablero",
                columns: table => new
                {
                    CartaTableroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartidaId = table.Column<int>(type: "int", nullable: false),
                    Indice = table.Column<int>(type: "int", nullable: false),
                    ClavePareja = table.Column<int>(type: "int", nullable: false),
                    EstaEmparejada = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tablero", x => x.CartaTableroId);
                    table.ForeignKey(
                        name: "FK_Tablero_Partidas_PartidaId",
                        column: x => x.PartidaId,
                        principalTable: "Partidas",
                        principalColumn: "PartidaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalaJugadores",
                columns: table => new
                {
                    SalaId = table.Column<int>(type: "int", nullable: false),
                    JugadorId = table.Column<int>(type: "int", nullable: false),
                    OrdenTurno = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaJugadores", x => new { x.SalaId, x.JugadorId });
                    table.ForeignKey(
                        name: "FK_SalaJugadores_Jugadores_JugadorId",
                        column: x => x.JugadorId,
                        principalTable: "Jugadores",
                        principalColumn: "JugadorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalaJugadores_Salas_SalaId",
                        column: x => x.SalaId,
                        principalTable: "Salas",
                        principalColumn: "SalaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_JugadorId",
                table: "Movimientos",
                column: "JugadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_PartidaId",
                table: "Movimientos",
                column: "PartidaId");

            migrationBuilder.CreateIndex(
                name: "IX_PartidaJugadores_JugadorId",
                table: "PartidaJugadores",
                column: "JugadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidas_GanadorId",
                table: "Partidas",
                column: "GanadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidas_JugadorActualId",
                table: "Partidas",
                column: "JugadorActualId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidas_SalaId",
                table: "Partidas",
                column: "SalaId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaJugadores_JugadorId",
                table: "SalaJugadores",
                column: "JugadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Salas_AnfitrionId",
                table: "Salas",
                column: "AnfitrionId");

            migrationBuilder.CreateIndex(
                name: "IX_Salas_CodigoIngreso",
                table: "Salas",
                column: "CodigoIngreso",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Salas_PartidaActualId",
                table: "Salas",
                column: "PartidaActualId");

            migrationBuilder.CreateIndex(
                name: "IX_Tablero_PartidaId_Indice",
                table: "Tablero",
                columns: new[] { "PartidaId", "Indice" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Movimientos_Partidas_PartidaId",
                table: "Movimientos",
                column: "PartidaId",
                principalTable: "Partidas",
                principalColumn: "PartidaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartidaJugadores_Partidas_PartidaId",
                table: "PartidaJugadores",
                column: "PartidaId",
                principalTable: "Partidas",
                principalColumn: "PartidaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Partidas_Salas_SalaId",
                table: "Partidas",
                column: "SalaId",
                principalTable: "Salas",
                principalColumn: "SalaId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partidas_Jugadores_GanadorId",
                table: "Partidas");

            migrationBuilder.DropForeignKey(
                name: "FK_Partidas_Jugadores_JugadorActualId",
                table: "Partidas");

            migrationBuilder.DropForeignKey(
                name: "FK_Salas_Jugadores_AnfitrionId",
                table: "Salas");

            migrationBuilder.DropForeignKey(
                name: "FK_Salas_Partidas_PartidaActualId",
                table: "Salas");

            migrationBuilder.DropTable(
                name: "Movimientos");

            migrationBuilder.DropTable(
                name: "PartidaJugadores");

            migrationBuilder.DropTable(
                name: "SalaJugadores");

            migrationBuilder.DropTable(
                name: "Tablero");

            migrationBuilder.DropTable(
                name: "Jugadores");

            migrationBuilder.DropTable(
                name: "Partidas");

            migrationBuilder.DropTable(
                name: "Salas");
        }
    }
}
