using System.Text.Json.Serialization;
namespace EkoMon.DomainModel.ParseModels
{
    public class Indicator
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("Measuring")]
        public List<int> Measuring { get; set; } = new();
    }

    public class Measurement
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("unit")]
        public Unit Unit { get; set; }

        [JsonPropertyName("limit")]
        public double? Limit { get; set; }
    }

    public class Root
    {
        [JsonPropertyName("Measurements")]
        public List<Measurement> Measurements { get; set; } = new();

        [JsonPropertyName("Indicators")]
        public List<Indicator> Indicators { get; set; } = new();
    }

    public class Unit
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
