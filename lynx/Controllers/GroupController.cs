using lynx.Models;
using lynx.Models.DTO;
using lynx.Services;
using Microsoft.AspNetCore.Mvc;

namespace lynx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        //[Authorize(Policy = "RolePolicy")]
        [HttpGet]
        [Route("GetGroups")]
        public async Task<ActionResult<IEnumerable<GetGroupDTO>>> GetGroups()
        {
            try
            {
                var groups = await _groupService.GetAllGroups();
                return Ok(groups);


                //var groups = await _groupService.GetAllGroups();
                //return Ok(new
                //{
                //    data = new
                //    {
                //        total = groups.Count(),
                //        rows = groups
                //    }
                //});
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("GetGroupsForContest/{contest_id}")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroupsForContest(int contest_id)
        {
            try
            {
                var groups = await _groupService.GetGroupsForContest(contest_id);
                return Ok(groups);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetGroup/{id}")]
        public async Task<ActionResult<GetGroupDTO>> GetGroup(int id)
        {
            try
            {
                var group = await _groupService.GetGroupById(id);
                if (null == group)
                    return NotFound("Group with such Id not found");
                return Ok(group);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddGroup")]
        public async Task<ActionResult> AddGroup(AddGroupDTO group)
        {
            try
            {
                var id = await _groupService.AddGroup(group);
                return Ok(new
                {
                    added_group = id,
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

        [HttpDelete]
        [Route("DeleteGroup/{id}")]
        public async Task<ActionResult> DeleteGroup(int id)
        {
            try
            {
                _ = await _groupService.DeleteGroup(id);
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
        [Route("AddUserToGroup")]
        public async Task<ActionResult> AddUserToGroup(int userid, int groupid)
        {
            try
            {
                _ = await _groupService.AddUserToGroup(userid, groupid);
                return NoContent();

            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpPost]
        [Route("AddUsersToGroup")]
        public async Task<ActionResult> AddUsersToGroup(List<int> userids, int groupid)
        {
            try
            {
                _ = await _groupService.AddUsersToGroup(userids, groupid);
                return NoContent();

            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpGet]
        [Route("AddGroupToGroup")]
        public async Task<ActionResult> AddGroupToGroup(int groupid_1, int groupid_2)
        {
            try
            {
                _ = await _groupService.AddGroupToGroup(groupid_1, groupid_2);
                return NoContent();

            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpPut]
        [Route("UpdateGroup/{group_id}")]
        public async Task<ActionResult> UpdateGroup(int group_id,EditGroupDTO obj)
        {
            try
            {
                _ = await _groupService.EditGroup(group_id, obj.group, obj.userids);
                return NoContent();

            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
    }
}
