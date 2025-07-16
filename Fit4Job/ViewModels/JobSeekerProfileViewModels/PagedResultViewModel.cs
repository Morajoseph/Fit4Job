namespace Fit4Job.ViewModels.JobSeekerProfileViewModels
{
    public class PagedResultViewModel<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
    }
}
