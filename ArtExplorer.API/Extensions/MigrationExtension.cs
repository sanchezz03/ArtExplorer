using ArtExplorer.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace ArtExplorer.API.Extensions;

public static class MigrationExtension
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            //scope.ServiceProvider.GetService<AppDbContext>()
            //    .Database.Migrate();
        }
    }
}
