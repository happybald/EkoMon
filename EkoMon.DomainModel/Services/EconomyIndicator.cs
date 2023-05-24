using EkoMon.DomainModel.Db;
using EkoMon.DomainModel.Models;
namespace EkoMon.DomainModel.Services
{
    public class EconomyIndicator
    {
        public const int CategoryId = 6;
        private readonly EntityContext entityContext;

        public EconomyIndicator(EntityContext entityContext)
        {
            this.entityContext = entityContext;
        }


        public IndicatorModel? Calculate(int locationId)
        {
            var export = entityContext.LocationParameters.Where(l => l.LocationId == locationId).Where(p => p.ParameterId == 55).OrderByDescending(d => d.DateTime).FirstOrDefault();
            var import = entityContext.LocationParameters.Where(l => l.LocationId == locationId).Where(p => p.ParameterId == 56).OrderByDescending(d => d.DateTime).FirstOrDefault();
            var salary = entityContext.LocationParameters.Where(l => l.LocationId == locationId).Where(p => p.ParameterId == 57).OrderByDescending(d => d.DateTime).FirstOrDefault();

            if (export == null || import == null || salary == null)
                return null;

            var pollutionIndex = (export.Value - import.Value) / salary.Value;

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
                >= 2 => IndexRank.VeryGood,
                >= 1.5 => IndexRank.Good,
                >= 1 => IndexRank.Medium,
                >= 0.5 => IndexRank.Bad,
                _ => IndexRank.VeryBad
            };
        }


       
    }
}
