using LocationsApp.DataAccess.Context;
using LocationsApp.DataAccess.Models;
using LocationsApp.DataAccess.Repository.Interfaces;
using LocationsApp.DataAccess.Utils;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationsApp.DataAccess.Repository
{
    public class CityRepository : DapperBase, ICityRepository
    {
        public CityRepository(IConfiguration configuration) : base(configuration) { }

        public Task<int> InsertAsync(CityModel model)
        {
            string sql = @"
                insert into dbo.Cities(
                    Name,
                    StateId,
                    CountryId
                ) output inserted.Id values (
                    @Name,
                    @StateId,
                    @CountryId
                )
            ";
            return SaveAndReturnIdAsync(sql, model);
        }

        public Task<int> UpdateAsync(CityModel model)
        {
            string sql = @"
                update dbo.Cities set
                    Name = @Name,
                    StateId = @StateId,
                    CountryId = @CountryId
                where
                    Id = @Id
            ";
            return SaveAsync(sql, model);
        }

        public Task<int> DeleteAsync(int id)
        {
            string sql = "delete from dbo.Cities where Id = @Id";
            return SaveAsync(sql, new { Id = id });
        }

        public Task<CityModel> FirstOrDefaulAsync(int id)
        {
            string sql = @"
                select 
                    a.Id,
                    a.Name,
                    a.StateId,
                    b.Name as State,
                    a.CountryId,
                    c.Name as Country
                from 
                    dbo.Cities a inner join
                    dbo.States b on a.StateId = b.Id inner join
                    dbo.Countries c on a.CountryId = c.Id
                where 
                    a.Id = @Id
            ";
            return QueryFirstOrDefaultAsync<CityModel>(sql, new { Id = id });
        }

        public Task<IEnumerable<CityModel>> GetDisplayListAsync(Pagination pagination, string cityName = "", string stateName = "")
        {
            string sql = @"
                select 
                    a.Id,
                    a.Name,
                    b.Name as State,
                    c.Name as Country
                from 
                    dbo.Cities a inner join
                    dbo.States b on a.StateId = b.Id inner join
                    dbo.Countries c on a.CountryId = c.Id
                where
                    a.Name like '%' + @CityName + '%'
                or  b.Name like '%' + @StateName + '%'
                order by
                    a.Name,
                    a.Id
                offset @Offset rows fetch next @PageSize rows only
            ";
            return QueryAsync<CityModel>(sql, new
            {
                CityName = stateName,
                StateName = stateName,
                Offset = pagination.Offset,
                PageSize = pagination.PageSize
            });
        }

        public Task<IEnumerable<CityModel>> GetLookupListAsync(Pagination pagination, int stateId, string cityName = "")
        {
            string sql = @"
                select 
                    Id,
                    Name
                from 
                    dbo.Cities
                where
                    StateId = @StateId
                and Name LIKE '%' + @Name + '%'
                order by 
                    Name,
                    Id
                offset @Offset rows fetch next @PageSize rows only
            ";
            return QueryAsync<CityModel>(sql, new
            {
                StateId = stateId,
                Name = cityName,
                Offset = pagination.Offset,
                PageSize = pagination.PageSize
            });
        }

        public Task<bool> ExistsAsync(int id)
        {
            var sql = "select case when exists(select 1 from dbo.Cities where Id = @Id) then 1 else 0 end";
            return ExistsAsync(sql, new { Id = id });
        }

        public Task<int> CountAsync()
        {
            var sql = "select count(*) from dbo.Cities";
            return CountAsync(sql, null);
        }
    }
}
