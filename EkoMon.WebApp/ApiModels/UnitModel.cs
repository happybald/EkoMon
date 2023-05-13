using EkoMon.DomainModel.Db;
namespace EkoMon.WebApp.ApiModels
{
    public class UnitModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public UnitModel()
        {
        }
        public UnitModel(Unit unit)
        {
            Id = unit.Id;
            Title = unit.Title;
        }
    }
}
