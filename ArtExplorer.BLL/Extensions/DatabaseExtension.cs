using ArtExplorer.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArtExplorer.BLL.Extensions;

public static class DataBaseExtension
{
    #region Public methods 

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = GetConnectionString(configuration);
        if (connectionString == null)
        {
            throw new Exception("Not found DB configuration in config file");
        }

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        return services;
    }

    #endregion

    #region Private methods

    private static string GetConnectionString(IConfiguration configuration)
    {
        return configuration.GetSection("Database:ConnectionString").Value;
    }

    #endregion
}
