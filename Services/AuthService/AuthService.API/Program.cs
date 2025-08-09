using AuthService.Application;
using AuthService.Persistence;
using AuthService.Infrastructure;
using Shared.Auth;
using Shared.Auth.Interfaces;
using Shared.Auth.Models;
using Shared.Common;
using Shared.Common.Interfaces;
using Shared.Infrastructure.Redis.Models;

var builder = WebApplication.CreateBuilder(args);

#region IOC

builder.Services.AddHttpContextAccessor();

#region Shared
builder.Services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddSingleton<IHashingHelper, HashingHelper>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection("Redis"));
builder.Services.AddSingleton<IJwtTokenValidator, JwtTokenValidator>();
#endregion

#region Auth.Persistence 
var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddPersistenceServices(connectionString);
#endregion

#region Auth.Application 
builder.Services.AddApplicationServices();
#endregion

#region Auth.Infrastructure 
builder.Services.AddInfrastructureServices();
#endregion


#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
