using Dapper;
using lynx.Data;
using lynx.Models;

namespace lynx.Services
{
    public interface IAssignmentService
    {
        Task<Assignment> GetAssignment(int id);
        Task<IEnumerable<Assignment>> GetAllAssignments();
        Task<int> CreateAssignment(Assignment assignment);
        Task<int> UpdateAssignment(Assignment assignment);
        Task<int> DeleteAssignment(int id);
        Task<IEnumerable<int>> CreateAssignmentsBulk(List<Assignment> assignments);
    }
    public class AssignmentService : IAssignmentService
    {
        private readonly DapperContext _context;
        public AssignmentService(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAssignment(Assignment assignment)
        {
            var parameters = new
            {
                assignment.name,
                assignment.description,
                assignment.points,
                assignment.website_link,
                assignment.parent_id
            };
            using(var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleAsync<int>(Queries.CreateAssignment,parameters);
            }
        }

        public async Task<IEnumerable<int>> CreateAssignmentsBulk(List<Assignment> assignments)
        {
            if (assignments.Count > 0)
            {
                var query = "insert into kino.dbo.assignment (parent_id,name,description,points,website_link) OUTPUT inserted.assignment_id VALUES "; 
                foreach(var assignment in assignments)
                {
                    query += $"({assignment.id},'{assignment.name}','{assignment.description}',{assignment.points},'{assignment.website_link}'),";
                }
                query = query.Remove(query.Length - 1);
                using(var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<int>(query);
                }
            }
            return Enumerable.Empty<int>();
        }

        public async Task<int> DeleteAssignment(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(Queries.DeleteAssignment,new {id});
            }
        }

        public async Task<IEnumerable<Assignment>> GetAllAssignments()
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Assignment>(Queries.GetAllAssignments);
            }
        }

        public async Task<Assignment> GetAssignment(int id)
        {
            var parameters = new { id };
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleAsync<Assignment>(Queries.GetAssignment,parameters);
            }
        }

        public async Task<int> UpdateAssignment(Assignment assignment)
        {
            var parameters = new {
                assignment.id,
                assignment.name,
                assignment.description,
                assignment.points,
            };
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(Queries.UpdateAssignment,parameters);
            }

        }
    }
}
