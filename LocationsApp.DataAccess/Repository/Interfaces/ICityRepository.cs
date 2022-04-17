using LocationsApp.DataAccess.Models;
using LocationsApp.DataAccess.Repository.Interfaces.Base;
using LocationsApp.DataAccess.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationsApp.DataAccess.Repository.Interfaces
{
    public interface ICityRepository : IRepository<CityModel, int>
    {
        Task<IEnumerable<CityModel>> GetDisplayListAsync(Pagination pagination, string cityName = "", string stateName = "");
        Task<IEnumerable<CityModel>> GetLookupListAsync(Pagination pagination, int stateId, string cityName = "");
    }
}
