using ArtExplorer.API.Extensions;
using ArtExplorer.BLL.Extensions;
using ArtExplorer.API.Extensions;
using ArtExplorer.DAL.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthorization()
    .AddControllers().Services
    .AddDatabase(builder.Configuration)
    .AddCoreServices()
    .AddIdentityServices(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

ApplyMigrations(app);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

static void ApplyMigrations(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        scope.ServiceProvider.GetService<AppDbContext>()
            .Database.Migrate();
    }
}
