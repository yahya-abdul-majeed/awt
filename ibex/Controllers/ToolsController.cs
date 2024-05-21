using ibex.Data;
using ibex.Models;
using ibex.Models.DTO;
using ibex.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ibex.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        private readonly IToolService _toolService;
        public ToolsController(IToolService toolService)
        {
            _toolService = toolService;
        }

        [HttpPost]
        [Route("AddLanguage")]
        public async Task<ActionResult> AddLanguage(Language language)
        {
            try
            {
                await _toolService.AddLanguage(language);
                return Ok();
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
        [Route("UpdateLanguage")]
        public async Task<ActionResult> UpdateLanguage(Language language)
        {
            try
            {
                await _toolService.UpdateLanguage(language);
                return Ok();
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
        [Route("AddFramework")]
        public async Task<ActionResult> AddFramework(AddFrameworkDTO framework)
        {
            try
            {
                await _toolService.AddFramework(framework);
                return Ok();
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
        [Route("UpdateFramework")]
        public async Task<ActionResult> UpdateFramework(UpdateFrameworkDTO framework)
        {
            try
            {
                await _toolService.UpdateFramework(framework);
                return Ok();
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
