namespace CozaStore.DTOs
{
    public class PermissionItemDto
    {
        public required string Name { get; set; }
        public string ClaimType { get; set; } = "Permission";
        public required string ClaimValue { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
