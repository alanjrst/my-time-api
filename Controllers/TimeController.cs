using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using my_time_api.Model;
using my_time_api.Services;

namespace my_time_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TimeController : ControllerBase
    {
        private readonly MyTimeService _myTimeService;

        public TimeController(MyTimeService myTimeService)
        {
            _myTimeService = myTimeService;
        }        
        [HttpGet]
        public async Task<ActionResult> Get() {
           return Ok((await _myTimeService._time.FindAsync(time => true , _myTimeService._optionsTime)).ToList());
        }

        [HttpGet("{id:length(24)}", Name = "GetTime")]
        public async Task<ActionResult> Get(string id)
        {
            var time = this.GetTime(id);

            if (time == null)
            {
                return NotFound();
            }

            return Ok(time);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Time time)
        {
            await _myTimeService._time.InsertOneAsync(time);

            return CreatedAtRoute("GetTime", new { id = time.TimeId }, time);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Time timeIn)
        {
            var time = this.GetTime(id);

            if (time == null)
            {
                return NotFound();
            }

            if(timeIn.TimeId != id){
                return BadRequest($"Id: {id} must match the object sent {time.TimeId}");
            }

            _myTimeService._time.ReplaceOneAsync(time => time.TimeId == id, timeIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var time = this.GetTime(id);

            if (time == null)
            {
                return NotFound();
            }

            _myTimeService._time.DeleteOneAsync(time => time.TimeId == id);

            return NoContent();
        }

        public Time GetTime(string id){
            return _myTimeService._time.Find(time => time.TimeId == id).SingleOrDefault();
        }
    }
}