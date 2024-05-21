using lynx.Models;
using lynx.Models.DTO;
using lynx.Services;
using Microsoft.AspNetCore.Mvc;

namespace lynx.Controllers
{
    // new controller for every request, HTTP is stateless, that is why new reply queue is generated for every request
    //Disposing connection and channel

    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;
        private readonly RPCClient _rpcClient;
        public SubmissionController(RPCClient rpcClient, ISubmissionService submissionService) {
            _rpcClient = rpcClient;
            _submissionService = submissionService;
        }
        [HttpPost]
        [Route("CreateSubmission")]
        public async Task<ActionResult<TestResult>> CreateSubmission(SubmissionDTO submission)
        {
            int submissionId = await _submissionService.CreateSubmission(submission);
            var response = await _rpcClient.CallAsync(submissionId).ConfigureAwait(false);
            return Ok(response);


            //try
            //{
            //    SubmissionContext context = await _submissionService.GetSubmissionContext(
            //        submission.assignment_parent_id,
            //        submission.test_fw_id
            //        );
            //    if (null == context)
            //        return StatusCode(503, "One of the services isn't responding correctly.");
            //    //add code skeletion here
            //    context.code_skeleton.code_before = context.code_skeleton.code_before.Replace("\\r", "\r").Replace("\\n", "\n");
            //    context.code_skeleton.code_after = context.code_skeleton.code_after.Replace("\\r", "\r").Replace("\\n", "\n");
            //    string code = string.Concat(context.code_skeleton.code_before,submission.source_code,context.code_skeleton.code_after);
            //    //
            //    PackagedSubmission packagedSubmission = new(code,submission.language_id, context);
            //    var response = await _rpcClient.CallAsync(packagedSubmission).ConfigureAwait(false);
            //    _ = await _submissionService.PersistSubmissionToDB(submission);

            //    return Ok(response);

            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, ex.Message);
            //}
        }

        [HttpGet]
        [Route("GetSubmissionsForAssignmentForUser/{user_id}/{assignment_id}")]
        public async Task<ActionResult<IEnumerable<Submission>>> GetSubmissionsForAssignmentForUser(int user_id,int assignment_id)
        {
            try
            {
                var submissions = await _submissionService.GetSubmissionsForAssignmentForUser(user_id,assignment_id);
                return Ok(submissions);
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
        [Route("GetSubmission/{submission_id}")]
        public async Task<ActionResult<Submission>> GetSubmission(int submission_id)
        {
            try
            {
                var submission = await _submissionService.GetSubmission(submission_id);
                if(null == submission)
                    return NotFound("Submission not found.");
                return Ok(submission);
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
        [Route("GetUserSubmissionQueue/{user_id}")]
        public async Task<ActionResult<IEnumerable<SubmissionQueueItem>>> GetUserSubmissionQueue(int user_id)
        {
            try
            {
                var submission_queue = await _submissionService.GetUserSubmissionQueue(user_id);
                return Ok(submission_queue);
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
        [Route("GetAllSubmissionsForAssignmentForContest/{assignment_id}/{contest_id}")]
        public async Task<ActionResult<IEnumerable<Submission>>> GetAllSubmissionsForAssignmentForContest(int assignment_id,int contest_id)
        {
            try
            {
                var submissions = await _submissionService.GetAllSubmissionsForAssignmentForContest(assignment_id,contest_id);
                return Ok(submissions);
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
        [Route("GetSubmissionsForUserForContest/{user_id}/{contest_id}")]
        public async Task<ActionResult<IEnumerable<Submission>>> GetSubmissionsForUserForContest(int user_id,int contest_id)
        {
            try
            {
                var submissions = await _submissionService.GetSubmissionsForUserForContest(user_id,contest_id);
                return Ok(submissions);
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
        [Route("CreateComment")]
        public async Task<ActionResult<int>>  CreateComment(Comment comment)
        {
            try
            {
                var id = await _submissionService.CreateComment(comment);
                return Ok(new
                {
                    added_comment = id
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
        [HttpGet]
        [Route("GetCommentsForSubmission/{id}")]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetCommentsForSubmission(int id)
        {
            try
            {
                var comments = await _submissionService.GetCommentsForSubmission(id);
                return Ok(comments);
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
        [Route("PostmoderateSubmission/{submission_id}")]
        public async Task<ActionResult> PostmoderateSubmission(int submission_id,bool accept)
        {
            try
            {
                _= await _submissionService.PostModerateSubmission(submission_id,accept);
                return Ok();
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
