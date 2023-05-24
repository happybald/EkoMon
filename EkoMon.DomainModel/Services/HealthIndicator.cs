using EkoMon.DomainModel.Db;
using EkoMon.DomainModel.Models;
namespace EkoMon.DomainModel.Services
{
    public class HealthIndicator
    {
        public const int CategoryId = 7;
        private readonly EntityContext entityContext;

        public HealthIndicator(EntityContext entityContext)
        {
            this.entityContext = entityContext;
        }


        public IndicatorModel? Calculate(int locationId)
        {
            var population = entityContext.LocationParameters.Where(l => l.LocationId == locationId).Where(p => p.ParameterId == 58).OrderByDescending(d => d.DateTime).FirstOrDefault();
            var disabledCount = entityContext.LocationParameters.Where(l => l.LocationId == locationId).Where(p => p.ParameterId == 59).OrderByDescending(d => d.DateTime).FirstOrDefault();
            var chronicDiseaseCount = entityContext.LocationParameters.Where(l => l.LocationId == locationId).Where(p => p.ParameterId == 60).OrderByDescending(d => d.DateTime).FirstOrDefault();

            if (population == null && (disabledCount == null || chronicDiseaseCount == null))
            {
                return null;
            }

            double healthyCount = population!.Value;
            if (disabledCount != null)
                healthyCount -= disabledCount.Value;
            if (chronicDiseaseCount != null)
                healthyCount -= chronicDiseaseCount.Value;

            double healthIndex = healthyCount / population.Value * 100;

            var pollutionIndex = healthIndex;

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
                >= 80 => IndexRank.VeryGood,
                >= 60 => IndexRank.Good,
                >= 40 => IndexRank.Medium,
                >= 20 => IndexRank.Bad,
                _ => IndexRank.VeryBad
            };
        }


    }
}
