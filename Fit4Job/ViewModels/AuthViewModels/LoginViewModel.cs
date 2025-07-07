namespace Fit4Job.ViewModels.AuthViewModels
{
    public class LoginViewModel
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public IList<string> Roles { get; set; }
        public DateTime Expiration { get; set; }
    }
}
