using System.Threading.Tasks;

namespace LocationsApp.DataAccess.Repository.Interfaces.Base
{
    public interface IBaseForDelete<TKey>
    {
        Task<int> DeleteAsync(TKey id);
    }

    public interface IBaseForDelete<TKey, TKey2>
    {
        Task<int> DeleteAsync(TKey id, TKey2 id2);
    }
}
