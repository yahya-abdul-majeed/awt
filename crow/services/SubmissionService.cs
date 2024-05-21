using System.Reflection.Metadata.Ecma335;
using crow.data;
using crow.models;
using Dapper;
using Microsoft.IdentityModel.Tokens;

namespace crow.services;

public interface ISubmissionService
{
    public Task<SubmissionContext> GetContext(int assignment_parent_id,int test_fw_id); 
    public Task<Submission?> GetSubmission(int submission_id);
    public Task OnSubmissionStart(int submission_id);
    public Task OnSubmissionEnd(int submission_id,TestResult result);
}

internal class SubmissionService : ISubmissionService
{
    private readonly DapperContext _context;
    private ILogger<SubmissionService> _logger;
    public SubmissionService(DapperContext context,ILogger<SubmissionService> logger)
    {
        _context = context;
        _logger = logger;   
    }
    public async Task<SubmissionContext> GetContext(int assignment_parent_id,int test_fw_id)
    {
        var parameters = new {
            assignment_id = assignment_parent_id,
            testing_fw_id =test_fw_id
            };
        using (var connection = _context.CreateConnectionIbex())
        {
            using (var multi = await connection.QueryMultipleAsync(Queries.GetSubmissionContext, parameters))
                    {
                        var skeleton = multi.ReadFirst<Skeleton>();
                        var websites = multi.Read<ContextItem>();
                        return new SubmissionContext
                        {
                            Websites = websites.ToList(),
                            code_skeleton = skeleton
                        };
                    }
                }

        }

    public async Task<Submission?> GetSubmission(int submission_id)
    {
        try{
            var parameters = new {submission_id};
            using(var connection = _context.CreateConnectionKino()){
                var res = await connection.QuerySingleAsync<Submission?>(Queries.GetSubmission,parameters);
                return res;
            }
        }
        catch(Exception){
            return null;
        }
    }
    public async Task OnSubmissionStart(int submission_id){
        var parameters = new {
            execution_host = System.Net.Dns.GetHostName(),
            status = Status.PROCESSING,
            started_at = DateTime.Now,
            id = submission_id
        };
        using(var connection = _context.CreateConnectionKino()){
            await connection.ExecuteAsync(Queries.OnSubmissionStart,parameters);
        }
    }
    public async Task OnSubmissionEnd(int submission_id,TestResult result){
        var parameters = new {
            status = Status.COMPLETED,
            finished_at = DateTime.Now,
            id = submission_id,
            result = result.result_file,
            auto_verdict = result.passed
        };
        using(var connection = _context.CreateConnectionKino()){
            await connection.ExecuteAsync(Queries.OnSubmissionEnd,parameters);
        }
    }

}