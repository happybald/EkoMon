using EkoMon.DomainModel.Db;
namespace EkoMon.WebApp.ApiModels
{
    public class LocationShortModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Latitude  { get; set; }
        public double Longitude { get; set; }

        public LocationShortModel()
        {
        }
        public LocationShortModel(Location location)
        {
            Id = location.Id;
            Title = location.Title;
            Latitude = location.Latitude;
            Longitude = location.Longitude;
        }
    }
}
