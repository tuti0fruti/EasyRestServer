namespace WebApplication1.Models
{
    public class TicketsaleRequest
    {
        public string email { get; set; }
        public int sale { get; set; }
        public List<int> seats { get; set; }
        public int session { get; set; }
    }
}
