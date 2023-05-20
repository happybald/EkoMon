namespace EkoMon.DomainModel.Models
{
    public class IndicatorModel
    {
        public int CategoryId { get; set; }
        public double Value { get; set; }
        public IndexRank Rank { get; set; }
    }
}
