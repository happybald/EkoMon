namespace EkoMon.DomainModel.Db
{
    public class LocationParameter
    {
        public int Id { get; set; }
        
        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;
    
        public int ParameterId { get; set; }
        public Parameter Parameter{ get; set; } = null!;
        
        public string Value { get; set; }
        
    }
}
