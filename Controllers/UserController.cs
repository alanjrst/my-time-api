using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using my_time_api.Model;
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

        [HttpPost("login")]
        public ActionResult<User> Login(User userIn)
        {
            var user = _myTimeService._user.Find(user => user.Email == userIn.Email).SingleOrDefault();

            if(user == null)
                return NotFound();

            if(BCrypt.Net.BCrypt.Verify(userIn.Password, user.Password)){
                user.Password = "";
                user.Token = "";
                return Ok(user);
            }else{
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<User> Create(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _myTimeService._user.InsertOneAsync(user);

            return Ok(user);
        }

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