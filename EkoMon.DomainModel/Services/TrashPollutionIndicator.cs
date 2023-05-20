using EkoMon.DomainModel.Db;
using EkoMon.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
namespace EkoMon.DomainModel.Services
{
    public class TrashPollutionIndicator
    {
        public const int CategoryId = 5;
        private readonly EntityContext entityContext;

        public TrashPollutionIndicator(EntityContext entityContext)
        {
            this.entityContext = entityContext;
        }

        public IndicatorModel Calculate(int locationId)
        {
            var concentrationKoefTuples = new List<(double, double)>();
            var location = entityContext.Locations.First(i => i.Id == locationId);
            var locationParametersByCategoryId = entityContext.LocationParameters.Where(l => l.LocationId == locationId).Include(p => p.Parameter).Where(p => p.Parameter.Koef.HasValue).Where(p => p.Parameter.CategoryId == CategoryId).AsEnumerable().GroupBy(p => p.ParameterId).ToDictionary(k => k.Key, v => v.ToList());

            foreach ((_, var value) in locationParametersByCategoryId)
            {
                var lastActual = value.OrderByDescending(d => d.DateTime).First();
                concentrationKoefTuples.Add((lastActual.Value, lastActual.Parameter.Koef!.Value));
            }

            double pollutionIndex = 0;
            foreach ((var concentration, var koef) in concentrationKoefTuples)
                pollutionIndex += concentration * koef;
            pollutionIndex /= location.Area;

            var pollutionClass = DeterminePollutionClass(pollutionIndex);

            return new IndicatorModel()
            {
                CategoryId = CategoryId,
                Value = pollutionIndex,
                Rank = pollutionClass,
            };
        }

        private IndexRank DeterminePollutionClass(double pollutionIndex)
        {
            return pollutionIndex switch
            {
                // Оцінювальна шкала забруднення
                <= 0.5 => IndexRank.VeryGood,
                <= 0.8 => IndexRank.Good,
                <= 1 => IndexRank.Medium,
                <= 1.5 => IndexRank.Bad,
                _ => IndexRank.VeryBad
            };
        }



    }
}
