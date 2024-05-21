using lynx.Models;
using lynx.Services;
using Microsoft.AspNetCore.Mvc;

namespace lynx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        private IHostApplicationLifetime _applicationLifetime;

        public AssignmentController(IAssignmentService assignmentService, IHostApplicationLifetime applicationLifetime)
        {
            _assignmentService = assignmentService;
            _applicationLifetime = applicationLifetime;
        }

        [HttpGet]
        [Route("test")]
        public ActionResult test()
        {
            var keys = Environment.GetEnvironmentVariables();
            return Ok(keys);
        }

        [HttpGet]
        [Route("GetAssignment/{id}")]
        public async Task<ActionResult<Assignment>> GetAssignment(int id)
        {
            try
            {
                var assignment = await _assignmentService.GetAssignment(id); 
                return Ok(assignment);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
        [HttpGet]
        [Route("GetAssignments")]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetAssignments()
        {
            try
            {
                var assignments = await _assignmentService.GetAllAssignments(); 
                return Ok(assignments);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }

        [HttpPost]
        [Route("CreateAssignment")]
        public async Task<ActionResult<int>> CreateAssignment(Assignment assignment)
        {
            try
            {
                var id = await _assignmentService.CreateAssignment(assignment);
                return Ok(id);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
        [HttpPut]
        [Route("UpdateAssignment")]
        public async Task<ActionResult> UpdateAssignment(Assignment assignment)
        {
            try
            {
                _ = await _assignmentService.UpdateAssignment(assignment);
                return NoContent();
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
        [HttpDelete]
        [Route("DeleteAssignment/{id}")]
        public async Task<ActionResult> DeleteAssignment(int id)
        {
            try
            {
                _ = await _assignmentService.DeleteAssignment(id);
                return NoContent();
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
    }
}
