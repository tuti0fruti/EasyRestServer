using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data
{
    public class TicketsaleData
    {
        [Key]
        public int idПродажа_Билета { get; set; }
        public DateTime Дата_Оплата { get; set; }
        public string Почта_Покупателя { get; set; }
        public int Скидки_idСкидка { get; set; }
        public int Места_idМеста { get; set; }
        public int Сеансы_idСеанса { get; set; }
    }
}
