using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaneOrbis.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_ORB_USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_USUARIO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DS_EMAIL = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DS_SENHA_HASH = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    DT_CADASTRO = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ORB_USUARIO", x => x.ID_USUARIO);
                });

            migrationBuilder.CreateTable(
                name: "T_ORB_PROPRIEDADE",
                columns: table => new
                {
                    ID_PROPRIEDADE = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NM_PROPRIEDADE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DS_LOCALIZACAO = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: true),
                    VL_AREA_HECTARE = table.Column<decimal>(type: "NUMBER(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ORB_PROPRIEDADE", x => x.ID_PROPRIEDADE);
                    table.ForeignKey(
                        name: "FK_T_ORB_PROPRIEDADE_T_ORB_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "T_ORB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_ORB_TALHAO",
                columns: table => new
                {
                    ID_TALHAO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_PROPRIEDADE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NM_TALHAO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    VL_AREA_HECTARE = table.Column<decimal>(type: "NUMBER(10,2)", nullable: true),
                    DS_TIPO_SOLO = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: true),
                    DS_CULTURA = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false, defaultValue: "CANA_DE_ACUCAR")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ORB_TALHAO", x => x.ID_TALHAO);
                    table.ForeignKey(
                        name: "FK_T_ORB_TALHAO_T_ORB_PROPRIEDADE_ID_PROPRIEDADE",
                        column: x => x.ID_PROPRIEDADE,
                        principalTable: "T_ORB_PROPRIEDADE",
                        principalColumn: "ID_PROPRIEDADE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_ORB_DISPOSITIVO_IOT",
                columns: table => new
                {
                    ID_DISPOSITIVO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_TALHAO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DS_MAC_ADDRESS = table.Column<string>(type: "NVARCHAR2(17)", maxLength: 17, nullable: false),
                    NM_APELIDO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    VL_LATITUDE = table.Column<decimal>(type: "NUMBER(10,8)", nullable: true),
                    VL_LONGITUDE = table.Column<decimal>(type: "NUMBER(11,8)", nullable: true),
                    DS_STATUS_DISPOSITIVO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false, defaultValue: "ATIVO"),
                    DT_INSTALACAO = table.Column<DateTime>(type: "DATE", nullable: false, defaultValueSql: "SYSDATE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ORB_DISPOSITIVO_IOT", x => x.ID_DISPOSITIVO);
                    table.ForeignKey(
                        name: "FK_T_ORB_DISPOSITIVO_IOT_T_ORB_TALHAO_ID_TALHAO",
                        column: x => x.ID_TALHAO,
                        principalTable: "T_ORB_TALHAO",
                        principalColumn: "ID_TALHAO",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    DT_COLETA = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ORB_DADO_SATELITE", x => x.ID_DADO_SATELITE);
                    table.ForeignKey(
                        name: "FK_T_ORB_DADO_SATELITE_T_ORB_DISPOSITIVO_IOT_ID_DISPOSITIVO",
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
                    DT_LEITURA = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ORB_LEITURA_SENSOR", x => x.ID_LEITURA);
                    table.ForeignKey(
                        name: "FK_T_ORB_LEITURA_SENSOR_T_ORB_DISPOSITIVO_IOT_ID_DISPOSITIVO",
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
                name: "IX_T_ORB_DISPOSITIVO_IOT_DS_MAC_ADDRESS",
                table: "T_ORB_DISPOSITIVO_IOT",
                column: "DS_MAC_ADDRESS",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_ORB_DISPOSITIVO_IOT_ID_TALHAO",
                table: "T_ORB_DISPOSITIVO_IOT",
                column: "ID_TALHAO");

            migrationBuilder.CreateIndex(
                name: "IX_T_ORB_LEITURA_SENSOR_ID_DISPOSITIVO",
                table: "T_ORB_LEITURA_SENSOR",
                column: "ID_DISPOSITIVO");

            migrationBuilder.CreateIndex(
                name: "IX_T_ORB_PROPRIEDADE_ID_USUARIO",
                table: "T_ORB_PROPRIEDADE",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_T_ORB_TALHAO_ID_PROPRIEDADE",
                table: "T_ORB_TALHAO",
                column: "ID_PROPRIEDADE");

            migrationBuilder.CreateIndex(
                name: "IX_T_ORB_USUARIO_DS_EMAIL",
                table: "T_ORB_USUARIO",
                column: "DS_EMAIL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_ORB_DADO_SATELITE");

            migrationBuilder.DropTable(
                name: "T_ORB_LEITURA_SENSOR");

            migrationBuilder.DropTable(
                name: "T_ORB_DISPOSITIVO_IOT");

            migrationBuilder.DropTable(
                name: "T_ORB_TALHAO");

            migrationBuilder.DropTable(
                name: "T_ORB_PROPRIEDADE");

            migrationBuilder.DropTable(
                name: "T_ORB_USUARIO");
        }
    }
}
