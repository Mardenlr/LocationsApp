using LocationsApp.DataAccess.Context;
using LocationsApp.DataAccess.Models;
using LocationsApp.DataAccess.Utils;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationsApp.DataAccess.Repository
{
    public class AddressRepository : DapperBase, IAddressRepository
    {
        public AddressRepository(IConfiguration configuration) : base(configuration) { }

        public Task<int> InsertAsync(AddressModel model)
        {
            string sql = @"
                insert into dbo.Addresses(
                    ZipCode,
                    Name,
                    DistrictId,
                    CityId,
                    StateId,
                    CountryId
                ) output inserted.Id values (
                    @ZipCode,
                    @Name,
                    @DistrictId,
                    @CityId,
                    @StateId,
                    @CountryId
                )
            ";
            return SaveAndReturnIdAsync(sql, model);
        }

        public Task<int> UpdateAsync(AddressModel model)
        {
            string sql = @"
                update dbo.Addresses set
                    ZipCode = @ZipCode,
                    Name = @Name,
                    DistrictId = @DistrictId,
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
            string sql = "delete from dbo.Addresses where Id = @Id";
            return SaveAsync(sql, new { Id = id });
        }

        public Task<AddressModel> FirstOrDefaulAsync(int id)
        {
            string sql = @"
                select 
                    a.Id,
                    a.ZipCode,
                    a.Name,
                    a.DistrictId,
                    b.Name as District,
                    a.CityId,
                    c.Name as City,
                    a.StateId,
                    d.Name as State,
                    a.CountryId,
                    e.Name as Country
                from 
                    dbo.Addresses a inner join
                    dbo.Districts b on a.DistrictId = b.Id inner join
                    dbo.Cities c on a.CityId = c.Id inner join
                    dbo.States d on a.StateId = d.Id inner join
                    dbo.Countries e on a.CountryId = e.Id
                where 
                    a.Id = @Id
            ";
            return QueryFirstOrDefaultAsync<AddressModel>(sql, new { Id = id });
        }

        public Task<IEnumerable<AddressModel>> GetDisplayListAsync(Pagination pagination, string zipCode = "", string addressName = "", string cityName = "")
        {
            string sql = @"
                select 
                    a.Id,
                    a.ZipCode,
                    a.Name,
                    b.Name as District,
                    c.Name as City,
                    d.Name as State,
                    e.Name as Country
                from 
                    dbo.Addresses a inner join
                    dbo.Districts b on a.DistrictId = b.Id inner join
                    dbo.Cities c on a.CityId = c.Id inner join
                    dbo.States d on a.StateId = d.Id inner join
                    dbo.Countries e on a.CountryId = e.Id
                where
                    a.ZipCode like '%' + @ZipCode + '%'
                or  a.Name like '%' + @AddressName + '%'
                or  c.Name like '%' + @CityName + '%'
                order by
                    a.Name,
                    a.Id
                offset @Offset rows fetch next @PageSize rows only
            ";
            return QueryAsync<AddressModel>(sql, new
            {
                ZipCode = zipCode,
                AddressName = addressName,
                CityName = cityName,
                Offset = pagination.Offset,
                PageSize = pagination.PageSize
            });
        }

        public Task<bool> ExistsAsync(int id)
        {
            var sql = "select case when exists(select 1 from dbo.Addresses where Id = @Id) then 1 else 0 end";
            return ExistsAsync(sql, new { Id = id });
        }

        public Task<int> CountAsync()
        {
            var sql = "select count(*) from dbo.Addresses";
            return CountAsync(sql, null);
        }
    }
}
