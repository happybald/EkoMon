using Microsoft.EntityFrameworkCore;
namespace WebApplication1.Db
{
    public class EntityContext : DbContext
    {
        public EntityContext(DbContextOptions<EntityContext> options) : base(options) {}
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<Unit> Units { get; set; } = null!;
        public DbSet<Parameter> Parameters { get; set; } = null!;
        public DbSet<LocationParameter> LocationParameters { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Location>().HasData(
                new Location("місто Київ", 50.4495382555955, 30.525422556705447) { Id = 1 },
                new Location("Київський автомобільний ремонтний завод", 50.454814, 30.635263) { Id = 2 },
                new Location("Станція для визначення рівня забруднення атмосферного повітря", 50.471031886528294, 30.51289857360725) { Id = 3 }
            );
            modelBuilder.Entity<Unit>().HasData(new Unit("мкг/м³") { Id = 1 }, new Unit("°C") { Id = 2 }, new Unit("%") { Id = 3 }, new Unit("гПа") { Id = 4 });
            modelBuilder.Entity<Parameter>().HasData(
                new Parameter("PM1") { Id = 1, UnitId = 1 },
                new Parameter("PM2.5") { Id = 2, UnitId = 1 },
                new Parameter("PM10") { Id = 3, UnitId = 1 },
                new Parameter("Температура") { Id = 4, UnitId = 2 },
                new Parameter("Відносна вологість") { Id = 5, UnitId = 3 },
                new Parameter("Атмосферний тиск") { Id = 6, UnitId = 4 },
                new Parameter("AQI PM2.5") { Id = 7 }
            );

            modelBuilder.Entity<LocationParameter>().HasData(
                new LocationParameter() { Id = 1, LocationId = 3, ParameterId = 1, Value = 2.5 },
                new LocationParameter() { Id = 2, LocationId = 3, ParameterId = 2, Value = 3.1 },
                new LocationParameter() { Id = 3, LocationId = 3, ParameterId = 3, Value = 5.7 },
                new LocationParameter() { Id = 4, LocationId = 3, ParameterId = 4, Value = 5.5 },
                new LocationParameter() { Id = 5, LocationId = 3, ParameterId = 5, Value = 57.7 },
                new LocationParameter() { Id = 6, LocationId = 3, ParameterId = 6, Value = 1006.7 },
                new LocationParameter() { Id = 7, LocationId = 3, ParameterId = 7, Value = 13 }
            );
        }
    }
}
