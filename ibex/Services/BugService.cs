using Dapper;
using ibex.Data;
using ibex.Models;
using ibex.Models.DTO;
namespace ibex.Services
{
    public interface IBugService
    {
        Task<Bug> GetBug(int id);
        Task<IEnumerable<Bug>> GetBugs();
        Task<IEnumerable<Bug>> GetBugsForWebsite(int website_id);
        Task AddBug(AddBugDTO bug);
        Task UpdateBug(UpdateBugDTO bug,int id);
        Task DeleteBug(int id);
    }
    public class BugService : IBugService
    {
        private readonly DapperContext _context;
        public BugService(DapperContext context)
        {
            _context = context;
        }
        public Task AddBug(AddBugDTO bug)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBug(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Bug> GetBug(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Bug>> GetBugs()
        {
            using(var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Bug>(Queries.GetBugs);
            }
        }

        public async Task<IEnumerable<Bug>> GetBugsForWebsite(int website_id)
        {
            using(var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Bug>(Queries.GetBugsForWebsite, new {id=website_id});
            }
        }

        public Task UpdateBug(UpdateBugDTO bug, int id)
        {
            throw new NotImplementedException();
        }
    }
}
