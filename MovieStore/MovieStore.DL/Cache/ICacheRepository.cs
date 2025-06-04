using MovieStore.Models.DTO;

namespace MovieStore.DL.Cache
{
    public interface ICacheRepository<TKey, TData> where TData : ICacheItem<TKey>
    {
        Task<IEnumerable<TData?>> FullLoad();

        Task<IEnumerable<TData?>> DifLoad(DateTime lastExecuted);
    }
}
