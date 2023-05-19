using EkoMon.DomainModel.Db;
namespace EkoMon.WebApp.ApiModels
{
    public class LocationModel : LocationShortModel
    {
        public string Address { get; set; }

        public List<GroupedLocationParameterModel> GroupedLocationParameters { get; set; } = new();

        public LocationModel()
        {
        }
        public LocationModel(Location location) : base(location)
        {
            Address = location.Address;
            var allParameters = location.LocationParameters.Select(p => p.Parameter).DistinctBy(i => i.Id);
            var locationParametersByParameterId = location.LocationParameters.GroupBy(p => p.ParameterId).ToDictionary(k => k.Key, v => v.ToList());
            foreach (var parameter in allParameters)
            {
                GroupedLocationParameters.Add(new GroupedLocationParameterModel()
                {
                    Parameter = new ParameterModel(parameter),
                    LocationParameters = locationParametersByParameterId[parameter.Id].Select(o => new LocationParameterModel(o)).OrderByDescending(d=>d.DateTime).ToList(),
                });
            }

        }
    }
}
