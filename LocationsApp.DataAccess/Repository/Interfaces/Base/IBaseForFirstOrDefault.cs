using System.Threading.Tasks;

namespace LocationsApp.DataAccess.Repository.Interfaces.Base
{
    public interface IBaseForFirstOrDefault<TModel, TKey> where TModel : class
    {
        Task<TModel> FirstOrDefaulAsync(TKey id);
    }

    public interface IBaseForFirstOrDefault<TModel, TKey, TKey2> where TModel : class
    {
        Task<TModel> FirstOrDefaulAsync(TKey id, TKey2 id2);
    }
}
