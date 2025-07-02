using Fit4Job.Repositories.Generic;
using Fit4Job.Repositories.Interfaces;

namespace Fit4Job.Repositories.Implementations
{
    public class CompanyTaskRepository : GenericRepository<CompanyTask>, ICompanyTaskRepository
    {
        public CompanyTaskRepository(Fit4JobDbContext context) : base(context)
        {
        }
    }
}
