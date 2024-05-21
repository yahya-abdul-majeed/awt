using lynx.Models;
using Refit;

namespace lynx
{
    public interface IbexClient
    {
        [Get("/api/assignment/GetSubmissionContext/{id}/{testing_fw_id}")]
        Task<SubmissionContext> GetSubmissionContext(int id, int testing_fw_id);
        [Post("/api/assignment/GetSpecificAssignments")]
        Task<IEnumerable<Assignment>> GetSpecificAssignments(List<int> ids);
    }
}
