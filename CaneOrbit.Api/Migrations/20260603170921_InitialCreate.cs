using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaneOrbis.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_ORB_DADO_SATELITE",
                columns: table => new
                {
                    ID_DADO_SATELITE = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_DISPOSITIVO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    VL_NDVI = table.Column<decimal>(type: "NUMBER(10,4)", nullable: true),
                    VL_PRECIPITACAO = table.Column<decimal>(type: "NUMBER(10,2)", nullable: true),
                    VL_TEMPERATURA_AR = table.Column<decimal>(type: "NUMBER(10,2)", nullable: true),
                    DS_CONDICAO_CLIMA = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    DT_COLETA = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ORB_DADO_SATELITE", x => x.ID_DADO_SATELITE);
                    table.ForeignKey(
                        name: "FK_ORB_DADO_SATELITE_DISPOSITIVO",
                        column: x => x.ID_DISPOSITIVO,
                        principalTable: "T_ORB_DISPOSITIVO_IOT",
                        principalColumn: "ID_DISPOSITIVO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_ORB_LEITURA_SENSOR",
                columns: table => new
                {
                    ID_LEITURA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_DISPOSITIVO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    VL_UMIDADE_SOLO = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false),
                    VL_TEMPERATURA = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false),
                    VL_PH_SOLO = table.Column<decimal>(type: "NUMBER(4,2)", nullable: true),
                    DT_LEITURA = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ORB_LEITURA_SENSOR", x => x.ID_LEITURA);
                    table.ForeignKey(
                        name: "FK_ORB_LEITURA_SENSOR_DISPOSITIVO",
                        column: x => x.ID_DISPOSITIVO,
                        principalTable: "T_ORB_DISPOSITIVO_IOT",
                        principalColumn: "ID_DISPOSITIVO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_ORB_DADO_SATELITE_ID_DISPOSITIVO",
                table: "T_ORB_DADO_SATELITE",
                column: "ID_DISPOSITIVO");

            migrationBuilder.CreateIndex(
                name: "IX_T_ORB_LEITURA_SENSOR_ID_DISPOSITIVO",
                table: "T_ORB_LEITURA_SENSOR",
                column: "ID_DISPOSITIVO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_ORB_DADO_SATELITE");

            migrationBuilder.DropTable(
                name: "T_ORB_LEITURA_SENSOR");
        }
    }
}