using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data
{
    public class TypeshowData
    {
        [Key]
        public int idТип_Показа { get; set; }
        public string Название { get; set; }
        public int Добавочная_цена { get; set; }
    }
}
