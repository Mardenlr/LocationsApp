using LocationsApp.DataAccess.Context;
using LocationsApp.DataAccess.Models;
using LocationsApp.DataAccess.Repository.Interfaces;
using LocationsApp.DataAccess.Utils;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationsApp.DataAccess.Repository
{
    public class DistrictRepository : DapperBase, IDistrictRepository
    {
        public DistrictRepository(IConfiguration configuration) : base(configuration) { }

        public Task<int> InsertAsync(DistrictModel model)
        {
            string sql = @"
                insert into dbo.Districts(
                    Name,
                    CityId,
                    StateId,
                    CountryId
                ) output inserted.Id values (
                    @Name,
                    @CityId,
                    @StateId,
                    @CountryId
                )
            ";
            return SaveAndReturnIdAsync(sql, model);
        }

        public Task<int> UpdateAsync(DistrictModel model)
        {
            string sql = @"
                update dbo.Districts set
                    Name = @Name,
                    CityId = @CityId,
                    StateId = @StateId,
                    CountryId = @CountryId
                where
                    Id = @Id
            ";
            return SaveAsync(sql, model);
        }

        public Task<int> DeleteAsync(int id)
        {
            string sql = "delete from dbo.Districts where Id = @Id";
            return SaveAsync(sql, new { Id = id });
        }

        public Task<DistrictModel> FirstOrDefaulAsync(int id)
        {
            string sql = @"
                select 
                    a.Id,
                    a.Name,
                    a.CityId,
                    b.Name as City,
                    a.StateId,
                    c.Name as State,
                    a.CountryId,
                    d.Name as Country
                from 
                    dbo.Districts a inner join
                    dbo.Cities b on a.CityId = b.Id inner join
                    dbo.States c on a.StateId = c.Id inner join
                    dbo.Countries d on a.CountryId = d.Id
                where 
                    a.Id = @Id
            ";
            return QueryFirstOrDefaultAsync<DistrictModel>(sql, new { Id = id });
        }

        public Task<IEnumerable<DistrictModel>> GetDisplayListAsync(Pagination pagination, string districtName = "", string cityName = "")
        {
            string sql = @"
                select 
                    a.Id,
                    a.Name,
                    b.Name as City,
                    c.Name as State,
                    d.Name as Country
                from 
                    dbo.Districts a inner join
                    dbo.Cities b on a.CityId = b.Id inner join
                    dbo.States c on a.StateId = c.Id inner join
                    dbo.Countries d on a.CountryId = d.Id
                where
                    a.Name like '%' + @DistrictName + '%'
                or  b.Name like '%' + @CityName + '%'
                order by
                    a.Name,
                    a.Id
                offset @Offset rows fetch next @PageSize rows only
            ";
            return QueryAsync<DistrictModel>(sql, new
            {
                DistrictName = districtName,
                CityName = cityName,
                Offset = pagination.Offset,
                PageSize = pagination.PageSize
            });
        }

        public Task<IEnumerable<DistrictModel>> GetLookupListAsync(Pagination pagination, int cityId, string districtName = "")
        {
            string sql = @"
                select 
                    Id,
                    Name
                from 
                    dbo.Districts
                where
                    CityId = @CityId
                and Name like '%' + @Name + '%'
                order by 
                    Name,
                    Id
                offset @Offset rows fetch next @PageSize rows only
            ";
            return QueryAsync<DistrictModel>(sql, new
            {
                CityId = cityId,
                Name = districtName,
                Offset = pagination.Offset,
                PageSize = pagination.PageSize
            });
        }

        public Task<bool> ExistsAsync(int id)
        {
            var sql = "select case when exists(select 1 from dbo.Districts where Id = @Id) then 1 else 0 end";
            return ExistsAsync(sql, new { Id = id });
        }

        public Task<int> CountAsync()
        {
            var sql = "select count(*) from dbo.Districts";
            return CountAsync(sql, null);
        }
    }
}
