using CozaStore.Data;

namespace CozaStore.Extensions
{
    public static class AuthServiceExtensions
    {
        public static IServiceCollection AddPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // product
                options.AddPolicy(ClaimStore.Product_Create, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Product_Create));

                options.AddPolicy(ClaimStore.Product_Edit, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Product_Edit));

                options.AddPolicy(ClaimStore.Product_Delete, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Product_Delete));

                options.AddPolicy(ClaimStore.Product_View, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Product_View));

                // category

                options.AddPolicy(ClaimStore.Category_Create, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Category_Create));

                options.AddPolicy(ClaimStore.Category_Edit, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Category_Edit));

                options.AddPolicy(ClaimStore.Category_Delete, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Category_Delete));

                options.AddPolicy(ClaimStore.Category_View, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Category_View));

                
                //size
                options.AddPolicy(ClaimStore.Size_Create, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Size_Create));
                options.AddPolicy(ClaimStore.Size_Edit, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Size_Edit));
                options.AddPolicy(ClaimStore.Size_Delete, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Size_Delete));
                options.AddPolicy(ClaimStore.Size_View, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Size_View));


                //color
                options.AddPolicy(ClaimStore.Color_Create, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Color_Create));
                options.AddPolicy(ClaimStore.Color_Edit, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Color_Edit));
                options.AddPolicy(ClaimStore.Color_Delete, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Color_Delete));
                options.AddPolicy(ClaimStore.Color_View, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Color_View));

                //role
                options.AddPolicy(ClaimStore.Role_Create, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Role_Create));
                options.AddPolicy(ClaimStore.Role_Edit, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Role_Edit));
                options.AddPolicy(ClaimStore.Role_Delete, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Role_Delete));
                options.AddPolicy(ClaimStore.Role_View, policy =>
                    policy.RequireClaim("Permission", ClaimStore.Role_View));

                //user
                options.AddPolicy(ClaimStore.User_Create, policy =>
                    policy.RequireClaim("Permission", ClaimStore.User_Create));
                options.AddPolicy(ClaimStore.User_View, policy =>
                    policy.RequireClaim("Permission", ClaimStore.User_View));
                options.AddPolicy(ClaimStore.User_Edit, policy =>
                    policy.RequireClaim("Permission", ClaimStore.User_Edit));
                options.AddPolicy(ClaimStore.User_Delete, policy =>
                    policy.RequireClaim("Permission", ClaimStore.User_Delete));


            });

            return services;

        }
    }
}
