namespace EkoMon.DomainModel.Db
{
    public class Parameter
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? UnitId { get; set; }
        public Unit? Unit { get; set; }
        public List<LocationParameter> LocationParameters { get; set; } = null!;
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public ParameterType Type { get; set; }
        public Parameter(string title)
        {
            Title = title;
        }
    }

    public enum ParameterType
    {
        Measurable = 0,
        Statistical = 1,
    }
}
