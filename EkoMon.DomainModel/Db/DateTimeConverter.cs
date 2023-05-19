using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace EkoMon.DomainModel.Db
{
    internal class DateTimeConverter : ValueConverter<DateTime, DateTime>
    {
        public DateTimeConverter() : base(x => x.ToUniversalTime(), x => DateTime.SpecifyKind(x, DateTimeKind.Utc))
        {
        }
    }
}
