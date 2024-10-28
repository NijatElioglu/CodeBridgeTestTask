using CodeBridgeTestTask.Core.Repositories;
using CodeBridgeTestTask.Persistence.DAL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace CodeBridgeTestTask.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

     

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

       

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync(); 
        }
        public async Task Remove(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity == null)
            {
                throw new InvalidOperationException($"Entity with ID {id} not found.");
            }
            var propertyInfo = entity.GetType().GetProperty("IsDeleted");
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(entity, true);
            }

            await _context.SaveChangesAsync();
        }

    

       
    }
}
