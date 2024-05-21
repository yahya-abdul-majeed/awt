using ibex.Data;
using ibex.Models;
using ibex.Models.DTO;
using Dapper;

namespace ibex.Services
{
    public interface IAssignmentService
    {
        Task<SubmissionContext> GetSubmissionContext(int assignment_id,int testing_fw_id);
        Task AddAssignment(AddAssignmentDTO assignment);
        Task UpdateAssignment(UpdateAssignmentDTO assignment);
        Task<IEnumerable<GetAssignmentDTO>> GetAssignments(); //returns only assignments with primary websites, used in dolphin
        Task<IEnumerable<GetAssignmentDTO>> GetAssignmentsForShark(); //for shark
        Task<IEnumerable<LynxAssignmentDTO>> GetSpecificAssignments(List<int> ids);
        Task<GetAssignmentDTO> GetAssignment(int id);
        Task<GetAssignmentDTO> GetAssignmentForShark(int id);
        Task DeleteAssignment(int assignment_id);
    }
    public class AssignmentService : IAssignmentService
    {
        private readonly DapperContext _context;
        public AssignmentService(DapperContext context)
        {
            _context = context;
        }
        public async Task AddAssignment(AddAssignmentDTO assignment)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var id = await connection.QuerySingleAsync<int>(Queries.CreateAssignment, new
                    {
                        assignment.name,
                        assignment.description,
                        assignment.points,
                        assignment.author
                    });
                    //add websites to assignment
                    if (assignment.websites.Count > 0)
                    {
                        string query = "insert into ibex.dbo.a_assignment_website (assignment_id,website_id,should_pass,is_primary) values ";
                        foreach (var website in assignment.websites)
                        {
                            int should_pass = website.should_pass ? 1 : 0;
                            query += $"({id},{website.id},{should_pass},0),";
                        }
                        query = query.Remove(query.Length - 1);
                        await connection.ExecuteAsync(query);
                    }
                    //add skeleton to assignment
                    if (assignment.skeleton_ids.Count > 0)
                    {
                        string query = "insert into ibex.dbo.a_assignment_skeleton (skeleton_id,assignment_id) values ";
                        foreach(var skeleton in assignment.skeleton_ids)
                        {
                            query += $"({skeleton},{id}),";
                        }
                        query = query.Remove(query.Length - 1);
                        await connection.ExecuteAsync(query);
                    }
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        public Task DeleteAssignment(int assignment_id)
        {
            throw new NotImplementedException();
        }

        public async Task<GetAssignmentDTO> GetAssignment(int id)
        {
            using(var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleAsync<GetAssignmentDTO>(Queries.GetAssignment,new {id});
            }
        }

        public async Task<GetAssignmentDTO> GetAssignmentForShark(int id)
        {
            using(var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleAsync<GetAssignmentDTO>(Queries.GetAssignmnetForShark,new {id});
            }
        }

        public async Task<IEnumerable<GetAssignmentDTO>> GetAssignments()
        {
            using(var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<GetAssignmentDTO>(Queries.GetAssignments);
            }
        }

        public async Task<IEnumerable<GetAssignmentDTO>> GetAssignmentsForShark()
        {
            using(var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<GetAssignmentDTO>(Queries.GetAssignmentsForShark);
            }
        }

        public async Task<IEnumerable<LynxAssignmentDTO>> GetSpecificAssignments(List<int> ids)
        {
            //var query = "select assignment_id as id,hosted_at as website_link* from ibex.dbo.assignment where ";
            var query = Queries.GetAssignments;
            query += " and (";
            foreach(var id in ids)
            {
                query += $"a.assignment_id = {id} or ";
            }

            query = query.Remove(query.Length - 3);
            query += ")";
            using(var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<LynxAssignmentDTO>(query);
            }
        }

        public async Task<SubmissionContext> GetSubmissionContext(int assignment_id,int testing_fw_id)
        {
            var parameters = new {assignment_id,testing_fw_id }; 
            using (var connection = _context.CreateConnection())
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

        public async Task UpdateAssignment(UpdateAssignmentDTO assignment)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    await connection.ExecuteAsync(Queries.UpdateAssignment, new
                    {
                        assignment.id,
                        assignment.name,
                        assignment.description,
                        assignment.points,
                        assignment.author
                    });
                    //remove websites for assignment
                    await connection.ExecuteAsync(Queries.RemoveAssignmentWebsites, new {assignment.id});
                    //add websites to assignment
                    if (assignment.websites.Count > 0)
                    {
                        string query = "insert into ibex.dbo.a_assignment_website (assignment_id,website_id,should_pass,is_primary) values ";
                        foreach (var website in assignment.websites)
                        {
                            int should_pass = website.should_pass ? 1 : 0;
                            query += $"({assignment.id},{website.id},{should_pass},0),";
                        }
                        query = query.Remove(query.Length - 1);
                        await connection.ExecuteAsync(query);
                    }
                    //remove skeletons for assignment
                    await connection.ExecuteAsync(Queries.RemoveAssignmentSkeletons, new {assignment.id});
                    //add skeleton to assignment
                    if (assignment.skeleton_ids.Count > 0)
                    {
                        string query = "insert into ibex.dbo.a_assignment_skeleton (skeleton_id,assignment_id) values ";
                        foreach(var skeleton in assignment.skeleton_ids)
                        {
                            query += $"({skeleton},{assignment.id}),";
                        }
                        query = query.Remove(query.Length - 1);
                        await connection.ExecuteAsync(query);
                    }
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
    }
}
