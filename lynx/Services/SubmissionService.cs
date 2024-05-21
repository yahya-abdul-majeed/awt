using Dapper;
using lynx.Data;
using lynx.Models;
using lynx.Models.DTO;

namespace lynx.Services
{
    public interface ISubmissionService
    {
        Task<int> CreateSubmission(SubmissionDTO submission);
        Task<SubmissionContext> GetSubmissionContext(int assignment_id,int test_fw_id);
        Task<int> PersistSubmissionToDB(SubmissionDTO submission);
        Task<Submission?> GetSubmission(int submission_id);
        Task<int> UpdateSubmission();
        Task<IEnumerable<SubmissionQueueItem>> GetUserSubmissionQueue(int userid);
        Task<IEnumerable<Submission>> GetSubmissionsForAssignmentForUser(int user_id, int assignment_id);
        Task<IEnumerable<Submission>> GetAllSubmissionsForAssignmentForContest(int assignment_id,int contest_id);
        Task<IEnumerable<Submission>> GetSubmissionsForUserForContest(int user_id,int contest_id);
        Task<int> CreateComment( Comment comment);
        Task<IEnumerable<CommentDTO>> GetCommentsForSubmission(int submission_id);
        Task<int> PostModerateSubmission(int submission_id, bool accept);


    }
    public class SubmissionService : ISubmissionService
    {
        private readonly DapperContext _context;
        private readonly IbexClient _ibexClient;
        public SubmissionService(DapperContext context,IbexClient ibexClient)
        {
            _context = context;
            _ibexClient = ibexClient;
        }
        public async Task<Submission?> GetSubmission(int submission_id)
        {
            var parameters = new { submission_id };

            using(var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Submission>(Queries.GetSubmissionById,parameters);
            }
        }

        public async Task<SubmissionContext> GetSubmissionContext(int assignment_id,int test_fw_id)
        {
            var res = await _ibexClient.GetSubmissionContext(assignment_id,test_fw_id);
            return res;
        }
        public async Task<int> CreateSubmission(SubmissionDTO submission)
        {
            var parameters = new {
                user_id = submission.user_id,
                test_fw_id = submission.test_fw_id,
                assignment = submission.assignment_id,
                assignment_parent = submission.assignment_parent_id,
                language = submission.language_id,
                sourcecode = submission.source_code,
                status = Status.INQUEUE,
                lang_fw = getLangFw(submission.language_id,submission.test_fw_id)
            };
            
            using(var connection = _context.CreateConnection())
            {
                var res = await connection.QuerySingleAsync<int>(Queries.CreateSubmission, parameters);
                return res;
            }
        }

        private string getLangFw(int lang, int tfw)
        {
            if (lang == 4 && tfw == 2)
                return "dotnet_nunit3";
            if (lang == 1 && tfw == 1)
                return "python3_unittest";

            return string.Empty;
        }

        public async Task<IEnumerable<SubmissionQueueItem>> GetUserSubmissionQueue(int userid)
        {
            var parameters = new { userid };

            using(var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<SubmissionQueueItem>(Queries.GetUserSubmissionQueue,parameters);
            }
        }

        public async Task<int> PersistSubmissionToDB(SubmissionDTO submission)
        {
            var parameters = new {
                assignment = submission.assignment_id,
                assignment_parent = submission.assignment_parent_id,
                language = submission.language_id,
                sourcecode = submission.source_code,
                status = Status.INQUEUE,
            };
            
            using(var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(Queries.CreateSubmission, parameters);
            }
        }

        public Task<int> UpdateSubmission()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Submission>> GetSubmissionsForAssignmentForUser(int user_id, int assignment_id)
        {
            var parameters = new
            {
                user_id,
                assignment_id
            };
            using(var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Submission>(Queries.GetSubmissionsForAssignmentForUser,parameters);
            }
        }

        public async Task<IEnumerable<Submission>> GetAllSubmissionsForAssignmentForContest(int assignment_id, int contest_id)
        {
            var parameters = new
            {
                contest_id,
                assignment_id
            };
            using(var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Submission>(Queries.GetAllSubmissionsForAssignmentForContest,parameters);
            }
        }

        public async Task<IEnumerable<Submission>> GetSubmissionsForUserForContest(int user_id,int contest_id)
        {
            var parameters = new
            {
                contest_id,
                user_id
            };
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Submission>(Queries.GetSubmissionsForUserForContest,parameters);
            }
        }

        public async Task<int> CreateComment(Comment comment)
        {
            var parameters = new
            {
                comment.submission_id,
                comment.parent_id,
                comment.user_id,
                comment.comment_text
            };
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleAsync<int>(Queries.CreateComment,parameters); 
            }
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsForSubmission(int submission_id)
        {
            var parameters = new { submission_id };
            using(var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<CommentDTO>(Queries.GetCommentsForSubmission, parameters);
            }
        }

        public async Task<int> PostModerateSubmission(int submission_id, bool accept)
        {
            var parameters = new { submission_id , accept};
            using(var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(Queries.PostmoderateSubmission, parameters);
            }
        }
    }
}
