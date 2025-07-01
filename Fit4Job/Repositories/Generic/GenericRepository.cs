namespace Fit4Job.Repositories.Generic
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        // Data source 
        private readonly DbSet<TModel> _dbSet;
        protected readonly Fit4JobDbContext _context;


        public GenericRepository(Fit4JobDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TModel>();
        }
        public async Task AddAsync(TModel entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public void Delete(TModel entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<bool> DeleteById(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TModel?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(TModel entity)
        {
            _dbSet.Update(entity);
        }
    }
}