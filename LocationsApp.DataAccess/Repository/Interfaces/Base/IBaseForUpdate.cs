using System.Threading.Tasks;

namespace LocationsApp.DataAccess.Repository.Interfaces.Base
{
    public interface IBaseForUpdate<TModel> where TModel : class
    {
        Task<int> UpdateAsync(TModel model);
    }
}
