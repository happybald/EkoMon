using System.ComponentModel.DataAnnotations;
namespace EkoMon.DomainModel.Db
{
    public class Location
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; } = string.Empty;
        /*Площа в м²*/
        public int Area { get; set; }
        public double Latitude  { get; set; }
        public double Longitude { get; set; }

        public List<LocationParameter> LocationParameters { get; set; } = null!;

        public Location(string title, double latitude, double longitude, int area)
        {
            Title = title;
            Latitude = latitude;
            Longitude = longitude;
            Area = area;
        }
    }
}
