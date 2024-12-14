using Microsoft.OpenApi.Models;
using ExpressYourself.Application.Mappings;
using ExpressYourself.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using ExpressYourself.Domain.Interfaces;
using ExpressYourself.Infrastructure.Persistence.Repository;
using ExpressYourself.Application.Interfaces;
using ExpressYourself.Application.Services;
using ExpressYourself.Infrastructure.Services;
using ExpressYourself.Application;
using ExpressYourself.API.Jobs;
using ExpressYourself.Infrastructure;
using System.Reflection;

namespace ExpressYourself.API.BuilderExtensions;

public static class BuilderExtension
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICountryRepository, CountryRepository>();
        builder.Services.AddScoped<IIpAddressRepository, IpAddressRepository>();
        builder.Services.AddScoped<DatabaseConnection>();
        builder.Services.AddScoped<IIpCountryReportRepository, IpCountryReportRepository>();
        builder.Services.AddScoped<ICountryService, CountryService>();
        builder.Services.AddScoped<IIpAddressService, IpAddressService>();
        builder.Services.AddScoped<ICacheService, CacheService>();
        builder.Services.AddScoped<UpdateIpInformationService>();
     
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

    public static void AddHttpClient(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        builder.Services.AddHttpClient<IIP2CService, IP2CService>(client =>
        {
            client.BaseAddress = new Uri(configuration["IP2C:Uri"]!);
            client.Timeout = TimeSpan.FromSeconds(10);
        });

    }

    public static void AddBackgroundServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHostedService<IpUpdateJob>();
    }

    public static void AddRedis(this WebApplicationBuilder builder)
    {
        var redisHost = builder.Configuration["Redis:Host"];
        var redisPort = builder.Configuration["Redis:Port"];
        var redisConnectionString = $"{redisHost}:{redisPort}";

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.InstanceName = "instance";
            options.Configuration = redisConnectionString;
        });




    }

}