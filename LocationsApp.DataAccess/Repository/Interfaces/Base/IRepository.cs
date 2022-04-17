namespace LocationsApp.DataAccess.Repository.Interfaces.Base
{
    public interface IRepository<TModel, TKey> :
        IBaseForInsert<TModel>,
        IBaseForUpdate<TModel>,
        IBaseForDelete<TKey>,
        IBaseForFirstOrDefault<TModel, TKey>,
        IBaseForExists<TKey>,
        IBaseForCount 
        where TModel : class
    {
        // for others custom methods
    }

    public interface IBaseRepository<TModel, TKey, TKey2> :
        IBaseForInsert<TModel>,
        IBaseForUpdate<TModel>,
        IBaseForDelete<TKey, TKey2>,
        IBaseForFirstOrDefault<TModel, TKey, TKey2>,
        IBaseForExists<TKey, TKey2>,
        IBaseForCount
        where TModel: class
    {
        // for others custom methods
    }
}
