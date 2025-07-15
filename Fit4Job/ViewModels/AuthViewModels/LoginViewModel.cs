namespace Fit4Job.ViewModels.AuthViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public DateTime Expiration { get; set; }

        public LoginViewModel()
        {

        }
        public LoginViewModel((string Token, DateTime Expiration) token , ApplicationUser user, int profileId)
        {
            Id = user.Id;
            Email = user.Email!;
            Token = token.Token;
            Username = user.UserName!;
            Role = user.Role.ToString();
            Expiration = token.Expiration;
            ProfileId = profileId;
        }

        public static LoginViewModel GetViewModel((string Token, DateTime Expiration) token, ApplicationUser user , int profileId)
        {
            return new LoginViewModel(token, user, profileId);
        }
    }
}
