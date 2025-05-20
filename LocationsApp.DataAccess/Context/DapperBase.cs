using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace LocationsApp.DataAccess.Context
{
    public class DapperBase : IDapperBase
    {
        private readonly IConfiguration _configuration;

        public DapperBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection ConnectionFactory()
        {
            return new SqlConnection(_configuration.GetConnectionString("Default"));
        }

        public async Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string sql, object parameters)
        {
            using (IDbConnection connection = ConnectionFactory())
            {
                return await connection.QueryAsync<TEntity>(sql, parameters);
            }
        }

        public async Task<TEntity> QueryFirstOrDefaultAsync<TEntity>(string sql, object parameters)
        {
            using (IDbConnection connection = ConnectionFactory())
            {
                return await connection.QueryFirstOrDefaultAsync<TEntity>(sql, parameters);
            }
        }

        public async Task<TType> QueryScalarAsync<TType>(string sql, object parameters)
        {
            using (IDbConnection connection = ConnectionFactory())
            {
                return await connection.ExecuteScalarAsync<TType>(sql, parameters);
            }
        }

        public async Task<int> SaveAsync(string sql, object parameters)
        {
            using (IDbConnection connection = ConnectionFactory())
            {
                return await connection.ExecuteAsync(sql, parameters);
            }
        }

        public async Task<int> SaveAndReturnIdAsync(string sql, object parameters)
        {
            using (IDbConnection connection = ConnectionFactory())
            {
                return await connection.ExecuteScalarAsync<int>(sql, parameters);
            }
        }

        public async Task<int> CountAsync(string sql, object parameters)
        {
            using (IDbConnection connection = ConnectionFactory())
            {
                return await connection.ExecuteScalarAsync<int>(sql, parameters);
            }
        }

        public async Task<bool> ExistsAsync(string sql, object parameters)
        {
            using (IDbConnection connection = ConnectionFactory())
            {
                var result = await connection.ExecuteScalarAsync<int>(sql, parameters);
                return result > 0 ? true : false;
            }
        }
    }
}