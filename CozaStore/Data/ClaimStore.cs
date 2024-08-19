using CozaStore.DTOs;
using CozaStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CozaStore.Data
{
    public static class ClaimStore
    {
        public const string Product_View = "Product.View";
        public const string Product_Create = "Product.Create";
        public const string Product_Edit = "Product.Edit";
        public const string Product_Delete = "Product.Delete";
        public const string Product_Detail = "Product.Detail";

        public const string Color_View = "Color.View";
        public const string Color_Create = "Color.Create";
        public const string Color_Edit = "Color.Edit";
        public const string Color_Delete = "Color.Delete";

        public const string Size_View = "Size.View";
        public const string Size_Create = "Size.Create";
        public const string Size_Edit = "Size.Edit";
        public const string Size_Delete = "Size.Delete";

        public const string Category_View = "Category.View";
        public const string Category_Create = "Category.Create";
        public const string Category_Edit = "Category.Edit";
        public const string Category_Delete = "Category.Delete";

        public const string Role_View = "Role.View";
        public const string Role_Create = "Role.Create";
        public const string Role_Edit = "Role.Edit";
        public const string Role_Delete = "Role.Delete";

        public const string User_View = "User.View";
        public const string User_Create = "User.Create";
        public const string User_Edit = "User.Edit";
        public const string User_Delete = "User.Delete";


        public static List<IdentityRoleClaim<string>> adminClaims = new List<IdentityRoleClaim<string>>()
        {
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Product_View},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Product_Create},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Product_Edit},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Product_Delete},

            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Category_View},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Category_Create},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Category_Edit},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Category_Delete},

            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Color_View},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Color_Create},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Color_Edit},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Color_Delete},

            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Size_View},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Size_Create},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Size_Edit},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Size_Delete},

            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Role_View},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Role_Create},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Role_Edit},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Role_Delete},

            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=User_View},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=User_Create},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=User_Edit},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=User_Delete},
        };


        public static List<IdentityRoleClaim<string>> customerClaims = new List<IdentityRoleClaim<string>>()
        {
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Product_View},
        };


        public static List<PermissionGroupDto> AllPermissionGroups = new List<PermissionGroupDto>() {
            new PermissionGroupDto
            {
                GroupName = "Product Management",
                Permissions = new List<PermissionItemDto>
                {
                    new PermissionItemDto {Name = "View Product", ClaimValue = Product_View},
                    new PermissionItemDto {Name = "Create Product", ClaimValue = Product_Create},
                    new PermissionItemDto {Name = "Edit Product", ClaimValue = Product_Edit},
                    new PermissionItemDto {Name = "Delete Product", ClaimValue = Product_Delete},
                }
            },

            new PermissionGroupDto
            {
                GroupName = "Category Management",
                Permissions = new List<PermissionItemDto>
                {
                    new PermissionItemDto {Name = "View Category", ClaimValue = Category_View},
                    new PermissionItemDto {Name = "Create Category", ClaimValue = Category_Create},
                    new PermissionItemDto {Name = "Edit Category", ClaimValue = Category_Edit},
                    new PermissionItemDto {Name = "Delete Category", ClaimValue = Category_Delete},
                }
            },

            new PermissionGroupDto
            {
                GroupName = "Color Management",
                Permissions = new List<PermissionItemDto>
                {
                    new PermissionItemDto {Name = "View Color", ClaimValue = Color_View},
                    new PermissionItemDto {Name = "Create Color", ClaimValue = Color_Create},
                    new PermissionItemDto {Name = "Edit Color", ClaimValue = Color_Edit},
                    new PermissionItemDto {Name = "Delete Color", ClaimValue = Color_Delete},
                }
            },

            new PermissionGroupDto
            {
                GroupName = "Size Management",
                Permissions = new List<PermissionItemDto>
                {
                    new PermissionItemDto {Name = "View Size", ClaimValue = Size_View},
                    new PermissionItemDto {Name = "Create Size", ClaimValue = Size_Create},
                    new PermissionItemDto {Name = "Edit Size", ClaimValue = Size_Edit},
                    new PermissionItemDto {Name = "Delete Size", ClaimValue = Size_Delete},
                }
            },

            new PermissionGroupDto
            {
                GroupName = "Role Management",
                Permissions = new List<PermissionItemDto>
                {
                    new PermissionItemDto {Name = "View Role", ClaimValue = Role_View},
                    new PermissionItemDto {Name = "Create Role", ClaimValue = Role_Create},
                    new PermissionItemDto {Name = "Edit Role", ClaimValue = Role_Edit},
                    new PermissionItemDto {Name = "Delete Role", ClaimValue = Role_Delete},
                }
            },

            new PermissionGroupDto
            {
                GroupName = "User Management",
                Permissions = new List<PermissionItemDto>
                {
                    new PermissionItemDto {Name = "View User", ClaimValue = User_View},
                    new PermissionItemDto {Name = "Create User", ClaimValue = User_Create},
                    new PermissionItemDto {Name = "Edit User", ClaimValue = User_Edit},
                    new PermissionItemDto {Name = "Delete User", ClaimValue = User_Delete},
                }
            },
        };
        
    }
}
