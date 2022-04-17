using LocationsApp.DataAccess.Context;
using LocationsApp.DataAccess.Models;
using LocationsApp.DataAccess.Repository.Interfaces;
using LocationsApp.DataAccess.Utils;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationsApp.DataAccess.Repository
{
    public class StateRepository : DapperBase, IStateRepository
    {
        public StateRepository(IConfiguration configuration) : base(configuration) { }

        public Task<int> InsertAsync(StateModel model)
        {
            string sql = @"
                insert into dbo.States(
                    ShortCode,
                    Name,
                    Capital,
                    CountryId
                ) output inserted.Id values (
                    @ShortCode,
                    @Name,
                    @Capital,
                    @CountryId
                )
            ";
            return SaveAndReturnIdAsync(sql, model);
        }

        public Task<int> UpdateAsync(StateModel model)
        {
            string sql = @"
                update dbo.States set
                    ShortCode = @ShortCode,
                    Name = @Name,
                    Capital = @Capital,
                    CountryId = @CountryId
                where
                    Id = @Id
            ";
            return SaveAsync(sql, model);
        }

        public Task<int> DeleteAsync(int id)
        {
            string sql = "delete from dbo.States where Id = @Id";
            return SaveAsync(sql, new { Id = id });
        }

        public Task<StateModel> FirstOrDefaulAsync(int id)
        {
            string sql = @"
                select 
                    a.Id,
                    a.ShortCode,
                    a.Name,
                    a.Capital,
                    a.CountryId,
                    b.Name as Country
                from 
                    dbo.States a inner join
                    dbo.Countries b on a.CountryId = b.Id
                where 
                    a.Id = @Id
            ";
            return QueryFirstOrDefaultAsync<StateModel>(sql, new { Id = id });
        }

        public Task<IEnumerable<StateModel>> GetDisplayListAsync(Pagination pagination, string stateName = "", string countryName = "")
        {
            string sql = @"
                select 
                    a.Id,
                    a.ShortCode,
                    a.Name,
                    a.Capital,
                    b.Name as Country
                from 
                    dbo.States a inner join
                    dbo.Countries b on a.CountryId = b.Id
                where
                    a.Name like '%' + @StateName + '%'
                or  b.Name like '%' + @CountryName + '%'
                order by
                    a.Name,
                    a.Id
                offset @Offset rows fetch next @PageSize rows only
            ";
            return QueryAsync<StateModel>(sql, new
            {
                StateName = stateName,
                CountryName = countryName,
                Offset = pagination.Offset,
                PageSize = pagination.PageSize
            });
        }

        public Task<IEnumerable<StateModel>> GetLookupListAsync(Pagination pagination, int countryId, string stateName = "")
        {
            string sql = @"
                select 
                    Id,
                    Name
                from 
                    dbo.States
                where
                    CountryId = @CountryId
                and Name LIKE '%' + @Name + '%'
                order by 
                    Name,
                    Id
                offset @Offset rows fetch next @PageSize rows only
            ";
            return QueryAsync<StateModel>(sql, new
            {
                CountryId = countryId,
                Name = stateName,
                Offset = pagination.Offset,
                PageSize = pagination.PageSize
            });
        }

        public Task<bool> ExistsAsync(int id)
        {
            var sql = "select case when exists(select 1 from dbo.States where Id = @Id) then 1 else 0 end";
            return ExistsAsync(sql, new { Id = id });
        }

        public Task<int> CountAsync()
        {
            var sql = "select count(*) from dbo.States";
            return CountAsync(sql, null);
        }
    }
}
