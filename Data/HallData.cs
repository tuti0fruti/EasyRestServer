using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data
{
    public class HallData
    {
        [Key]
        public int idЗал { get; set; }
        public string Описание_Зала { get; set; }
        public int Тип_Показа_idТип_Показа { get; set; }
    }
}
