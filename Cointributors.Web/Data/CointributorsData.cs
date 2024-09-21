namespace Cointributors.Web.Data
{
    public enum DestinationType
    {
        User,
        Dependency
    }

    public class CointributorsData
    {
        public string Address { get; set; }

        public List<CointributorsAllocationData> Allocations { get; set; }
    }

    public class CointributorsAllocationData
    {
        public string Address { get; set; }
        public DestinationType Type { get; set; }
        public int Allocation { get; set; }
    }
}
