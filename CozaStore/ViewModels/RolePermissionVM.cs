using CozaStore.DTOs;
using CozaStore.Models;

namespace CozaStore.ViewModels
{
    public class RolePermissionVM
    {
        public AppRole? AppRole { get; set; }
        public List<PermissionGroupDto>? PermissionGroups { get; set; }
        public List<string>? SelectedClaimValueList { get; set; }
    }
}
