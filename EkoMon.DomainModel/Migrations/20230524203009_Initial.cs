using System;
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
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Area = table.Column<int>(type: "integer", nullable: false),
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
                    UnitId = table.Column<int>(type: "integer", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Limit = table.Column<double>(type: "double precision", nullable: true),
                    Koef = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parameters_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Стан повітря" },
                    { 2, "Стан водних ресурсів" },
                    { 3, "Стан ґрунтів" },
                    { 4, "Рівень радіації" },
                    { 5, "Відходи" },
                    { 6, "Економічний стан" },
                    { 7, "Стан здоров’я населення" },
                    { 8, "Енергетичний стан" }
                });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "мг/м³" },
                    { 2, "мл" },
                    { 3, "мг/екв/л" },
                    { 4, "мг/л" },
                    { 5, "мг/кг" },
                    { 6, "нЗв/год" },
                    { 7, "кг/день" },
                    { 8, "тис. грн" },
                    { 9, "чол." },
                    { 10, "м³" },
                    { 11, "кВт·год" }
                });

            migrationBuilder.InsertData(
                table: "Parameters",
                columns: new[] { "Id", "CategoryId", "Koef", "Limit", "Title", "UnitId" },
                values: new object[,]
                {
                    { 1, 1, null, 5.0, "Вміст пилу", 1 },
                    { 2, 1, null, 0.059999999999999998, "Азоту оксид", 1 },
                    { 3, 1, null, 0.10000000000000001, "Амонію сульфат", 1 },
                    { 4, 1, null, 3.0, "Оксид вуглецю", 1 },
                    { 5, 1, null, 0.0030000000000000001, "Формальдегід", 1 },
                    { 6, 1, null, 0.00029999999999999997, "Свинець", 1 },
                    { 7, 1, null, 0.0001, "Бенз(а)пірен", 1 },
                    { 8, 2, null, 100.0, "Мікробне число", null },
                    { 9, 2, null, 3.0, "Колі-індекс", null },
                    { 10, 2, null, 300.0, "Колі-титр", 2 },
                    { 11, 2, null, 7.0, "Твердість", 3 },
                    { 12, 2, null, 1000.0, "Щільний осадок", 4 },
                    { 13, 2, null, 0.29999999999999999, "Залізо", 4 },
                    { 14, 2, null, 500.0, "Сульфати", 4 },
                    { 15, 2, null, 350.0, "Хлориди", 4 },
                    { 16, 2, null, 1.0, "Мідь", 4 },
                    { 17, 2, null, 5.0, "Марганець", 4 },
                    { 18, 2, null, 0.10000000000000001, "Фосфати", 4 },
                    { 19, 2, null, 10.0, "Нітрати", 4 },
                    { 20, 2, null, 0.002, "Нітрити", 4 },
                    { 21, 2, null, 1.5, "Фтор", 4 },
                    { 22, 2, null, 0.029999999999999999, "Свинець", 4 },
                    { 23, 2, null, 0.050000000000000003, "Миш’як", 4 },
                    { 24, 2, null, 0.0050000000000000001, "Ртуть", 4 },
                    { 25, 2, null, 0.10000000000000001, "Ціаніди", 4 },
                    { 26, 2, null, 0.10000000000000001, "Алюміній", 4 },
                    { 27, 2, null, 3.5, "Молібден", 4 },
                    { 28, 2, null, 0.001, "Селен", 4 },
                    { 29, 2, null, 0.69999999999999996, "Стронцій", 4 },
                    { 30, 2, null, 0.00020000000000000001, "Берилій", 4 },
                    { 31, 2, null, 8.0, "pH", null },
                    { 32, 3, null, 0.02, "Бенз(а)пірен", 5 },
                    { 33, 3, null, 0.29999999999999999, "Бензол", 5 },
                    { 34, 3, null, 1500.0, "Марганець", 5 },
                    { 35, 3, null, 2.1000000000000001, "Ртуть", 5 },
                    { 36, 3, null, 32.0, "Свинець", 5 },
                    { 37, 3, null, 160.0, "Сірка", 5 },
                    { 38, 3, null, 0.40000000000000002, "Сірководень", 5 },
                    { 39, 3, null, 1.5, "Кадмій", 5 },
                    { 40, 3, null, 130.0, "Нітрати", 5 },
                    { 41, 4, null, null, "Концентрація продуктів ядерного розпаду", 6 },
                    { 42, 5, 0.80000000000000004, null, "Відходи виробництва", 7 },
                    { 43, 5, 0.90000000000000002, null, "Гірничі породи", 7 },
                    { 44, 5, 0.69999999999999996, null, "Залишкові продукти первинної обробки сировини", 7 },
                    { 45, 5, 0.59999999999999998, null, "Новоутворені речовини та їх суміші", 7 },
                    { 46, 5, 0.5, null, "Залишкові продукти сільськогосподарського виробництва", 7 },
                    { 47, 5, 0.59999999999999998, null, "Бракована та некондиційна продукція", 7 },
                    { 48, 5, 0.40000000000000002, null, "Неідентифікована продукція", 7 },
                    { 49, 5, 0.29999999999999999, null, "Відходи споживання", 7 },
                    { 50, 5, 0.20000000000000001, null, "Побутові відходи", 7 },
                    { 51, 5, 0.69999999999999996, null, "Осади очисних споруд", 7 },
                    { 52, 5, 0.80000000000000004, null, "Залишки від медичного та ветеринарного обслуговування", 7 },
                    { 53, 5, 0.59999999999999998, null, "Залишкові продукти інших видів діяльності", 7 },
                    { 54, 5, 1.0, null, "Радіоактивні відходи", 7 },
                    { 55, 6, null, null, "Експорт товарів та послуг", 8 },
                    { 56, 6, null, null, "Імпорт товарів та послуг", 8 },
                    { 57, 6, null, null, "Заробітна плата", 8 },
                    { 58, 7, null, null, "Кількість людей", 9 },
                    { 59, 7, null, null, "Кількість інвалідів", 9 },
                    { 60, 7, null, null, "Кількість хронічних хвороб", 9 },
                    { 61, 8, null, null, "Обсяги використання води", 10 },
                    { 62, 8, null, null, "Обсяги використання електроенергії", 11 },
                    { 63, 8, null, null, "Обсяги використання газу", 10 },
                    { 64, 8, null, null, "Обсяги використання теплової енергії", 11 }
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
                name: "IX_Parameters_CategoryId",
                table: "Parameters",
                column: "CategoryId");

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
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Units");
        }
    }
}
