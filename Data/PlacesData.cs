using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data
{
    public class PlacesData
    {
        [Key]
        public int idМеста { get; set; }
        public int Номер_Ряда { get; set; }
        public int Номер_Места { get; set; }
        public int Зал_idЗал { get; set; }
    }
}
