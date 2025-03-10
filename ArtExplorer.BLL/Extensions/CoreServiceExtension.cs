using Microsoft.Extensions.DependencyInjection;

namespace ArtExplorer.BLL.Extensions;

public static class CustomeServiceExtension
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        return services;
    }
}

