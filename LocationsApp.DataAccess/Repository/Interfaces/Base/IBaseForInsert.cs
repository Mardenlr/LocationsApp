using System.Threading.Tasks;

namespace LocationsApp.DataAccess.Repository.Interfaces.Base
{
    public interface IBaseForInsert<TModel> where TModel : class
    {
        Task<int> InsertAsync(TModel model);
    }
}
