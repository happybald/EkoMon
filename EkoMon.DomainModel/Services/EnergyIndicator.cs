using EkoMon.DomainModel.Db;
using EkoMon.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
namespace EkoMon.DomainModel.Services
{
    public class EnergyIndicator
    {
        public const int CategoryId = 8;
        private readonly EntityContext entityContext;

        public EnergyIndicator(EntityContext entityContext)
        {
            this.entityContext = entityContext;
        }


        public IndicatorModel? Calculate(int locationId)
        {
            var water = entityContext.LocationParameters.Where(l => l.LocationId == locationId).Where(p => p.ParameterId == 61).OrderByDescending(d => d.DateTime).ToList();
            var electricity = entityContext.LocationParameters.Where(l => l.LocationId == locationId).Where(p => p.ParameterId == 62).OrderByDescending(d => d.DateTime).ToList();
            var gas = entityContext.LocationParameters.Where(l => l.LocationId == locationId).Where(p => p.ParameterId == 63).OrderByDescending(d => d.DateTime).ToList();
            var heat = entityContext.LocationParameters.Where(l => l.LocationId == locationId).Where(p => p.ParameterId == 64).OrderByDescending(d => d.DateTime).ToList();

            var indexes = CalculateIndex(water, electricity, gas, heat);

            return new IndicatorModel()
            {
                CategoryId = CategoryId,
                Value = string.Join(",", indexes.Select(i=>$"{i.Key}: {i.Value:F2}")),
                Rank = IndexRank.Medium,
            };
        }
        private Dictionary<string, double> CalculateIndex(List<LocationParameter> water, List<LocationParameter> electricity, List<LocationParameter> gas, List<LocationParameter> heat)
        {
            var averageWater = water.Any() ? water.Average(v => v.Value) : 0;
            var averageElectricity = electricity.Any() ? electricity.Average(v => v.Value) : 0;
            var averageGas = gas.Any() ? gas.Average(v => v.Value) : 0;
            var averageHeat = heat.Any() ? heat.Average(v => v.Value) : 0;
            return new Dictionary<string, double>() { { "W", averageWater }, { "E", averageElectricity }, { "G", averageGas }, { "H", averageHeat } };
        }
    }
}
