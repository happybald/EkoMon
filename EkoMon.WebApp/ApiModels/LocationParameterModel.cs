using EkoMon.DomainModel.Db;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
namespace EkoMon.WebApp.ApiModels
{
    public class LocationParameterModel
    {
        public int Id { get; set; }
        public ParameterModel Parameter { get; set; }
        public double Value { get; set; }
        public DateTime DateTime { get; set; }
        public LocationParameterModel()
        {
        }
        public LocationParameterModel(LocationParameter locationParameter)
        {
            Id = locationParameter.Id;
            Parameter = new ParameterModel(locationParameter.Parameter);
            Value = locationParameter.Value;
            DateTime = locationParameter.DateTime.ToLocalTime();
        }
    }
}
