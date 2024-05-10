using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    public class price_time
    {
        public price_time(TimeOnly time, int price) 
        { 
            this.time = time;
            this.price = price;
        } 
        public TimeOnly time { get; set; }
        public int price { get; set; }
    }
    public class Films_answer
    {
        public Films_answer(string films_name, string films_duration, string films_type, string films_country, List<price_time> price_time) 
        { 
            this.films_name = films_name;
            this.films_country = films_country;
            this.films_duration = films_duration;
            this.films_type = films_type;

            this.price_time = price_time;
        }
        public string films_name { get; set; }
        public string films_country { get; set; }
        public string films_duration { get; set; }
        public string films_type { get; set;}
        public List<price_time> price_time { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        //Получение доступа к БД
        private CinemaDBContex _cimenaDBContex;

        public FilmsController(CinemaDBContex cinemaDBContex)
        {
            _cimenaDBContex = cinemaDBContex;
        }

        //Запрос на получение данных
        [HttpGet("GetFilmsNow/{Id}")]
        public IActionResult Get([FromRoute]int Id)
        {
            try
            {
                List<FilmsData> films = _cimenaDBContex.Фильмы.ToList();
                List<TypeData> type = _cimenaDBContex.Жанры_Фильмы.ToList();
                List<TypeNameData> type_name = _cimenaDBContex.Жанры.ToList();
                List<CountryData> country = _cimenaDBContex.Страны.ToList();
                List<SessionData> session = _cimenaDBContex.Сеансы.ToList();
               
                List<HallData> hall = _cimenaDBContex.Зал.ToList();
                List<TypeshowData> typeShow = _cimenaDBContex.Тип_Показа.ToList();
                List<TicketsaleData> TicketSale = _cimenaDBContex.Продажа_Билета.ToList();
                List<DiscountsData> Discount = _cimenaDBContex.Скидки.ToList();
                
                //дописать проверки --------------
                //проверка если нету пользователей
                if (films.Count == 0)
                {
                    return StatusCode(404, "No films found");
                }

                List<Films_answer> films_today = new List<Films_answer>();
                List<price_time> price_time = new List<price_time>();


                foreach (var film in films)
                {
                    string type_ = string.Empty;
                    string country_ = string.Empty;

                    
                    bool flagSession = false;

                    //прибавляю добавочную цену
                    foreach (var session1 in session)
                    {
                        int price = film.Стоимость_Проката;

                        foreach (var item in hall)
                        {
                            if (session1.Зал_idЗал == item.idЗал)
                            {
                                foreach (var item1 in typeShow)
                                {
                                    if (item.Тип_Показа_idТип_Показа == item1.idТип_Показа)
                                    {
                                        price += item1.Добавочная_цена;
                                    }
                                    break;
                                }
                                break;
                            }
                        }

                        //вычитаем скидку
                        foreach (var item in TicketSale)
                        {
                            if (session1.idСеанса == item.Сеансы_idСеанса)
                            {
                                foreach (var item1 in Discount)
                                {
                                    if (item.Скидки_idСкидка == item1.idСкидка)
                                    {
                                        price -= price * (item1.Процент_Скидки / 100);
                                    }
                                }
                               
                            }
                        }

                        price_time.Add(
                            new price_time(session1.Время_Начала_Сеанса, price)
                            );

                        //Проверка есть сегодня сеансы или нет
                        if (film.idФильмы == session1.Фильмы_idФильмы)
                        {
                            if (session1.Дата_Сеанса.ToShortDateString() == DateTime.Now.AddDays(Id).ToShortDateString())
                            {
                                flagSession = true;
                                break;
                            }
                        }
                    }
                    
                    if (!flagSession)
                    {
                        continue;
                    }

                    foreach (var item in country)
                    {
                        if (film.Страны_idСтраны == item.idСтраны)
                        {
                            country_ = item.Название;
                            break;
                        }
                    }

                    foreach (var item in type)
                    {
                        if (film.idФильмы == item.Фильмы_idФильмы)
                        {
                            foreach (var item1 in type_name)
                            {
                                if (item.Жанры_idЖанры == item1.idЖанры)
                                {
                                    type_ += item1.Название + " ";
                                }
                            }
                        }
                    }

                    films_today.Add(
                                new Films_answer(film.Название, film.Продолжительность.ToString(), type_, country_, price_time)
                                );
                }
                
                if (films_today.Count == 0)
                {
                    return StatusCode(404, "No films today found");
                }
                string json = JsonConvert.SerializeObject(films_today);

                return new JsonResult(films_today);
            }
            catch (Exception)
            {
                //в случае неудачи вернем ошибку 500 - внутрения ошибка сервера
                return StatusCode(500, "An error has occurred");
            }
        }
    }
}
