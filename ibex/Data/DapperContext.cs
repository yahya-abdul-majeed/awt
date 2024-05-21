using Microsoft.Data.SqlClient;
using System.Data;

namespace ibex.Data
{
    public class DapperContext
    {
        private readonly string _connectionString;
        private readonly string _masterConnectionString;

        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
            _masterConnectionString = configuration.GetConnectionString("MasterConnection");
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
        public IDbConnection CreateMasterConnection()
            => new SqlConnection(_masterConnectionString);
    }
}
