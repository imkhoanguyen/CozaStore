using CozaStore.Models;

namespace CozaStore.ViewModels
{
    public class UserRoleVM
    {
        public required string userId { get; set; }
        public string? SelectedRoleName { get; set; }
        public required List<AppRole> RoleList { get; set; }
    }
}
