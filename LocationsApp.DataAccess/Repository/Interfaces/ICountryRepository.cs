using LocationsApp.DataAccess.Models;
using LocationsApp.DataAccess.Repository.Interfaces.Base;
using LocationsApp.DataAccess.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationsApp.DataAccess.Repository.Interfaces
{
    public interface ICountryRepository : IRepository<CountryModel, int>
    {
        Task<IEnumerable<CountryModel>> GetDisplayListAsync(Pagination pagination, string countryName = "");
        Task<IEnumerable<CountryModel>> GetLookupListAsync(Pagination pagination, string countryName = "");
    }
}
