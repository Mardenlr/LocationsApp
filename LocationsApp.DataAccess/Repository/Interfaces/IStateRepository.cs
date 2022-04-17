using LocationsApp.DataAccess.Models;
using LocationsApp.DataAccess.Repository.Interfaces.Base;
using LocationsApp.DataAccess.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationsApp.DataAccess.Repository.Interfaces
{
    public interface IStateRepository : IRepository<StateModel, int>
    {
        Task<IEnumerable<StateModel>> GetDisplayListAsync(Pagination pagination, string stateName = "", string countryName = "");
        Task<IEnumerable<StateModel>> GetLookupListAsync(Pagination pagination, int countryId, string stateName = "");
    }
}