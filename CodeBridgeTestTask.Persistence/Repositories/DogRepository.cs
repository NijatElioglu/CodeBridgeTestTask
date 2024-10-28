using CodeBridgeTestTask.Core.Entities;
using CodeBridgeTestTask.Core.Interfaces;
using CodeBridgeTestTask.Persistence.DAL;
using Microsoft.EntityFrameworkCore;

namespace CodeBridgeTestTask.Persistence.Repositories
{
    public class DogRepository : GenericRepository<Dogs>, IDogRepository
    {
        public DogRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IQueryable<Dogs>> GetAllWithSortingAndPagingAsync(string attribute, string order, int pageNumber, int pageSize)
        {
            var query = _context.Set<Dogs>().AsQueryable();

            query = attribute.ToLower() switch
            {
                "weight" => order.ToLower() == "desc"
                    ? query.OrderByDescending(d => EF.Property<double>(d, "Weight"))
                    : query.OrderBy(d => EF.Property<double>(d, "Weight")),

                "taillength" => order.ToLower() == "desc"
                    ? query.OrderByDescending(d => EF.Property<double>(d, "TailLenght"))
                    : query.OrderBy(d => EF.Property<double>(d, "TailLenght")),

                _ => order.ToLower() == "desc"
                    ? query.OrderByDescending(d => EF.Property<object>(d, "Name"))
                    : query.OrderBy(d => EF.Property<object>(d, "Name")),
            };

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return await Task.FromResult(query);
        }

        public async Task<Dogs> GetByNameAsync(string name)
        {
            return await _context.Set<Dogs>().FirstOrDefaultAsync(d => d.Name.ToLower() == name.ToLower());
        }
    }
}