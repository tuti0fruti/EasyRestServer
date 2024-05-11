namespace WebApplication1.Models
{
    public class SessionRequest
    {
        public DateTime Дата_Сеанса { get; set; }
        public string Время_Начала_Сеанса { get; set; }
        public int Фильмы_idФильмы { get; set; }
        public int Зал_idЗал { get; set; }
        public int Тип_сеанса { get; set; }
    }
}
