using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data
{
    public class UsersData
    {
        [Key]
        public int id { get; set; }
        public string login_users { get; set; }
        public string password_users { get; set; }
        public string email { get; set; }
        public string role { get; set; }
    }
}
