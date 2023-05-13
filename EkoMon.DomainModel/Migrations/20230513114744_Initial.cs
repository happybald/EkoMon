using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EkoMon.DomainModel.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    UnitId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parameters_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LocationParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    ParameterId = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationParameters_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationParameters_Parameters_ParameterId",
                        column: x => x.ParameterId,
                        principalTable: "Parameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Address", "Latitude", "Longitude", "Title" },
                values: new object[,]
                {
                    { 1, "", 50.449538255595499, 30.525422556705447, "місто Київ" },
                    { 2, "", 50.454813999999999, 30.635262999999998, "Київський автомобільний ремонтний завод" },
                    { 3, "", 50.471031886528294, 30.512898573607249, "Станція для визначення рівня забруднення атмосферного повітря" }
                });

            migrationBuilder.InsertData(
                table: "Parameters",
                columns: new[] { "Id", "Title", "UnitId" },
                values: new object[] { 7, "AQI PM2.5", null });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "мкг/м³" },
                    { 2, "°C" },
                    { 3, "%" },
                    { 4, "гПа" }
                });

            migrationBuilder.InsertData(
                table: "LocationParameters",
                columns: new[] { "Id", "LocationId", "ParameterId", "Value" },
                values: new object[] { 7, 3, 7, 13.0 });

            migrationBuilder.InsertData(
                table: "Parameters",
                columns: new[] { "Id", "Title", "UnitId" },
                values: new object[,]
                {
                    { 1, "PM1", 1 },
                    { 2, "PM2.5", 1 },
                    { 3, "PM10", 1 },
                    { 4, "Температура", 2 },
                    { 5, "Відносна вологість", 3 },
                    { 6, "Атмосферний тиск", 4 }
                });

            migrationBuilder.InsertData(
                table: "LocationParameters",
                columns: new[] { "Id", "LocationId", "ParameterId", "Value" },
                values: new object[,]
                {
                    { 1, 3, 1, 2.5 },
                    { 2, 3, 2, 3.1000000000000001 },
                    { 3, 3, 3, 5.7000000000000002 },
                    { 4, 3, 4, 5.5 },
                    { 5, 3, 5, 57.700000000000003 },
                    { 6, 3, 6, 1006.7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationParameters_LocationId",
                table: "LocationParameters",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationParameters_ParameterId",
                table: "LocationParameters",
                column: "ParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_UnitId",
                table: "Parameters",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationParameters");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Parameters");

            migrationBuilder.DropTable(
                name: "Units");
        }
    }
}
