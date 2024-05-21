using lynx.Models;

namespace lynx
{
    public interface IAssignmentProvider
    {
        Task<SubmissionContext> GetSubmissionContext(int assignment_parent_id);
    }
    public class AssignmentProvider : IAssignmentProvider
    {
        public Task<SubmissionContext> GetSubmissionContext(int assignment_parent_id)
        {
            return Task.FromResult(new SubmissionContext());
        }
    }
}
