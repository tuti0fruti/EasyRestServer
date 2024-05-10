using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;

namespace WebApplication1.Data
{
    public class CinemaDBContex : DbContext
    {
        public CinemaDBContex(DbContextOptions<CinemaDBContex> options) : base(options)
        {

        }

        //�������� ������� ������� ����������
        public DbSet<FilmsData> ������ { get; set; }
        public DbSet<TypeNameData> ����� { get; set; }
        public DbSet<TypeData> �����_������ { get; set; }
        public DbSet<CountryData> ������ { get; set; }
        public DbSet<SessionData> ������ { get; set; }
        public DbSet<HallData> ��� { get; set; }
        public DbSet<TypeshowData> ���_������ { get; set; }
        public DbSet<TicketsaleData> �������_������ { get; set; }
        public DbSet<DiscountsData> ������ { get; set; }

        
    }
}