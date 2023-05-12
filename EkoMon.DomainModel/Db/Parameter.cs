namespace EkoMon.DomainModel.Db
{
    public class Parameter
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; } = null!;
        public List<LocationParameter> LocationParameters { get; set; } = null!;
    }
}
