
namespace HotelApp.WebApi
{
    using Data;
    using Data.Models;
    using Data.Repository.Interfaces;
    using Services.Core.Interfaces;
    using Web.Infrastructure.Extensions;

    using Microsoft.EntityFrameworkCore;

    using static GCommon.ApplicationConstants;

    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<HotelAppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddAuthorization();
            builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
                .AddEntityFrameworkStores<HotelAppDbContext>();

            // Add services to the container.
            builder.Services.AddRepositories(typeof(IBookingRepository).Assembly);
            builder.Services.AddUserDefinedServices(typeof(IBookingService).Assembly);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(AllowAllDomainsPolicy, policyBuilder =>
                {
                    policyBuilder
                        .WithOrigins("https://localhost:7180")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapIdentityApi<ApplicationUser>();
            app.MapControllers();

            app.Run();
        }
    }
}
