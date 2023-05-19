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
                    Type = table.Column<int>(type: "integer", nullable: false)
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
                    { 2, "%" },
                    { 3, "мг/кг" },
                    { 4, "г/л" },
                    { 5, "тонн" },
                    { 6, "чол." },
                    { 7, "млн. дол." },
                    { 8, "грн." },
                    { 9, "м³" },
                    { 10, "кВт·год" },
                    { 11, "м³" },
                    { 12, "Гкал" },
                    { 13, "роки" }
                });

            migrationBuilder.InsertData(
                table: "Parameters",
                columns: new[] { "Id", "CategoryId", "Title", "Type", "UnitId" },
                values: new object[,]
                {
                    { 1, 1, "Вміст пилу", 0, 1 },
                    { 2, 1, "Двоокис азоту (NO2)", 0, 1 },
                    { 3, 1, "Двоокис сірки (SO2)", 0, 1 },
                    { 4, 1, "Оксид вуглецю", 0, 1 },
                    { 5, 1, "Формальдегід (H2CO)", 0, 1 },
                    { 6, 1, "Свинець", 0, 1 },
                    { 7, 1, "Бенз(а)пірен", 0, 1 },
                    { 8, 2, "Показники епідемічної безпеки (мікробіологічні, паразитарні)", 0, null },
                    { 9, 2, "Санітарно-хімічні (органолептичні, фізико-хімічні, санітарно-токсикологічні)", 0, null },
                    { 10, 2, "Радіаційні показники", 0, null },
                    { 11, 3, "Гумус", 0, 2 },
                    { 12, 3, "Рухомі сполуки фосфору (P2O5)", 0, 3 },
                    { 13, 3, "Рухомі сполуки калію (K2O)", 0, 3 },
                    { 14, 3, "Засоленість", 0, 4 },
                    { 15, 3, "Солонцюватість", 0, 4 },
                    { 16, 3, "Забруднення хімічними речовинами", 0, null },
                    { 17, 3, "pH", 0, null },
                    { 18, 4, "Концентрація продуктів ядерного розпаду з коротким часом життя", 0, null },
                    { 19, 4, "Концентрація продуктів ядерного розпаду з середнім часом життя", 0, null },
                    { 20, 5, "Клас небезпеки", 0, null },
                    { 21, 5, "Токсичність", 0, null },
                    { 22, 5, "Склад (вміст речовин)", 0, null },
                    { 23, 5, "Маса або об’єм", 0, null },
                    { 24, 6, "Валовий внутрішній продукт", 0, 8 },
                    { 25, 6, "Вантажообіг", 0, 5 },
                    { 26, 6, "Пасажирообіг", 0, 6 },
                    { 27, 6, "Експорт товарів та послуг", 0, 7 },
                    { 28, 6, "Імпорт товарів та послуг", 0, 7 },
                    { 29, 6, "Заробітна плата", 0, 8 },
                    { 30, 7, "Обсяги використання води", 0, 9 },
                    { 31, 7, "Електроенергія", 0, 10 },
                    { 32, 7, "Газ", 0, 11 },
                    { 33, 7, "Теплова енергія за кожен місяць", 0, 12 },
                    { 34, 1, "Індекс якості повітря", 1, null },
                    { 35, 2, "Індекс забрудненості води", 1, null },
                    { 36, 3, "Бал бонітету для складового грунту", 1, null },
                    { 37, 4, "Ймовірність критичної події", 1, null },
                    { 38, 4, "Ризики радіаційного впливу на довкілля та здоров'я населення", 1, null },
                    { 39, 6, "Індекс промислової продукції", 1, null },
                    { 40, 6, "Індекс обсягу сільськогосподарського виробництва", 1, null },
                    { 41, 6, "Індекс будівельної продукції", 1, null },
                    { 42, 6, "Індекс споживчих цін", 1, null },
                    { 43, 6, "Індекс цін виробників промислової продукції", 1, null },
                    { 44, 7, "Ризики захворювання", 1, null },
                    { 45, 7, "Прогноз захворювання", 1, null },
                    { 46, 7, "Прогноз тривалості життя", 1, 13 },
                    { 47, 8, "Середні обсяги споживання за місяць та рік", 1, null },
                    { 48, 8, "Енергоефективність будівлі або виробництва", 1, null }
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
