using System.Threading.Tasks;

namespace LocationsApp.DataAccess.Repository.Interfaces.Base
{
    public interface IBaseForCount
    {
        Task<int> CountAsync();
    }
}
