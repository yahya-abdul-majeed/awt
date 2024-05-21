using ibex.Models;
using ibex.Models.DTO;
using ibex.Services;
using Microsoft.AspNetCore.Mvc;

namespace ibex.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugController : ControllerBase
    {
        private readonly IBugService _bugService;

        public BugController(IBugService bugService)
        {
            _bugService = bugService;
        }

        [HttpGet]
        [Route("GetBug/{id}")]
        public async Task<ActionResult<Bug>> GetBug(int id)
        {
            try
            {
                var bug = await _bugService.GetBug(id);
                return Ok(bug);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }
        [HttpGet]
        [Route("GetBugs")]
        public async Task<ActionResult<IEnumerable<Bug>>> GetBugs()
        {
            try
            {
                var bugs = await _bugService.GetBugs();
                return Ok(bugs);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }
        [HttpGet]
        [Route("GetBugsForWebsite/{website_id}")]
        public async Task<ActionResult<IEnumerable<Bug>>> GetBugsForWebsite(int website_id)
        {
            try
            {
                var bugs = await _bugService.GetBugsForWebsite(website_id);
                return Ok(bugs);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpPost]
        [Route("AddBug")]
        public async Task<ActionResult> AddBug(AddBugDTO bug)
        {
            try
            {
                await _bugService.AddBug(bug);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpDelete]
        [Route("DeleteBug/{id}")]
        public async Task<ActionResult> DeleteBug(int id)
        {
            try
            {
                await _bugService.DeleteBug(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }

        }

        [HttpPut]
        [Route("UpdateBug/{id}")]
        public async Task<ActionResult> UpdateBug(UpdateBugDTO bug,int id)
        {
            try
            {
                await _bugService.UpdateBug(bug,id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }

        }
    }
}
