﻿namespace WebApplication1.Db
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; } = string.Empty;
        public double Latitude  { get; set; }
        public double Longitude { get; set; }

        public List<LocationParameter> LocationParameters { get; set; } = null!;

        public Location(string name, double latitude, double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
