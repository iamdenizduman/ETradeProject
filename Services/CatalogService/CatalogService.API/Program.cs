using CatalogService.Application;
using CatalogService.Persistence;
using CatalogService.Persistence.Models.Settings;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region IOC

#region Auth.Persistence 
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));
var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddPersistenceServices();
#endregion

#region Auth.Application 
builder.Services.AddApplicationServices();
#endregion

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
