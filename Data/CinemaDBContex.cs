using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;

namespace WebApplication1.Data
{
    public class CinemaDBContex : DbContext
    {
        public CinemaDBContex(DbContextOptions<CinemaDBContex> options) : base(options)
        {

        }

        //выбираем таблицу которую используем
        public DbSet<FilmsData> Фильмы { get; set; }
        public DbSet<TypeNameData> Жанры { get; set; }
        public DbSet<TypeData> Жанры_Фильмы { get; set; }
        public DbSet<CountryData> Страны { get; set; }
        public DbSet<SessionData> Сеансы { get; set; }
        public DbSet<HallData> Зал { get; set; }
        public DbSet<TypeshowData> Тип_Показа { get; set; }
        public DbSet<TicketsaleData> Продажа_Билета { get; set; }
        public DbSet<DiscountsData> Скидки { get; set; }

        
    }
}