using EkoMon.DomainModel.Db;
using EkoMon.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
namespace EkoMon.DomainModel.Services
{
    public class EarthPollutionIndicator : GdkIndicator
    {
        public const int CategoryId = 3;
        private readonly EntityContext entityContext;

        public EarthPollutionIndicator(EntityContext entityContext)
        {
            this.entityContext = entityContext;
        }


        public IndicatorModel Calculate(int locationId)
        {
            var concentrationLimitTuples = new List<(double, double)>();
            var locationParametersByCategoryId = entityContext.LocationParameters.Where(l => l.LocationId == locationId).Include(p => p.Parameter).Where(p => p.Parameter.Limit.HasValue).Where(p => p.Parameter.CategoryId == CategoryId).AsEnumerable().GroupBy(p => p.ParameterId).ToDictionary(k => k.Key, v => v.ToList());

            foreach (var (_, value) in locationParametersByCategoryId)
            {
                var lastActual = value.OrderByDescending(d => d.DateTime).First();
                concentrationLimitTuples.Add((lastActual.Value, lastActual.Parameter.Limit!.Value));
            }

            var pollutionIndex = CalculateIndex(concentrationLimitTuples);

            var pollutionClass = DeterminePollutionClass(pollutionIndex);

            return new IndicatorModel()
            {
                CategoryId = CategoryId,
                Value = pollutionIndex,
                Rank = pollutionClass,
            };
        }


       
    }
}
