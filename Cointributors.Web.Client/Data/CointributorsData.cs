namespace Cointributors.Web.Client.Data
{
    public enum DestinationType
    {
        User,
        Dependency
    }

    public class CointributorsData
    {
        public string AvatarUrl { get; set; }
        public string Address { get; set; }

        public List<CointributorsAllocationData> Allocations { get; set; }
    }

    public class CointributorsAllocationData
    {
        public string Destination { get; set; }
        public DestinationType Type { get; set; }
        public string AvatarUrl { get; set; }
        public string? Address { get; set; }
        public int Allocation { get; set; }
    }
}
