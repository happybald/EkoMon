namespace WebApplication1.Db
{
    public class Parameter
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? UnitId { get; set; }
        public Unit? Unit { get; set; }
        public List<LocationParameter> LocationParameters { get; set; } = null!;
        public Parameter(string title)
        {
            Title = title;
        }
    }
}
