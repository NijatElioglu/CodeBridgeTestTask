using CodeBridgeTestTask.Core.Entities;
using CodeBridgeTestTask.Core.Repositories;

namespace CodeBridgeTestTask.Core.Interfaces
{
    public interface IDogRepository : IGenericRepository<Dogs>
    {
        Task<IQueryable<Dogs>> GetAllWithSortingAndPagingAsync(string attribute, string order, int pageNumber, int pageSize);
         Task<Dogs> GetByNameAsync(string name);
    }
}
