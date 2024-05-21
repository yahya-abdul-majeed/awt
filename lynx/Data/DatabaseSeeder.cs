using Dapper;

namespace lynx.Data
{
    public class DatabaseSeeder
    {
        private readonly DapperContext _context;

        public DatabaseSeeder(DapperContext context)
        {
            _context = context;
        }

        public async Task SeedDB()
        {
            using(var conn = _context.CreateMasterConnection())
            {
                var records = await conn.QueryAsync(Queries.DatabaseExists);
                if (!records.Any())
                    await conn.ExecuteAsync(Queries.SeedQuery);
            }
        }
    }
}
