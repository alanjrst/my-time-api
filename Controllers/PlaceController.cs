using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using my_time_api.Model;
using my_time_api.Services;
using System.Threading.Tasks;

namespace my_time_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlaceController : ControllerBase
    {
        private readonly MyTimeService _myTimeService;

        public PlaceController(MyTimeService myTimeService)
        {
            _myTimeService = myTimeService;
        }        
        [HttpGet]
        public async Task<ActionResult> Get() {
           return Ok((await _myTimeService._place.FindAsync(Place => true, _myTimeService._optionsPlace)).ToList());
        }

        [HttpGet("{id:length(24)}", Name = "GetPlace")]
        public async Task<ActionResult> Get(string id)
        {
            var place = this.GetPlace(id);

            if (place == null)
            {
                return NotFound();
            }

            return Ok(place);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Place place)
        {
            if(string.IsNullOrEmpty(place.UserId))
                return BadRequest("Place without user");

            await _myTimeService._place.InsertOneAsync(place);

            return CreatedAtRoute("GetPlace", new { id = place.PlaceId }, place);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Place placeIn)
        {
            var place = this.GetPlace(id);

            if (place == null)
            {
                return NotFound();
            }

            if(placeIn.PlaceId != id){
                return BadRequest($"Id: {id} must match the object sent {place.PlaceId}");
            }

            _myTimeService._place.ReplaceOneAsync(place => place.PlaceId == id, placeIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var place = this.GetPlace(id);

            if (place == null)
            {
                return NotFound();
            }

            _myTimeService._place.DeleteOneAsync(place => place.PlaceId == id);

            return NoContent();
        }

        public Place GetPlace(string id){
            return _myTimeService._place.Find(place => place.PlaceId == id).SingleOrDefault();
        }
       
    }
}