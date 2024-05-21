using lynx.Services;
using lynx.Models;
using Microsoft.AspNetCore.Mvc;
using lynx.Models.DTO;

namespace lynx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContestController : ControllerBase
    {
        private readonly IContestService _contestService;
        public ContestController(IContestService contestService)
        {
            _contestService = contestService;
        }

        [HttpPost]
        [Route("AddContest")]
        public async Task<ActionResult> AddContest(AddContestDTO contest)
        {
            try
            {
                var id = await _contestService.AddContest(contest);
                return Ok(new
                {
                    added_contest = id
                });
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddContestFromContest")]
        public async Task<ActionResult> AddContest(AddContestDTO contest, int contestid)
        {
            try
            {
                var id = await _contestService.AddContest(contest,contestid);
                return Ok(new
                {
                    added_contest = id
                });
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddAssignmentsToContest")]
        public async Task<ActionResult> AddAssignmentsToContest(List<AssignmentAdditionDTO> additions)
        {
            try
            {
                _ = await _contestService.AddAssignmentsToContest(additions);
                return NoContent();
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPost]
        [Route("AddRemoteAssignmentsToContest")]
        public async Task<ActionResult> AddRemoteAssignmentsToContest(List<int> remote_ids,int contest_id)
        {
            try
            {
                _ = await _contestService.AddRemoteAssignmentsToContest(remote_ids,contest_id);
                return NoContent();
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet]
        [Route("GetContestsForUser/{id}")]
        public async Task<ActionResult<IEnumerable<GetContestDTO>>> GetContestsForUser(int id)
        {
            try
            {
                var contests = await _contestService.GetContestsForUser(id);
                return Ok(contests);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet]
        [Route("GetContest/{id}")]
        public async Task<ActionResult<GetContestDTO>> GetContest(int id)
        {
            try
            {
                var contest = await _contestService.GetContest(id);
                if (null == contest)
                    return NotFound("Contest with such Id not found");
                return Ok(contest);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("GetContests")]
        public async Task<ActionResult<IEnumerable<GetContestDTO>>> GetContests()
        {
            try
            {
                var contests = await _contestService.GetContests();
                return Ok(contests);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("GetContestsForAssignment/{id}")]
        public async Task<ActionResult<IEnumerable<GetContestForAssignmentDTO>>> GetContestsForAssignment(int id)
        {
            try
            {
                var contests = await _contestService.GetContestsForAssignment(id);
                return Ok(contests);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("AddGroupToContest")]
        public async Task<ActionResult> AddGroupToContest(int contestid, int groupid)
        {
            try
            {
                _ = await _contestService.AddGroupToContest(contestid, groupid);
                return NoContent();
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost]
        [Route("AddGroupsToContest")]
        public async Task<ActionResult> AddGroupsToContest(AddGroupsToContestDTO data)
        {
            try
            {
                _ = await _contestService.AddGroupsToContest(data.contest_id, data.groupids);
                return NoContent();
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet]
        [Route("GetContestAssignments/{contestid}")]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetContestAssignments(int contestid)
        {
            try
            {
                var assignments = await _contestService.GetContestAssignments(contestid);
                return Ok(assignments);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch( Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetContestRankings/{contestid}")]
        public async Task<ActionResult<IEnumerable<ContestRankingItem>>> GetContestRankings(int contestid)
        {
            try
            {
                var rankings = await _contestService.GetContestRankings(contestid);
                return Ok(rankings);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut]
        [Route("UpdateContest/{id}")]
        public async Task<ActionResult> EditContest(int id,EditContestDTO obj)
        {
            try
            {
                _ = await _contestService.EditContest(id,obj.contest,obj.groups,obj.assignments);
                return NoContent();
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete]
        [Route("DeleteContest/{id}")]
        public async Task<ActionResult> DeleteContest(int id)
        {
            try
            {
                _ = await _contestService.DeleteContest(id);
                return NoContent();
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }

}
