namespace Fit4Job.ViewModels.UserBadgeViewModels
{
    public class UserBadgeViewModels
    {
        public int Id { get; set; }

        public int BadgeId { get; set; }

        public string BadgeName { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public DateTime EarnedAt { get; set; }

        public static UserBadgeViewModels FromModel(UserBadge badge)
        {
            return new UserBadgeViewModels
            {
                Id = badge.Id,
                BadgeId = badge.BadgeId,
                BadgeName = badge.Badge?.Name ?? "", 
                Notes = badge.Notes,
                EarnedAt = badge.EarnedAt
            };
        }
    }
}
