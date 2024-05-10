using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Data
{
    public class SessionData
    {
        [Key]
        public int idСеанса { get; set; }
        public DateTime Дата_Сеанса { get; set; }
        public TimeOnly Время_Начала_Сеанса { get; set; }
        public int Фильмы_idФильмы { get; set; }
        public int Зал_idЗал { get; set; }
    }
}
