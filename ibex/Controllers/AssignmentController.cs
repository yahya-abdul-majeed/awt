using ibex.Models;
using ibex.Models.DTO;
using ibex.Services;
using Microsoft.AspNetCore.Mvc;

namespace ibex.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpGet]
        [Route("GetSubmissionContext/{id}/{testing_fw_id}")]
        public async Task<ActionResult<SubmissionContext>> GetSubmissionContext(int id,int testing_fw_id)
        {
            try
            {
                var sc = await _assignmentService.GetSubmissionContext(id,testing_fw_id);
                return Ok(sc);
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
        [Route("GetAssignment/{id}")]
        public async Task<ActionResult<GetAssignmentDTO>> GetAssignment(int id)
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
            catch(Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }
        [HttpGet]
        [Route("GetAssignmentForShark/{id}")]
        public async Task<ActionResult<GetAssignmentDTO>> GetAssignmentForShark(int id)
        {
            try
            {
                var assignment = await _assignmentService.GetAssignmentForShark(id);
                return Ok(assignment);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpGet]
        [Route("GetAssignments")]
        public async Task<ActionResult<IEnumerable<GetAssignmentDTO>>> GetAssignments()
        {
            try
            {
                var assignments = await _assignmentService.GetAssignments();
                return Ok(assignments);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception)
            {
                return StatusCode(500, "Something went wrong");
            }

        }
        [HttpGet]
        [Route("GetAssignmentsForShark")]
        public async Task<ActionResult<IEnumerable<GetAssignmentDTO>>> GetAssignmentsForShark()
        {
            try
            {
                var assignments = await _assignmentService.GetAssignmentsForShark();
                return Ok(assignments);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception)
            {
                return StatusCode(500, "Something went wrong");
            }

        }
        [HttpPost]
        [Route("GetSpecificAssignments")]
        public async Task<ActionResult<IEnumerable<LynxAssignmentDTO>>> GetSpecificAssignments([FromBody]List<int> ids)
        {
            try
            {
                var assignments = await _assignmentService.GetSpecificAssignments(ids);
                return Ok(assignments);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception)
            {
                return StatusCode(500, "Something went wrong");
            }

        }


        [HttpPost]
        [Route("AddAssignment")]
        public async Task<ActionResult> AddAssignment(AddAssignmentDTO assignment)
        {
            try
            {
                await _assignmentService.AddAssignment(assignment);
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
        [Route("DeleteAssignment/{id}")]
        public async Task<ActionResult> DeleteAssignment(int id)
        {
            try
            {
                await _assignmentService.DeleteAssignment(id);
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
        [Route("UpdateAssignment")]
        public async Task<ActionResult> UpdateAssignment(UpdateAssignmentDTO assignment)
        {
            try
            {
                await _assignmentService.UpdateAssignment(assignment);
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
