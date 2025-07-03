namespace Fit4Job.Repositories.Generic
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        Task AddAsync(TModel entity);
        Task<IDbContextTransaction> BeginTransactionAsync();
        void Delete(TModel entity);
        Task<bool> DeleteById(int id);
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel?> GetByIdAsync(int id);
        Task SaveChangesAsync();
        //Task<bool> SoftDeleteAsync(int id);
        void Update(TModel entity);
    }
}