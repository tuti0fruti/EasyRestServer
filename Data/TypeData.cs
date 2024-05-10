using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data
{
    public class TypeData
    {
        [Key]
        public int id { get; set; }
        public int Жанры_idЖанры { get; set; }
        public int Фильмы_idФильмы { get; set; }
        
    }
}
