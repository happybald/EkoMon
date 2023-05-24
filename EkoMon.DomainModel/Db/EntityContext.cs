using Microsoft.EntityFrameworkCore;
namespace EkoMon.DomainModel.Db
{
    public class EntityContext : DbContext
    {
        public EntityContext(DbContextOptions<EntityContext> options) : base(options) {}
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<Unit> Units { get; set; } = null!;
        public DbSet<Parameter> Parameters { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<LocationParameter> LocationParameters { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category("Стан повітря") { Id = 1 },
                new Category("Стан водних ресурсів") { Id = 2 },
                new Category("Стан ґрунтів") { Id = 3 },
                new Category("Рівень радіації") { Id = 4 },
                new Category("Відходи") { Id = 5 },
                new Category("Економічний стан") { Id = 6 },
                new Category("Стан здоров’я населення") { Id = 7 },
                new Category("Енергетичний стан") { Id = 8 }
            );
            modelBuilder.Entity<Unit>().HasData(
                new Unit("мг/м³") { Id = 1 },
                new Unit("мл") { Id = 2 },
                new Unit("мг/екв/л") { Id = 3 },
                new Unit("мг/л") { Id = 4 },
                new Unit("мг/кг") { Id = 5 },
                new Unit("нЗв/год") { Id = 6 },
                new Unit("кг/день") { Id = 7 },
                new Unit("тис. грн") { Id = 8 },
                new Unit("чол.") { Id = 9 },
                new Unit("м³") { Id = 10 },
                new Unit("кВт·год") { Id = 11 }
            );

            modelBuilder.Entity<Parameter>().HasData(
                new Parameter("Вміст пилу") { Id = 1, CategoryId = 1, UnitId = 1, Limit = 5.0 },
                new Parameter("Азоту оксид") { Id = 2, CategoryId = 1, UnitId = 1, Limit = 0.06 },
                new Parameter("Амонію сульфат") { Id = 3, CategoryId = 1, UnitId = 1, Limit = 0.1 },
                new Parameter("Оксид вуглецю") { Id = 4, CategoryId = 1, UnitId = 1, Limit = 3.0 },
                new Parameter("Формальдегід") { Id = 5, CategoryId = 1, UnitId = 1, Limit = 0.003 },
                new Parameter("Свинець") { Id = 6, CategoryId = 1, UnitId = 1, Limit = 0.0003 },
                new Parameter("Бенз(а)пірен") { Id = 7, CategoryId = 1, UnitId = 1, Limit = 0.0001 },
                new Parameter("Мікробне число") { Id = 8, CategoryId = 2, UnitId = null, Limit = 100 },
                new Parameter("Колі-індекс") { Id = 9, CategoryId = 2, UnitId = null, Limit = 3 },
                new Parameter("Колі-титр") { Id = 10, CategoryId = 2, UnitId = 2, Limit = 300.0 },
                new Parameter("Твердість") { Id = 11, CategoryId = 2, UnitId = 3, Limit = 7.0 },
                new Parameter("Щільний осадок") { Id = 12, CategoryId = 2, UnitId = 4, Limit = 1000.0 },
                new Parameter("Залізо") { Id = 13, CategoryId = 2, UnitId = 4, Limit = 0.3 },
                new Parameter("Сульфати") { Id = 14, CategoryId = 2, UnitId = 4, Limit = 500.0 },
                new Parameter("Хлориди") { Id = 15, CategoryId = 2, UnitId = 4, Limit = 350.0 },
                new Parameter("Мідь") { Id = 16, CategoryId = 2, UnitId = 4, Limit = 1.0 },
                new Parameter("Марганець") { Id = 17, CategoryId = 2, UnitId = 4, Limit = 5.0 },
                new Parameter("Фосфати") { Id = 18, CategoryId = 2, UnitId = 4, Limit = 0.1 },
                new Parameter("Нітрати") { Id = 19, CategoryId = 2, UnitId = 4, Limit = 10.0 },
                new Parameter("Нітрити") { Id = 20, CategoryId = 2, UnitId = 4, Limit = 0.002 },
                new Parameter("Фтор") { Id = 21, CategoryId = 2, UnitId = 4, Limit = 1.5 },
                new Parameter("Свинець") { Id = 22, CategoryId = 2, UnitId = 4, Limit = 0.03 },
                new Parameter("Миш’як") { Id = 23, CategoryId = 2, UnitId = 4, Limit = 0.05 },
                new Parameter("Ртуть") { Id = 24, CategoryId = 2, UnitId = 4, Limit = 0.005 },
                new Parameter("Ціаніди") { Id = 25, CategoryId = 2, UnitId = 4, Limit = 0.1 },
                new Parameter("Алюміній") { Id = 26, CategoryId = 2, UnitId = 4, Limit = 0.1 },
                new Parameter("Молібден") { Id = 27, CategoryId = 2, UnitId = 4, Limit = 3.5 },
                new Parameter("Селен") { Id = 28, CategoryId = 2, UnitId = 4, Limit = 0.001 },
                new Parameter("Стронцій") { Id = 29, CategoryId = 2, UnitId = 4, Limit = 0.7 },
                new Parameter("Берилій") { Id = 30, CategoryId = 2, UnitId = 4, Limit = 0.0002 },
                new Parameter("pH") { Id = 31, CategoryId = 2, UnitId = null, Limit = 8.0 },
                new Parameter("Бенз(а)пірен") { Id = 32, CategoryId = 3, UnitId = 5, Limit = 0.02 },
                new Parameter("Бензол") { Id = 33, CategoryId = 3, UnitId = 5, Limit = 0.3 },
                new Parameter("Марганець") { Id = 34, CategoryId = 3, UnitId = 5, Limit = 1500.0 },
                new Parameter("Ртуть") { Id = 35, CategoryId = 3, UnitId = 5, Limit = 2.1 },
                new Parameter("Свинець") { Id = 36, CategoryId = 3, UnitId = 5, Limit = 32.0 },
                new Parameter("Сірка") { Id = 37, CategoryId = 3, UnitId = 5, Limit = 160.0 },
                new Parameter("Сірководень") { Id = 38, CategoryId = 3, UnitId = 5, Limit = 0.4 },
                new Parameter("Кадмій") { Id = 39, CategoryId = 3, UnitId = 5, Limit = 1.5 },
                new Parameter("Нітрати") { Id = 40, CategoryId = 3, UnitId = 5, Limit = 130.0 },
                new Parameter("Концентрація продуктів ядерного розпаду") { Id = 41, CategoryId = 4, UnitId = 6 },
                new Parameter("Відходи виробництва") { Id = 42, CategoryId = 5, UnitId = 7, Koef = 0.8 },
                new Parameter("Гірничі породи") { Id = 43, CategoryId = 5, UnitId = 7, Koef = 0.9 },
                new Parameter("Залишкові продукти первинної обробки сировини") { Id = 44, CategoryId = 5, UnitId = 7, Koef = 0.7 },
                new Parameter("Новоутворені речовини та їх суміші") { Id = 45, CategoryId = 5, UnitId = 7, Koef = 0.6 },
                new Parameter("Залишкові продукти сільськогосподарського виробництва") { Id = 46, CategoryId = 5, UnitId = 7, Koef = 0.5 },
                new Parameter("Бракована та некондиційна продукція") { Id = 47, CategoryId = 5, UnitId = 7, Koef = 0.6 },
                new Parameter("Неідентифікована продукція") { Id = 48, CategoryId = 5, UnitId = 7, Koef = 0.4 },
                new Parameter("Відходи споживання") { Id = 49, CategoryId = 5, UnitId = 7, Koef = 0.3 },
                new Parameter("Побутові відходи") { Id = 50, CategoryId = 5, UnitId = 7, Koef = 0.2 },
                new Parameter("Осади очисних споруд") { Id = 51, CategoryId = 5, UnitId = 7, Koef = 0.7 },
                new Parameter("Залишки від медичного та ветеринарного обслуговування") { Id = 52, CategoryId = 5, UnitId = 7, Koef = 0.8 },
                new Parameter("Залишкові продукти інших видів діяльності") { Id = 53, CategoryId = 5, UnitId = 7, Koef = 0.6 },
                new Parameter("Радіоактивні відходи") { Id = 54, CategoryId = 5, UnitId = 7, Koef = 1 },
                new Parameter("Експорт товарів та послуг") { Id = 55, CategoryId = 6, UnitId = 8 },
                new Parameter("Імпорт товарів та послуг") { Id = 56, CategoryId = 6, UnitId = 8 },
                new Parameter("Заробітна плата") { Id = 57, CategoryId = 6, UnitId = 8 },
                new Parameter("Кількість людей") { Id = 58, CategoryId = 7, UnitId = 9 },
                new Parameter("Кількість інвалідів") { Id = 59, CategoryId = 7, UnitId = 9 },
                new Parameter("Кількість хронічних хвороб") { Id = 60, CategoryId = 7, UnitId = 9 },
                new Parameter("Обсяги використання води") { Id = 61, CategoryId = 8, UnitId = 10 },
                new Parameter("Обсяги використання електроенергії") { Id = 62, CategoryId = 8, UnitId = 11 },
                new Parameter("Обсяги використання газу") { Id = 63, CategoryId = 8, UnitId = 10 },
                new Parameter("Обсяги використання теплової енергії") { Id = 64, CategoryId = 8, UnitId = 11 }
            );

        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveConversion<DateTimeConverter>();
        }
    }
}
