using System.Text.Json.Serialization;
using EkoMon.DomainModel.Db;
using EkoMon.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.Configure<AppSettings>(builder.Configuration);
var appSettings = builder.Configuration.Get<AppSettings>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});

builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<EntityContext>(options =>
{
    options.UseNpgsql(appSettings.DbConnection, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument();

var app = builder.Build();
await InitDatabase(app);

app.UseCors(myAllowSpecificOrigins);

app.UseOpenApi();
app.UseSwaggerUi3();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


async Task InitDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    // Migrate
    var databaseContext = scope.ServiceProvider.GetRequiredService<EntityContext>();
    await databaseContext.Database.MigrateAsync();
}
