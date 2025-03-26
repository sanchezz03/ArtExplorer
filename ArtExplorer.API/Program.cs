using ArtExplorer.API.Extensions;
using ArtExplorer.BLL.Extensions;

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
