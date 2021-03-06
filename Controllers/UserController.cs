using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        public readonly IConfiguration _config;

        public UserController(MyTimeService myTimeService, IConfiguration config)
        {
            _myTimeService = myTimeService;
            _config = config;
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
                var tokenService = new TokenService(_config);
                user.Password = "";
                user.Token = tokenService.GenerateToken(user);
                return Ok(user);
            }else{
                return BadRequest(new { Message = "Name and password not valid !" });
            }
        }

        [HttpPost]
        [Route("logout")]
        [AllowAnonymous]
        public ActionResult<User> Logout(User userIn)
        {
            var user = _myTimeService._user.Find(user => user.Email == userIn.Email).FirstOrDefault();

            if(user == null)
                return NotFound();
            
            var claims = new ClaimsIdentityLogout();
            claims.Name = this.User.FindFirst(ClaimTypes.Name).Value;
            claims.NameIdentifier = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            claims.Role = this.User.FindFirst(ClaimTypes.Role).Value;
            
            _myTimeService._claimsIdentityLogout.InsertOneAsync(claims);

            return Ok("");
        }

        [HttpPost]
        public ActionResult<User> Create(User user)
        {
            var userResult = _myTimeService._user.Find(user => user.Email == user.Email).SingleOrDefault();
            if(userResult != null){
                return BadRequest("Usuário já cadastrado.");
            }

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