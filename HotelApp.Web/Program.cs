namespace HotelApp.Web
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    using Data;
    using Data.Models;
    using Data.Repository.Interfaces;
    using Data.Seeding;
    using Data.Seeding.Interfaces;
    using Infrastructure.Extensions;
    using Services.Core.Interfaces;

    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services
                .AddDbContext<HotelAppDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    ConfigureIdentity(builder.Configuration, options);
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<HotelAppDbContext>();

            // 403 AccessDenied; 404 can appear if routing hides the resource
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Home/Error?statusCode=403"; 
            });

            builder.Services.AddRepositories(typeof(IBookingRepository).Assembly);
            builder.Services.AddUserDefinedServices(typeof(IBookingService).Assembly);

            // TODO: Implement as extension method
            builder.Services.AddTransient<IIdentitySeeder, IdentitySeeder>();

            builder.Services.AddControllersWithViews();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.SeedDefaultIdentity();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseManagerAccessRestriction();

            // This middleware redirects admins to the admin dashboard
            // app.UserAdminRedirection();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        private static void ConfigureIdentity(IConfigurationManager configurationManager, IdentityOptions identityOptions)
        {
            identityOptions.SignIn.RequireConfirmedEmail =
                configurationManager.GetValue<bool>($"IdentityConfig:SignIn:RequireConfirmedEmail");
            identityOptions.SignIn.RequireConfirmedAccount =
                configurationManager.GetValue<bool>($"IdentityConfig:SignIn:RequireConfirmedAccount");
            identityOptions.SignIn.RequireConfirmedPhoneNumber =
                configurationManager.GetValue<bool>($"IdentityConfig:SignIn:RequireConfirmedPhoneNumber");

            identityOptions.Password.RequiredLength =
                configurationManager.GetValue<int>($"IdentityConfig:Password:RequiredLength");
            identityOptions.Password.RequireNonAlphanumeric =
                configurationManager.GetValue<bool>($"IdentityConfig:Password:RequireNonAlphanumeric");
            identityOptions.Password.RequireDigit =
                configurationManager.GetValue<bool>($"IdentityConfig:Password:RequireDigit");
            identityOptions.Password.RequireLowercase =
                configurationManager.GetValue<bool>($"IdentityConfig:Password:RequireLowercase");
            identityOptions.Password.RequireUppercase =
                configurationManager.GetValue<bool>($"IdentityConfig:Password:RequireUppercase");
            identityOptions.Password.RequiredUniqueChars =
                configurationManager.GetValue<int>($"IdentityConfig:Password:RequiredUniqueChars");
        }
    }
}
