namespace CozaStore.DTOs
{
    public class PermissionGroupDto
    {
        public required string GroupName { get; set; }
        public required List<PermissionItemDto> Permissions { get; set; }
    }
}
