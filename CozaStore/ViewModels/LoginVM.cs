using System.ComponentModel.DataAnnotations;

namespace CozaStore.ViewModels
{
    public class LoginVM
    {
        public required string UserName { get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
