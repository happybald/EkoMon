namespace EkoMon.WebApp.ApiModels
{
    public class GroupedLocationParameterModel
    {
        public ParameterModel Parameter { get; set; }
        public List<LocationParameterModel> LocationParameters { get; set; } = new();
    }
}
