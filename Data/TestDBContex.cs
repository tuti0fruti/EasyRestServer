using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data
{
    public class TestDBContex : DbContext
    {
        public TestDBContex(DbContextOptions<TestDBContex> options) : base(options)
        {
                
        }

        //выбираем таблицу которую используем
        public DbSet<tbUser> tbUsers { get; set; }


    }
}
