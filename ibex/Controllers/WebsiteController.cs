using ibex.Models;
using ibex.Models.DTO;
using ibex.Services;
using Microsoft.AspNetCore.Mvc;

namespace ibex.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebsiteController : ControllerBase
    {
        private readonly IWebsiteService _websiteService;

        public WebsiteController(IWebsiteService websiteService)
        {
            _websiteService = websiteService;
        }

        [HttpGet]
        [Route("GetWebsite/{id}")]
        public async Task<ActionResult<Website>> GetWebsite(int id)
        {
            try
            {
                var website = await _websiteService.GetWebsite(id);
                return Ok(website);
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
        [Route("GetWebsites")]
        public async Task<ActionResult<Website>> GetWebsites()
        {
            try
            {
                var websites = await _websiteService.GetWebsites();
                return Ok(websites);
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
        [Route("GetParentWebsites")]
        public async Task<ActionResult<IEnumerable<Website>>> GetParentWebsites()
        {
            try
            {
                var websites = await _websiteService.GetParentWebsites();
                return Ok(websites);
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
        [Route("GetWebsitesForAssignment/{id}")]
        public async Task<ActionResult<Website>> GetWebsitesForAssignment(int id)
        {
            try
            {
                var websites = await _websiteService.GetWebsitesForAssignment(id);
                return Ok(websites);
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
        [Route("GetWebsiteTree")]
        public async Task<ActionResult<List<WebsiteNodeDTO>>> GetWebsiteTree()
        {
            try
            {
                var tree= await _websiteService.GetWebsiteTree();
                return Ok(tree);
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
        [Route("AddWebsite")]
        public async Task<ActionResult> AddWebsite(AddWebsiteDTO website)
        {
            try
            {
                await _websiteService.AddWebsite(website);
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
        [HttpPost]
        [Route("UpdateWebsite")]
        public async Task<ActionResult> UpdateWebsite(UpdateWebsiteDTO website)
        {
            try
            {
                await _websiteService.UpdateWebsite(website);
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
        [Route("DeleteWebsite/{id}")]
        public async Task<ActionResult> DeleteWebsite(int id)
        {
            try
            {
                await _websiteService.DeleteWebsite(id);
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
