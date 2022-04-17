using LocationsApp.DataAccess.Models;
using LocationsApp.DataAccess.Repository.Interfaces.Base;
using LocationsApp.DataAccess.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationsApp.DataAccess.Repository
{
    public interface IAddressRepository : IRepository<AddressModel, int>
    {
        Task<IEnumerable<AddressModel>> GetDisplayListAsync(Pagination pagination, string zipCode = "", string addressName = "", string cityName = "");
    }
}
