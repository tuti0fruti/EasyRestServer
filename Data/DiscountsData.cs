using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data
{
    public class DiscountsData
    {
        [Key]
        public int idСкидка { get; set; }
        public string Название_Скидки { get; set; }
        public int Процент_Скидки { get; set; }
    }
}
