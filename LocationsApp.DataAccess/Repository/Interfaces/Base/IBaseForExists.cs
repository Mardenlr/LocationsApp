using System.Threading.Tasks;

namespace LocationsApp.DataAccess.Repository.Interfaces.Base
{
    public interface IBaseForExists<TKey>
    {
        Task<bool> ExistsAsync(TKey id);
    }

    public interface IBaseForExists<TKey, TKey2>
    {
        Task<bool> ExistsAsync(TKey id, TKey2 id2);
    }
}
