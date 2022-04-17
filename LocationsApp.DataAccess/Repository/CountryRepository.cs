using LocationsApp.DataAccess.Context;
using LocationsApp.DataAccess.Models;
using LocationsApp.DataAccess.Repository.Interfaces;
using LocationsApp.DataAccess.Utils;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationsApp.DataAccess.Repository
{
    public class CountryRepository : DapperBase, ICountryRepository
    {
        public CountryRepository(IConfiguration configuration) : base(configuration) { }

        public Task<int> InsertAsync(CountryModel model)
        {
            string sql = @"
                insert into dbo.Countries(
                    Name
                ) output inserted.Id values (
                    @Name
                )
            ";
            return SaveAndReturnIdAsync(sql, model);
        }

        public Task<int> UpdateAsync(CountryModel model)
        {
            string sql = @"
                update dbo.Countries set
                    Name = @Name
                where
                    Id = @Id
            ";
            return SaveAsync(sql, model);
        }

        public Task<int> DeleteAsync(int id)
        {
            string sql = "delete from dbo.Countries where Id = @Id";
            return SaveAsync(sql, new { Id = id });
        }

        public Task<CountryModel> FirstOrDefaulAsync(int id)
        {
            string sql = @"
                select 
                    Id,
                    Name
                from 
                    dbo.Countries
                where 
                    Id = @Id
            ";
            return QueryFirstOrDefaultAsync<CountryModel>(sql, new { Id = id });
        }

        public Task<IEnumerable<CountryModel>> GetDisplayListAsync(Pagination pagination, string CountryName = "")
        {
            string sql = @"
                select 
                    Id,
                    Name
                from 
                    dbo.Countries
                where 
                    Name LIKE '%' + @Name + '%'
                order by 
                    Name,
                    Id
                offset @Offset rows fetch next @PageSize rows only
            ";
            return QueryAsync<CountryModel>(sql, new
            {
                Name = CountryName,
                Offset = pagination.Offset,
                PageSize = pagination.PageSize
            });
        }

        public Task<IEnumerable<CountryModel>> GetLookupListAsync(Pagination pagination, string CountryName = "")
        {
            string sql = @"
                select 
                    Id,
                    Name
                from 
                    dbo.Countries
                where 
                    Name LIKE '%' + @Name + '%'
                order by 
                    Name,
                    Id
                offset @Offset rows fetch next @PageSize rows only
            ";
            return QueryAsync<CountryModel>(sql, new
            {
                Name = CountryName,
                Offset = pagination.Offset,
                PageSize = pagination.PageSize
            });
        }

        public Task<bool> ExistsAsync(int id)
        {
            var sql = "select case when exists(select 1 from dbo.Countries where Id = @Id) then 1 else 0 end";
            return ExistsAsync(sql, new { Id = id });
        }

        public Task<int> CountAsync()
        {
            var sql = "select count(*) from dbo.Countries";
            return CountAsync(sql, null);
        }
    }
}
