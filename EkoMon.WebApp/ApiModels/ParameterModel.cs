using EkoMon.DomainModel.Db;
namespace EkoMon.WebApp.ApiModels
{
    public class ParameterModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public double? Limit { get; set; }
        public UnitModel? Unit { get; set; }
        public ParameterModel()
        {
        }
        public ParameterModel(Parameter parameter)
        {
            Id = parameter.Id;
            Title = parameter.Title;
            CategoryId = parameter.CategoryId;
            Limit = parameter.Limit;
            if (parameter.Unit != null)
                Unit = new UnitModel(parameter.Unit);
        }
    }
}
