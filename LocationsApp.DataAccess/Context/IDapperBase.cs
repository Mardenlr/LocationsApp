using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationsApp.DataAccess.Context
{
    public interface IDapperBase
    {
        Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string sql, object parameters);
        Task<TEntity> QueryFirstOrDefaultAsync<TEntity>(string sql, object parameters);
        Task<TType> QueryScalarAsync<TType>(string sql, object parameters);
        Task<int> SaveAsync(string sql, object parameters);
        Task<int> SaveAndReturnIdAsync(string sql, object parameters);
        Task<int> CountAsync(string sql, object parameters);
        Task<bool> ExistsAsync(string sql, object parameters);
    }
}
