using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data
{
    public class CountryData
    {
        [Key]
        public int idСтраны { get; set; }
        public string Название { get; set; }
    }
}
