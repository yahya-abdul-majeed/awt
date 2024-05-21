using ibex.Models;
using ibex.Models.DTO;
using ibex.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ibex.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkeletonController : ControllerBase
    {
        private readonly ISkeletonService _skeletonService;

        public SkeletonController(ISkeletonService skeletonService)
        {
            _skeletonService = skeletonService;
        }

        [HttpGet]
        [Route("GetSkeletons")]
        public async Task<ActionResult<IEnumerable<SkeletonDTO>>> GetSkeletons()
        {
            try
            {
                var skeletons = await _skeletonService.GetSkeletons();
                return Ok(skeletons);
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
        [Route("GetSkeleton/{id}")]
        public async Task<ActionResult<SkeletonDTO>> GetSkeleton(int id)
        {
            try
            {
                var skeleton = await _skeletonService.GetSkeleton(id);
                return Ok(skeleton);
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
        [Route("GetSkeletonsForAssignment/{id}")]
        public async Task<ActionResult<IEnumerable<SkeletonDTO>>> GetSkeletonsForAssignment(int id)
        {
            try
            {
                var skeleton = await _skeletonService.GetSkeletonsForAssignment(id);
                return Ok(skeleton);
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
        [Route("GetTestingFrameworks")]
        public async Task<ActionResult<IEnumerable<TestingFrameworkDTO>>> GetTestingFrameworks()
        {
            try
            {
                var frameworks = await _skeletonService.GetTestingFrameworks();
                return Ok(frameworks);
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
        [Route("AddSkeleton")]
        public async Task<ActionResult> AddSkeleton(AddSkeletonDTO skeleton)
        {
            try
            {
                await _skeletonService.AddSkeleton(skeleton);
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
        [Route("UpdateSkeleton")]
        public async Task<ActionResult> UpdateSkeleton(UpdateSkeletonDTO skeleton)
        {
            try
            {
                await _skeletonService.UpdateSkeleton(skeleton);
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
        [HttpGet]
        [Route("GetLanguages")]
        public async Task<ActionResult<IEnumerable<Language>>> GetLanguages()
        {
            try
            {
                var languages = await _skeletonService.GetLanguages();
                return Ok(languages);
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
