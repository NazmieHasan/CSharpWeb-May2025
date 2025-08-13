
namespace HotelApp.WebApi
{
    using Data;
    using Data.Models;
    using Data.Repository.Interfaces;
    using Services.Core.Interfaces;
    using Web.Infrastructure.Extensions;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddIdentityApiEndpoints<IdentityUser>()
                .AddEntityFrameworkStores<HotelAppDbContext>();

            // Add services to the container.
            builder.Services.AddRepositories(typeof(IBookingRepository).Assembly);
            builder.Services.AddUserDefinedServices(typeof(IBookingService).Assembly);

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

            app.UseAuthorization();

            app.MapIdentityApi<IdentityUser>();
            app.MapControllers();

            app.Run();
        }
    }
}
