
namespace Fit4Job.Repositories.Interfaces
{
    public interface INotificationRepository:IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId);
        Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(int userId);
        Task MarkAsReadAsync(int notificationId);
        Task MarkAllAsReadAsync(int userId);
        Task<IEnumerable<Notification>> GetRecentNotificationsAsync(int userId, int limit);
        Task DeleteAllNotificationsForUserAsync(int userId);
    }
}
