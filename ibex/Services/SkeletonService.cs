using Dapper;
using ibex.Data;
using ibex.Models;
using ibex.Models.DTO;

namespace ibex.Services
{
    public interface ISkeletonService
    {
        Task<IEnumerable<SkeletonDTO>> GetSkeletons();
        Task<SkeletonDTO> GetSkeleton(int id);
        Task AddSkeleton(AddSkeletonDTO skeleton);
        Task UpdateSkeleton(UpdateSkeletonDTO skeleton);
        Task<IEnumerable<SkeletonDTO>> GetSkeletonsForAssignment(int assignment_id);
        Task<IEnumerable<TestingFrameworkDTO>> GetTestingFrameworks();
        Task<IEnumerable<Language>> GetLanguages();
    }
    public class SkeletonService : ISkeletonService
    {
        private readonly DapperContext _context;
        public SkeletonService(DapperContext context)
        {
            _context = context;
        }

        public async Task<SkeletonDTO> GetSkeleton(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleAsync<SkeletonDTO>(Queries.GetSkeleton, new {id});
            }
        }

        public async Task<IEnumerable<SkeletonDTO>> GetSkeletonsForAssignment(int assignment_id)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<SkeletonDTO>(Queries.GetSkeletonsForAssignment, new {id=assignment_id});
            }
        }

        public async Task<IEnumerable<SkeletonDTO>> GetSkeletons()
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<SkeletonDTO>(Queries.GetSkeletons);
            }
        }

        public async Task<IEnumerable<TestingFrameworkDTO>> GetTestingFrameworks()
        {
            using(var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<TestingFrameworkDTO>(Queries.GetTestingFrameworks);
            }
        }

        public async Task AddSkeleton(AddSkeletonDTO skeleton)
        {
            using(var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(Queries.AddSkeleton,skeleton);
            }
        }

        public async Task UpdateSkeleton(UpdateSkeletonDTO skeleton)
        {
            using(var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(Queries.UpdateSkeleton,skeleton);
            }
        }

        public async Task<IEnumerable<Language>> GetLanguages()
        {
            using(var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Language>(Queries.GetLanguages);
            }
        }
    }
}
