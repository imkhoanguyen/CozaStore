namespace CozaStore.ViewModels
{
    public class LoginVM
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public bool RememberMe { get; set; } = false;

    }
}
