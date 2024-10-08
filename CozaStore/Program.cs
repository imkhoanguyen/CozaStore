using CozaStore.Data;
using CozaStore.Data.Seed;
using CozaStore.Extensions;
using CozaStore.Hubs;
using CozaStore.Middleware;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);
builder.Services.AddPolicy();





var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<OrderHub>("/hubs/order");
app.MapHub<ReviewHub>("/hubs/review");



using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    context.Database.Migrate();
    ColorSeed.Seed(context);
    CategorySeed.Seed(context);
    SizeSeed.Seed(context);
    ProductSeed.Seed(context);
    ShippingMethodSeed.Seed(context);
    await UserSeed.SeedAsync(services, context);
    await RoleSeed.SeedAsync(services);
    await RoleClaimSeed.SeedAsync(context, services);
    await UserRoleSeed.SeedAsync(services, context);

} catch(Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}


app.Run();
