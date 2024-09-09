using CozaStore.Data;

namespace CozaStore.Extensions
{
    public static class AuthServiceExtensions
    {
        public static IServiceCollection AddPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {

                 //color
                options.AddPolicy(ClaimStore.Color_Create, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Color_Create));
                options.AddPolicy(ClaimStore.Color_Edit, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Color_Edit));
                options.AddPolicy(ClaimStore.Color_Delete, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Color_Delete));
                

                // category
                options.AddPolicy(ClaimStore.Category_Create, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Category_Create));
                options.AddPolicy(ClaimStore.Category_Edit, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Category_Edit));
                options.AddPolicy(ClaimStore.Category_Delete, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Category_Delete));


                //size
                options.AddPolicy(ClaimStore.Size_Create, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Size_Create));
                options.AddPolicy(ClaimStore.Size_Edit, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Size_Edit));
                options.AddPolicy(ClaimStore.Size_Delete, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Size_Delete));


                // product
                options.AddPolicy(ClaimStore.Product_Create, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Product_Create));
                options.AddPolicy(ClaimStore.Product_Edit, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Product_Edit));
                options.AddPolicy(ClaimStore.Product_Delete, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Product_Delete));


                // other
                options.AddPolicy(ClaimStore.Order_ComfirmPayment, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Order_ComfirmPayment));
                options.AddPolicy(ClaimStore.Order_Comfirm, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Order_Comfirm));
                options.AddPolicy(ClaimStore.Order_Delete, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Order_Delete));


                // shipping
                options.AddPolicy(ClaimStore.Shipping_Create, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Shipping_Create));
                options.AddPolicy(ClaimStore.Shipping_Delete, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Shipping_Delete));
                options.AddPolicy(ClaimStore.Shipping_Edit, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Shipping_Edit));


                // user management
                options.AddPolicy(ClaimStore.User_UpdateUserRole, policy =>
                    policy.RequireClaim("Permission", ClaimStore.User_UpdateUserRole));
                options.AddPolicy(ClaimStore.User_Lockout, policy =>
                    policy.RequireClaim("Permission", ClaimStore.User_Lockout));


                // role management 
                options.AddPolicy(ClaimStore.Role_Create, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Role_Create));
                options.AddPolicy(ClaimStore.Role_Edit, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Role_Edit));
                options.AddPolicy(ClaimStore.Role_Delete, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Role_UpdateClaimRoles));
                options.AddPolicy(ClaimStore.Role_Delete, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Role_UpdateClaimRoles));
               

                // access page
                options.AddPolicy(ClaimStore.AccessOrder, policy =>
                {
                    policy.RequireClaim("Permission", ClaimStore.AccessOrder);
                });
                options.AddPolicy(ClaimStore.AccessUser, policy =>
                {
                    policy.RequireClaim("Permission", ClaimStore.AccessUser);
                });
                options.AddPolicy(ClaimStore.AccessColor, policy =>
                {
                    policy.RequireClaim("Permission", ClaimStore.AccessColor);
                });
                options.AddPolicy(ClaimStore.AccessSize, policy =>
                {
                    policy.RequireClaim("Permission", ClaimStore.AccessSize);
                });
                options.AddPolicy(ClaimStore.AccessProduct, policy =>
                {
                    policy.RequireClaim("Permission", ClaimStore.AccessProduct);
                });
                 options.AddPolicy(ClaimStore.AccessRole, policy =>
                {
                    policy.RequireClaim("Permission", ClaimStore.AccessRole);
                });
                options.AddPolicy(ClaimStore.AccessDashboard, policy =>
                {
                    policy.RequireClaim("Permission", ClaimStore.AccessDashboard);
                });
                options.AddPolicy(ClaimStore.AccessBlog, policy =>
                {
                    policy.RequireClaim("Permission", ClaimStore.AccessBlog);
                });
                options.AddPolicy(ClaimStore.AccessCategory, policy =>
                {
                    policy.RequireClaim("Permission", ClaimStore.AccessCategory);
                });
                options.AddPolicy(ClaimStore.AccessShipping, policy =>
                {
                    policy.RequireClaim("Permission", ClaimStore.AccessShipping);
                });
                options.AddPolicy(ClaimStore.AccessOrderUser, policy =>
                {
                    policy.RequireClaim("Permission", ClaimStore.AccessOrderUser);
                });
            });

            return services;

        }
    }
}
