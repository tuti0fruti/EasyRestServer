using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    record Person(string Name, int Age);
    public class LoginModel
    {
        public string Login { get; set; }
        public string HashedPassword { get; set; }
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
                List<FilmsData> Films = _cimenaDBContex.Фильмы.ToList();
                List<TypeData> Type = _cimenaDBContex.Жанры_Фильмы.ToList();
                List<TypeNameData> TypeName = _cimenaDBContex.Жанры.ToList();
                List<CountryData> Country = _cimenaDBContex.Страны.ToList();
                List<SessionData> Session = _cimenaDBContex.Сеансы.ToList();
               
                List<HallData> Hall = _cimenaDBContex.Зал.ToList();
                List<TypeshowData> TypeShow = _cimenaDBContex.Тип_Показа.ToList();
                List<TicketsaleData> TicketSale = _cimenaDBContex.Продажа_Билета.ToList();
                List<DiscountsData> Discount = _cimenaDBContex.Скидки.ToList();
                
                
                //проверка если нету фильмов
                if (Films.Count == 0)
                {
                    return StatusCode(404, "No films found");
                }

                var filmSessionInfo = (from film_join in _cimenaDBContex.Фильмы
                                       join session_join in _cimenaDBContex.Сеансы on film_join.idФильмы equals session_join.Фильмы_idФильмы into filmSessions
                                       from session_join in filmSessions.DefaultIfEmpty()
                                       
                                       join country_join in _cimenaDBContex.Страны on film_join.Страны_idСтраны equals country_join.idСтраны
                                       join hall_join in _cimenaDBContex.Зал on session_join.Зал_idЗал equals hall_join.idЗал
                                       join showType_join in _cimenaDBContex.Тип_Показа on hall_join.Тип_Показа_idТип_Показа equals showType_join.idТип_Показа
                                       
                                       where session_join.Время_Начала_Сеанса != null && session_join.Дата_Сеанса.Date == DateTime.Now.AddDays(Id).Date
                                       group new { film_join, session_join, hall_join, showType_join, country_join }
                                       by new { film_join.Название, film_join.Продолжительность, session_join.Время_Начала_Сеанса, film_join.Стоимость_Проката, showType_join.Добавочная_цена, country_join.НазваниеСтраны }
                       into filmGroup
                                       select new
                                       {
                                           name = filmGroup.Key.Название,
                                           country = filmGroup.Key.НазваниеСтраны,
                                           dura = filmGroup.Key.Продолжительность,
                                           time_ = filmGroup.Key.Время_Начала_Сеанса,
                                           price_ = filmGroup.Key.Стоимость_Проката + filmGroup.Key.Добавочная_цена
                                       });

                var groupedSessions = filmSessionInfo.GroupBy(
                            session => new { session.name, session.country, session.dura },
                            (key, sessions) => new
                            {
                                key.name,
                                key.country,
                                key.dura,
                                time_1 = sessions.Select(s => s.time_).ToList(),
                                price_1 = sessions.Select(s => s.price_).ToList()
                            });

                return Ok(groupedSessions);
            }
            catch (Exception)
            {
                //в случае неудачи вернем ошибку 500 - внутрения ошибка сервера
                return StatusCode(500, "An error has occurred");
            }
        }
        
        [HttpGet("GetInformationNameFilm")]
        public IActionResult Get()
        {
            try
            {
                List<FilmsData> Films = _cimenaDBContex.Фильмы.ToList();
                
                List<string> filmInfoName = new List<string>();

                foreach (var item in Films)
                {
                    filmInfoName.Add(item.Название);
                }

                return Ok(filmInfoName);
            }
            catch (Exception)
            {
                //в случае неудачи вернем ошибку 500 - внутрения ошибка сервера
                return StatusCode(500, "An error has occurred");
            }
        }
        
        [HttpGet("GetInformationHall")]
        public IActionResult GetHall()
        {
            try
            {
                List<HallData> Hall = _cimenaDBContex.Зал.ToList();
                
                List<int> numberhall = new List<int>();

                foreach (var item in Hall)
                {
                    numberhall.Add(item.idЗал);
                }

                return Ok(numberhall);
            }
            catch (Exception)
            {
                //в случае неудачи вернем ошибку 500 - внутрения ошибка сервера
                return StatusCode(500, "An error has occurred");
            }
        }

        [HttpPost("LoginAdm")]
        public IActionResult Login(LoginModel model)
        {
            // Получаем данные из модели
            string login = model.Login;
            string hashedPassword = model.HashedPassword;

            List<UsersData> users = _cimenaDBContex.Users.ToList();

            if (users == null)
            {
                return BadRequest();
            }


            foreach (var item in users)
            {
                if (item.login_users == login)
                {
                    if (item.role == "admin")
                    {
                        if (VerifyPassword(item.password_users, hashedPassword))
                        {
                            // Возвращаем успешный статус или ошибку в зависимости от результата аутентификации
                            return Ok();
                        }   
                    }
                }
            }
            
            return BadRequest();
        }
        
        //Проверка пароля
        public static bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            // Создаем объект для вычисления хеша SHA256
            using (SHA256 sha256 = SHA256.Create())
            {
                // Преобразуем входной пароль в массив байтов
                byte[] inputBytes = Encoding.UTF8.GetBytes(inputPassword);

                // Вычисляем хеш входного пароля
                byte[] inputHashBytes = sha256.ComputeHash(inputBytes);

                // Преобразуем массив байтов в строку HEX
                StringBuilder inputBuilder = new StringBuilder();
                for (int i = 0; i < inputHashBytes.Length; i++)
                {
                    inputBuilder.Append(inputHashBytes[i].ToString("x2"));
                }

                // Сравниваем хеши
                return inputBuilder.ToString() == hashedPassword;
            }
        }

        [HttpPost("CreateFilm")]
        public IActionResult Create([FromBody] FilmsData film)
        {
            List<FilmsData> Films = _cimenaDBContex.Фильмы.ToList();

            FilmsData new_film = new FilmsData();
            new_film.idФильмы = Films.Count() + 1;
            new_film.Название  = film.Название;
            new_film.Продолжительность = film.Продолжительность;
            new_film.Страны_idСтраны = film.Страны_idСтраны + 1;
            new_film.Описание = film.Описание;
            new_film.Возрастное_ограничение = film.Возрастное_ограничение;
            new_film.Дата_Проката  = film.Дата_Проката;
            new_film.Стоимость_Проката  = film.Стоимость_Проката;
            
            try
            {
                //добавим нового пользователя в БД
                _cimenaDBContex.Фильмы.Add(new_film);
                _cimenaDBContex.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error has occurred");
            }

            return Ok();
        }
        
        [HttpPost("CreateSession")]
        public IActionResult Create([FromBody] SessionRequest session)
        {

            List<SessionData> Session = _cimenaDBContex.Сеансы.ToList();

            SessionData new_session = new SessionData();
            new_session.Дата_Сеанса = session.Дата_Сеанса;
            new_session.Время_Начала_Сеанса = TimeOnly.ParseExact(session.Время_Начала_Сеанса, "HH:mm", CultureInfo.InvariantCulture);
            new_session.Фильмы_idФильмы = session.Фильмы_idФильмы + 1;
            new_session.Зал_idЗал = session.Зал_idЗал + 1;

            //dkfhdksflh

            //try
            //{
            //    //добавим нового пользователя в БД
            //    _cimenaDBContex.Фильмы.Add(new_film);
            //    _cimenaDBContex.SaveChanges();
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, "An error has occurred");
            //}


            return Ok();
        }








    }
}
