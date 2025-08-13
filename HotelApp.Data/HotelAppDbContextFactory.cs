namespace HotelApp.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.IO;
    using System.Linq;

    public class HotelAppDbContextFactory : IDesignTimeDbContextFactory<HotelAppDbContext>
    {
        public HotelAppDbContext CreateDbContext(string[] args)
        {
            var solutionDir = FindSolutionRoot();

            // Ако подадеш името на проекта като аргумент, използвай него
            string projectName = args?.FirstOrDefault();

            // Намери всички проекти с appsettings.json
            var webProjects = Directory.GetDirectories(solutionDir, "*", SearchOption.AllDirectories)
                                       .Where(d => File.Exists(Path.Combine(d, "appsettings.json")))
                                       .ToList();

            string webProjectPath;

            if (!string.IsNullOrEmpty(projectName))
            {
                webProjectPath = webProjects.FirstOrDefault(d => d.EndsWith(projectName))
                                 ?? throw new DirectoryNotFoundException($"Not found project {projectName} с appsettings.json");
            }
            else
            {
                // Вземи първия .Web проект
                webProjectPath = webProjects.FirstOrDefault()
                                 ?? throw new DirectoryNotFoundException("Not found Web project with appsettings.json");
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(webProjectPath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<HotelAppDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new HotelAppDbContext(optionsBuilder.Options);
        }

        private string FindSolutionRoot()
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (dir != null && !dir.GetFiles("*.sln").Any())
            {
                dir = dir.Parent;
            }

            if (dir == null)
                throw new Exception("Not found solution file (.sln).");

            return dir.FullName;
        }
    }
}
