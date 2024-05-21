using Dapper; 
using ibex.Data;
using ibex.Models;
using ibex.Models.DTO;

namespace ibex.Services
{
    public interface IToolService
    {
        Task AddLanguage(Language language);
        Task UpdateLanguage(Language language);
        Task AddFramework(AddFrameworkDTO framework);
        Task UpdateFramework(UpdateFrameworkDTO framework);
    }
    public class ToolService : IToolService
    {
        private readonly DapperContext _context;
        public ToolService(DapperContext context)
        {
            _context = context;
        }
        public async Task AddFramework(AddFrameworkDTO framework)
        {
            using(var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(Queries.AddFramework,framework);
            }
        }

        public async Task AddLanguage(Language language)
        {
            using(var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(Queries.AddLanguage, new
                {
                    language.name
                });
            }
        }

        public async Task UpdateFramework(UpdateFrameworkDTO framework)
        {
            using(var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(Queries.UpdateFramework,framework);
            }
        }

        public async Task UpdateLanguage(Language language)
        {
            using(var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(Queries.UpdateLanguage, language);
            }

        }
    }
}
