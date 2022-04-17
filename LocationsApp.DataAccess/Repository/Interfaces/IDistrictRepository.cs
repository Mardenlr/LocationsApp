using LocationsApp.DataAccess.Models;
using LocationsApp.DataAccess.Repository.Interfaces.Base;
using LocationsApp.DataAccess.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationsApp.DataAccess.Repository.Interfaces
{
    public interface IDistrictRepository : IRepository<DistrictModel, int>
    {
        Task<IEnumerable<DistrictModel>> GetDisplayListAsync(Pagination pagination, string districtName = "", string cityName = "");
        Task<IEnumerable<DistrictModel>> GetLookupListAsync(Pagination pagination, int cityId, string districtName = "");
    }
}
