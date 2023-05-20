using EkoMon.DomainModel.Models;
namespace EkoMon.DomainModel.Services
{
    public class GdkIndicator
    {
        protected double CalculateIndex(List<(double, double)> concentrationLimitTuples)
        {
            double pollutionIndex = 0;
            foreach ((var concentration, var limit) in concentrationLimitTuples)
                pollutionIndex += concentration / limit;

            return pollutionIndex;
        }

        protected IndexRank DeterminePollutionClass(double pollutionIndex)
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
