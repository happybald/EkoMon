using Microsoft.EntityFrameworkCore;
namespace EkoMon.DomainModel.Db
{
    public class EntityContext : DbContext
    {
        public EntityContext(DbContextOptions<EntityContext> options) : base(options) {}
        
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<Unit> Units  { get; set; } = null!;
        public DbSet<Parameter> Parameters  { get; set; } = null!;
        public DbSet<LocationParameter> LocationParameters { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>().HasData(
                new Location("місто Київ", 50.4495382555955, 30.525422556705447) { Id = 1 },
                new Location("Київський автомобільний ремонтний завод", 50.454814, 30.635263) { Id = 2 }
            );
        }
    }
}
