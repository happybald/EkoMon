using EkoMon.DomainModel.Db;
namespace EkoMon.WebApp.ApiModels
{
    public class LocationModel : LocationShortModel
    {
        public string Address { get; set; }
        
        public List<LocationParameterModel> LocationParameters { get; set; }

        public LocationModel()
        {
        }
        public LocationModel(Location location) : base(location)
        {
            Address = location.Address;
            LocationParameters = location.LocationParameters.Select(o => new LocationParameterModel(o)).ToList();
        }
    }
}
