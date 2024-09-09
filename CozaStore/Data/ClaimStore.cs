using CozaStore.DTOs;
using Microsoft.AspNetCore.Identity;

namespace CozaStore.Data
{
    public static class ClaimStore
    {
        // color
        public const string Color_Create = "Color.Create";
        public const string Color_Edit = "Color.Edit";
        public const string Color_Delete = "Color.Delete";

        // category
        public const string Category_Create = "Category.Create";
        public const string Category_Edit = "Category.Edit";
        public const string Category_Delete = "Category.Delete";

        // size
        public const string Size_Create = "Size.Create";
        public const string Size_Edit = "Size.Edit";
        public const string Size_Delete = "Size.Delete";

        // product
        public const string Product_Create = "Product.Create";
        public const string Product_Edit = "Product.Edit";
        public const string Product_Delete = "Product.Delete";

        // order
        public const string Order_ComfirmPayment = "Order.ConfirmPayment";
        public const string Order_Comfirm = "Order.Confirm";
        public const string Order_Delete = "Order.Delete";
        
        //shipping
        public const string Shipping_Create = "Shipping_Create";
        public const string Shipping_Delete = "Shipping_Delete";
        public const string Shipping_Edit = "Shipping_Delete";
        
        // user management
        public const string User_UpdateUserRole = "User.UpdateUserRole";
        public const string User_Lockout = "User.Lockout";

        // role management
        public const string Role_Create = "Role.Create";
        public const string Role_Edit = "Role.Edit";
        public const string Role_Delete = "Role.Delete";
        public const string Role_UpdateClaimRoles = "Role.Claims";

        // access pages
        public const string AccessOrder = "Order";
        public const string AccessUser = "User";
        public const string AccessColor = "Color";
        public const string AccessSize = "Size";
        public const string AccessProduct = "Product";
        public const string AccessRole = "Role";
        public const string AccessDashboard = "Dashboard";
        public const string AccessBlog = "Blog";
        public const string AccessCategory = "Category";
        public const string AccessShipping = "Shipping";
        public const string AccessOrderUser = "OrderUser";

        
        public const string Cart_Add = "Cart.Add";

        public static List<IdentityRoleClaim<string>> adminClaims = new List<IdentityRoleClaim<string>>()
        {

            // color 
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Color_Create},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Color_Edit},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Color_Delete},

            // category
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Category_Create},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Category_Edit},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Category_Delete},

            // size
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Size_Create},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Size_Edit},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Size_Delete},

            // product
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Product_Create},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Product_Edit},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Product_Delete},

            // order
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Order_Comfirm},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Order_ComfirmPayment},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Order_Delete},

            // shipping
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Shipping_Create},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Shipping_Delete},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Shipping_Edit},

            // user management
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=User_UpdateUserRole},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=User_Lockout},

            // role management
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Role_Create},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Role_Edit},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Role_Delete},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Role_UpdateClaimRoles},

            // access pages
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=AccessOrder},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=AccessUser},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=AccessColor},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=AccessSize},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=AccessProduct},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=AccessRole},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=AccessDashboard},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=AccessBlog},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=AccessCategory},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=AccessShipping},
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=AccessOrderUser},



            

            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Cart_Add},

        };


        public static List<IdentityRoleClaim<string>> customerClaims = new List<IdentityRoleClaim<string>>()
        {
            new IdentityRoleClaim<string> {ClaimType="Permission", ClaimValue=Cart_Add},
        };


        public static List<PermissionGroupDto> AllPermissionGroups = new List<PermissionGroupDto>() {
            new PermissionGroupDto
            {
                GroupName = "Access Page",
                Permissions = new List<PermissionItemDto>
                {
                    new PermissionItemDto {Name = "Order", ClaimValue = AccessOrder},
                    new PermissionItemDto {Name = "User", ClaimValue = AccessUser},
                    new PermissionItemDto {Name = "Color", ClaimValue = AccessColor},
                    new PermissionItemDto {Name = "Size", ClaimValue = AccessSize},
                    new PermissionItemDto {Name = "Product", ClaimValue = AccessProduct},
                    new PermissionItemDto {Name = "Role", ClaimValue = AccessRole},
                    new PermissionItemDto {Name = "Dashboard", ClaimValue = AccessDashboard},
                    new PermissionItemDto {Name = "Blog", ClaimValue = AccessBlog},
                    new PermissionItemDto {Name = "Category", ClaimValue = AccessCategory},
                    new PermissionItemDto {Name = "Shipping", ClaimValue = AccessShipping},
                    new PermissionItemDto {Name = "Order (User)", ClaimValue = AccessOrderUser},
                }
            },
            new PermissionGroupDto
            {
                GroupName = "Product Management",
                Permissions = new List<PermissionItemDto>
                {
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
                    new PermissionItemDto {Name = "Create Role", ClaimValue = Role_Create},
                    new PermissionItemDto {Name = "Edit Role", ClaimValue = Role_Edit},
                    new PermissionItemDto {Name = "Delete Role", ClaimValue = Role_Delete},
                    new PermissionItemDto {Name = "Add/Edit Role Access", ClaimValue = Role_UpdateClaimRoles}
                }
            },

            new PermissionGroupDto
            {
                GroupName = "User Management",
                Permissions = new List<PermissionItemDto>
                {
                    new PermissionItemDto {Name = "Change Role of User", ClaimValue = User_UpdateUserRole},
                    new PermissionItemDto {Name = "Lock User", ClaimValue = User_Lockout},
                }
            },

            new PermissionGroupDto
            {
                GroupName = "Order Management",
                Permissions = new List<PermissionItemDto>
                {
                    new PermissionItemDto {Name = "Confirm Payment", ClaimValue = Order_ComfirmPayment},
                    new PermissionItemDto {Name = "Confirm Order", ClaimValue = Order_Comfirm},
                    new PermissionItemDto {Name = "Delete Order", ClaimValue = Order_Delete},
                }
            },

            new PermissionGroupDto
            {
                GroupName = "Shipping Method Management",
                Permissions = new List<PermissionItemDto>
                {
                    new PermissionItemDto {Name = "Create Shipping Method", ClaimValue = Shipping_Create},
                    new PermissionItemDto {Name = "Edit Shipping Method", ClaimValue = Shipping_Edit},
                    new PermissionItemDto {Name = "Delete Shipping Method", ClaimValue = Shipping_Delete},
                }
            },
        };

    }
}
