namespace EkoMon.DomainModel.Db
{
    public class LocationParameter
    {
        public int Id { get; set; }
        
        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;
    
        public int ParameterId { get; set; }
        public Parameter Parameter{ get; set; } = null!;
        
        public double Value { get; set; }

        public DateTime DateTime { get; set; } = DateTime.UtcNow;


        public LocationParameter()
        {
        }
        public LocationParameter(int parameterId, double value)
        {
            ParameterId = parameterId;
            Value = value;
        }
        
        public LocationParameter(Parameter parameter, double value)
        {
            Parameter = parameter;
            ParameterId = parameter.Id;
            Value = value;
        }

    }
}
