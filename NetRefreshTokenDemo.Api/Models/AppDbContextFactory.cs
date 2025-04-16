using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace NetRefreshTokenDemo.Api.Models
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // تحميل ملف الإعدادات
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<AppDbContext>();

            // الحصول على connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // استخدام SQL Server
            builder.UseSqlServer(connectionString);

            return new AppDbContext(builder.Options);
        }
    }
}
