using Fit4Job.Models;
using Fit4Job.Repositories.Generic;
using Fit4Job.Repositories.Interfaces;

namespace Fit4Job.Repositories.Implementations
{
    public class CompanyTaskSubmissionRepository : GenericRepository<CompanyTaskSubmission>, ICompanyTaskSubmissionRepository
    {
        public CompanyTaskSubmissionRepository(Fit4JobDbContext context) : base(context)
        {
        }
    }
}
