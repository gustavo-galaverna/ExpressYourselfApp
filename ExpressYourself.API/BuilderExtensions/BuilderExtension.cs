using Microsoft.OpenApi.Models;
using System.Reflection;
using ExpressYourself.Application.Mappings;
using ExpressYourself.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using ExpressYourself.Domain.Interfaces;
using ExpressYourself.Infrastructure.Persistence.Repository;
using ExpressYourself.Application.Interfaces;
using ExpressYourself.Application.Services;
namespace ExpressYourself.API.BuilderExtensions;

public static class BuilderExtension
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICountryRepository, CountryRepository>();
        builder.Services.AddScoped<IIpAddressRepository, IpAddressRepository>();
        builder.Services.AddScoped<ICountryService, CountryService>();
        builder.Services.AddScoped<IIpAddressService, IpAddressService>();
     
    }   
    public static void AddContext(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var connectionString = configuration.GetConnectionString("Default");
        builder.Services.AddDbContext<ExpressYourselfContext>(options => 
            options.UseSqlServer(connectionString));

    }

    public static void AddMappings(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(ProfileMappings));
    }

    public static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options => 
        {
            options.SwaggerDoc("v1", new OpenApiInfo 
            { 
                Title = "API V1", Version = "v1" 
            }); 
            var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
        });
        
    }

}