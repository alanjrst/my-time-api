using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using my_time_api.Model;
using my_time_api.Service;
using my_time_api.Services;

namespace my_time_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyTimeService _myTimeService;

        public UserController(MyTimeService myTimeService)
        {
            _myTimeService = myTimeService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult<User> Login(User userIn)
        {
            var user = _myTimeService._user.Find(user => user.Email == userIn.Email).FirstOrDefault();

            if(user == null)
                return NotFound();

            if(BCrypt.Net.BCrypt.Verify(userIn.Password, user.Password)){
                user.Password = "";
                user.Token = TokenService.GenerateToken(user);
                return Ok(user);
            }else{
                return BadRequest(new { Message = "Name and password not valid !" });
            }
        }

        [HttpPost]
        public ActionResult<User> Create(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _myTimeService._user.InsertOneAsync(user);

            return Ok(user);
        }

        [Authorize]
        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, User userIn)
        {
            var user = this.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            if(userIn.UserId != id){
                return BadRequest($"Id: {id} must match the object sent {userIn.UserId}");
            }

            userIn.Password = user.Password;
            userIn.Token = user.Token;

            _myTimeService._user.ReplaceOneAsync(user => user.UserId == id, userIn);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var user = this.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            _myTimeService._user.DeleteOneAsync(user => user.UserId == id);

            return NoContent();
        }
       
        public User GetUser(string id){
            return _myTimeService._user.Find(user => user.UserId == id).SingleOrDefault();
        }
    }
}