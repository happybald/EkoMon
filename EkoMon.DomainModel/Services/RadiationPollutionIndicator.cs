using EkoMon.DomainModel.Db;
using EkoMon.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
namespace EkoMon.DomainModel.Services
{
    public class RadiationPollutionIndicator
    {
        public const int CategoryId = 4;
        private readonly EntityContext entityContext;

        public RadiationPollutionIndicator(EntityContext entityContext)
        {
            this.entityContext = entityContext;
        }

        public IndicatorModel? Calculate(int locationId)
        {
            var radiationParameter = entityContext.LocationParameters
                .Where(l => l.LocationId == locationId)
                .Include(p => p.Parameter)
                .Where(p => p.Parameter.CategoryId == CategoryId)
                .OrderByDescending(d => d.DateTime)
                .FirstOrDefault();

            if (radiationParameter == null)
                return null;
            var pollutionIndex = radiationParameter.Value;

            var pollutionClass = DeterminePollutionClass(pollutionIndex);

            return new IndicatorModel()
            {
                CategoryId = CategoryId,
                Value = pollutionIndex.ToString("F2"),
                Rank = pollutionClass,
            };
        }

        private IndexRank DeterminePollutionClass(double pollutionIndex)
        {
            return pollutionIndex switch
            {
                // Оцінювальна шкала забруднення
                <= 0.15 => IndexRank.VeryGood,
                <= 0.3 => IndexRank.Good,
                <= 0.6 => IndexRank.Medium,
                <= 1.2 => IndexRank.Bad,
                _ => IndexRank.VeryBad
            };
        }



    }
}
