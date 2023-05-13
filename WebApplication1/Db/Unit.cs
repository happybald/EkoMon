namespace WebApplication1.Db
{
    public class Unit
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Parameter> Parameters { get; set; } = null!;
        public Unit(string title)
        {
            Title = title;
        }
    }
}
