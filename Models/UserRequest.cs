namespace WebApplication1.Models
{
    public class UserRequest
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }

        public string State { get; set; }
        public string Country { get; set; }
    }
}
