using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //Получение доступа к БД
        private TestDBContex _testDBContex;

        public UserController(TestDBContex testDBContex)
        {
            _testDBContex = testDBContex;
        }

        //Запрос на получение данных
        [HttpGet("GetUsers")]
        public IActionResult Get()
        {
            try
            {
                var users = _testDBContex.tbUsers.ToList();
                //проверка если нету пользователей
                if (users.Count == 0)
                {
                    return StatusCode(404, "No user found");
                }

                return Ok(users);
            }
            catch (Exception)
            {
                //в случае неудачи вернем ошибку 500 - внутрения ошибка сервера
                return StatusCode(500, "An error has occurred");
            }
        }
        //Запрос на создание данных
            //Если пользователь что то передаст получим ошибку поэтому создажим Model -> Models -> UserRequest
            //Ожидается что пользователь должен прислать определенную модель описаную в UserRequest
        [HttpPost("CreateUsers")]
        public IActionResult Create([FromBody] UserRequest request)
        {
            tbUser new_user = new tbUser();
            new_user.UserName = request.UserName;
            new_user.FirstName = request.FirstName;
            new_user.LastName = request.LastName;
            new_user.City = request.City;
            new_user.Country = request.Country;
            new_user.State = request.State;

            try
            {
                //добавим нового пользователя в БД
                _testDBContex.tbUsers.Add(new_user);
                _testDBContex.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error has occurred");
            }

            var users = _testDBContex.tbUsers.ToList();
            return Ok(users);
        }
        
        [HttpPut("UpdateUsers")]
        public IActionResult Update([FromBody] UserRequest request)
        {
            try
            {
                var user = _testDBContex.tbUsers.FirstOrDefault(x => x.Id == request.Id);
                if (user == null)
                {
                    return StatusCode(404, "User not found");
                }

                user.UserName = request.UserName;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.City = request.City;
                user.Country = request.Country;
                user.State = request.State;

                //обновим запис старого пользователя
                _testDBContex.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _testDBContex.SaveChanges();

            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error has occurred");
            }

            var users = _testDBContex.tbUsers.ToList();
            return Ok(users);
        }

        //В метод DLETE требуется передать только идентификатор удаляемого пользователя
        [HttpDelete("DeleteUsers/{Id}")]
        public IActionResult Delete([FromRoute]int Id)
        {
            try
            {
                var user = _testDBContex.tbUsers.FirstOrDefault(x => x.Id == Id);
                if (user == null)
                {
                    return StatusCode(404, "User not found");
                }

                //обновим запис старого пользователя
                _testDBContex.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                _testDBContex.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error has occurred");
            }

            var users = _testDBContex.tbUsers.ToList();
            return Ok(users);
        }


        //пустышка для проверки
        //private List<UserRequest> GetUsers()
        //{
        //    return new List<UserRequest>{
        //        new UserRequest()
        //        {
        //            UserName = "ABC",
        //            FirstName = "d"
        //        }
        //    };
        //}


    }
}
