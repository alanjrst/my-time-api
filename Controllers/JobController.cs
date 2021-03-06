using System;
using System.Security.Claims;
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
    public class JobController : ControllerBase
    {
        private readonly MyTimeService _myTimeService;
        public String _userId { get; set; }

        public JobController(MyTimeService myTimeService)
        {
            _myTimeService = myTimeService;            
            // _userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }        
        
        [HttpGet]
        public async Task<ActionResult> Get() {            
            return Ok((await _myTimeService._job.FindAsync(job => job.UserId == this.User.FindFirst(ClaimTypes.NameIdentifier).Value , _myTimeService._optionsJob)).ToList());
        }

        [HttpGet("byPlace/{idPlace}")]
        public async Task<ActionResult> GetByPlace(string idPlace) {            
            return Ok((await _myTimeService._job.FindAsync(job => job.PlaceId == idPlace , _myTimeService._optionsJob)).ToList());
        }

        [HttpGet("{id:length(24)}", Name = "GetJob")]
        public async Task<ActionResult> Get(string id)
        {
            var job = this.GetJob(id);

            if (job == null)
            {
                return NotFound();
            }

            return Ok(job);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Job job)
        {
            job.UserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _myTimeService._job.InsertOneAsync(job);

            return CreatedAtRoute("GetJob", new { id = job.JobId }, job);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Job jobIn)
        {
            var job = this.GetJob(id);

            if (job == null)
            {
                return NotFound();
            }

            if(jobIn.JobId != id){
                return BadRequest($"Id: {id} must match the object sent {job.JobId}");
            }

            _myTimeService._job.ReplaceOneAsync(job => job.JobId == id, jobIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var job = this.GetJob(id);

            if (job == null)
            {
                return NotFound();
            }

            _myTimeService._job.DeleteOneAsync(job => job.JobId == id);

            return NoContent();
        }

        public Job GetJob(string id){
            return _myTimeService._job.Find(job => job.JobId == id).SingleOrDefault();
        }
    }
}