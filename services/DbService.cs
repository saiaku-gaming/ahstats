using System.Data;
using Dapper;
using Npgsql;

namespace AHStats.services;

public class DbService(IConfiguration configuration) : IDbService
{
    private readonly IDbConnection _db = new NpgsqlConnection(configuration.GetConnectionString("AhStats"));

    public async Task<T?> GetAsync<T>(string command, object parms)
    {
        return (await _db.QueryAsync<T>(command, parms).ConfigureAwait(false)).FirstOrDefault();
    }

    public async Task<List<T>> GetAll<T>(string command, object parms)
    {
        return (await _db.QueryAsync<T>(command, parms)).ToList();
    }

    public async Task<int> EditData(string command, object parms)
    {
        return await _db.ExecuteAsync(command, parms);
    }
}