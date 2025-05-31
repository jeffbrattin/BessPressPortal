using BessPressPortal.Shared.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BessPressPortal.Api.Data
{
    public class TestRepository : ITestRepository
    {

        private readonly string _connectionString;

        public TestRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);


        public async Task<IEnumerable<TestDto>> GetAllAsync()
        {
            var sql = "SELECT recid, sd_date, sv_text FROM tbl_test";
            using var connection = CreateConnection();
            return await connection.QueryAsync<TestDto>(sql);
        }


    }
}
