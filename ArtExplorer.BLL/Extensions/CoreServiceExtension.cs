using ArtExplorer.BLL.Services;
using ArtExplorer.BLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ArtExplorer.BLL.Extensions;

public static class CustomeServiceExtension
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        return services
            .AddScoped<ITokenService, TokenService>()
            .AddScoped<IMetMuseumService, MetMuseumService>()
            .AddScoped<IFavoriteService, FavoriteService>();
    }
}

