namespace EkoMon.DomainModel.Db
{
    public class Unit
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Parameter> Parameters { get; set; } = null!;
    }
}
