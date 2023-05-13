namespace WebApplication1.Db
{
    public class LocationParameter
    {
        public int Id { get; set; }
        
        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;
    
        public int ParameterId { get; set; }
        public Parameter Parameter{ get; set; } = null!;
        
        public double Value { get; set; }
        
    }
}
