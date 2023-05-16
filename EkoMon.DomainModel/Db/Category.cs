using System.Reflection.Metadata.Ecma335;
namespace EkoMon.DomainModel.Db
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Parameter> Parameters { get; set; }
    }
}
