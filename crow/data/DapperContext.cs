using System.Data;
using Microsoft.Data.SqlClient;
namespace crow.data;

public class DapperContext{
    private readonly string _connectionStringKino;
    private readonly string _connectionStringIbex;
    private readonly string _tempIbex;
    private readonly string _tempKino;

    public DapperContext(IConfiguration config) 
    {
        //environment variables only worked after adding them to /etc/profile
        //disable 'Enforce password expiration' on sql server

        // var builder = new ConfigurationBuilder();
        // IConfigurationRoot config = builder.AddJsonFile("appsettings.json").Build();

        _connectionStringKino = Environment.GetEnvironmentVariable("ASPNETCORE_LYNX_CS").Replace("\\\\", "\\");
        _connectionStringIbex = Environment.GetEnvironmentVariable("ASPNETCORE_IBEX_CS").Replace("\\\\", "\\");
        // Console.WriteLine(Environment.GetEnvironmentVariable("ASPNETCORE_IBEX_CS"));
        // var ev = Environment.GetEnvironmentVariable("ASPNETCORE_IBEX_CS");

        // _connectionStringKino = config.GetConnectionString("SqlConnectionKino");
        // _connectionStringIbex = config.GetConnectionString("SqlConnectionIbex");
    }

    public IDbConnection CreateConnectionKino()
        => new SqlConnection(_connectionStringKino);

    public IDbConnection CreateConnectionIbex()
        => new SqlConnection(_connectionStringIbex);
}