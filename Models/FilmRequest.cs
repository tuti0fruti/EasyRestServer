using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data
{
    public class FilmsRequest
    {
        public int idФильмы { get; set; }
        public string Название { get; set; }
        public int Продолжительность { get; set; }
        public string Название_Страны { get; set; }
        public string Описание { get; set; }
        public int Возрастное_ограничение { get; set; }
        public DateTime Дата_Проката { get; set; }
        public int Стоимость_Проката { get; set; }

    }
}