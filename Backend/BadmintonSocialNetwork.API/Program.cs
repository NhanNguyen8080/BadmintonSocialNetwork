using AutoMapper;
using BadmintonSocialNetwork.API.Extensions;
using BadmintonSocialNetwork.Service.Helpers;
using System.Linq.Expressions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerConfiguration()
                .AddDatabaseContext()
                .AddJwtAuth()
                .AddDependencyInjection()
                .ConfigureSection(builder.Configuration);

//Mapping services
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new Mapping());
});
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

app.SeedDatabase()
   .UseErrorHandlingMiddle()
   .UseJwtMiddleware()
   .UseSwaggerDocumentation();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
