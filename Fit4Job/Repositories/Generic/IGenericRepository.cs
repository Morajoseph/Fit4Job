namespace Fit4Job.Repositories.Generic
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        Task AddAsync(TModel entity);
        void Delete(TModel entity);
        Task<bool> DeleteById(int id);
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel?> GetByIdAsync(int id);
        Task SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        void Update(TModel entity);
    }
}