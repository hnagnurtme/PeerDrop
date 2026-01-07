using Microsoft.EntityFrameworkCore;
using PeerDrop.API;
using PeerDrop.API.Middlewares;
using PeerDrop.BLL;
using PeerDrop.DAL;
using PeerDrop.DAL.DbContexts;
using PeerDrop.Shared;
using PeerDrop.Shared.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add custom services from all layers
builder.Services.AddSharedLayer(builder.Configuration);
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration); 

var app = builder.Build();

// Auto apply migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Middleware pipeline

if (app.Environment.IsDevelopment())
{
    app.UseCors(ProjectConstants.CorsConstants.AllowDevPolicy);
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "PeerDrop API v1");
    });
}
else
{
    app.UseCors(ProjectConstants.CorsConstants.AllowProductionPolicy);
}

// Exception handling
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();