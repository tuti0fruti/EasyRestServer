using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data
{
    public class TypeNameData
    {
        [Key]
        public int idЖанры { get; set; }
        public string Название { get; set; }
    }
}
