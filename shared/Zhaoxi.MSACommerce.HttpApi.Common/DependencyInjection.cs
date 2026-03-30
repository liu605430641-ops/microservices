using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.Grafana.Loki;
using Zhaoxi.MSACommerce.HttpApi.Common.Infrastructure;
using Zhaoxi.MSACommerce.HttpApi.Common.Services;
using Zhaoxi.MSACommerce.UseCases.Common.Interfaces;

namespace Zhaoxi.MSACommerce.HttpApi.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddHttpApiCommon(this IServiceCollection services)
    {
        services.AddHealthChecks();

        services.AddScoped<IUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddExceptionHandler<UseCaseExceptionHandler>();

        services.AddProblemDetails();

        ConfigureCors(services);

        ConfigureSwagger(services);

        return services;
    }

    public static void AddSerilogLoki(this IServiceCollection services, IConfiguration configuration, string appName)
    {
        // 注册 Serilog 服务
        services.AddSerilog((sp, lc) =>
        {
            lc.Enrich.FromLogContext();
#if DEBUG
            lc.WriteTo.Console();
#elif RELEASE
            lc.WriteTo.Console(LogEventLevel.Error)
#endif
            var lokiUri = configuration["LokiUri"];
            if (lokiUri is not null)
            {
                lc.WriteTo.GrafanaLoki(
                    uri: lokiUri,
                    labels: new List<LokiLabel>
                    {
                        new() { Key = "App", Value = appName },
                        new() { Key = "Host", Value = configuration["Urls"] ?? string.Empty }
                    });
            }
            
            var esUri = configuration["EsUri"];
            if (esUri is not null)
            {
                
                lc.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(esUri))
                {
                    IndexFormat = "Serilog-index-{0:yyyy.MM.dd}",
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8
                });
                lc.Enrich.WithProperty("App", appName);
                
                lc.Enrich.WithProperty("Host", configuration["Urls"] ?? string.Empty);
            }
        });
    }

    private static void ConfigureCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAny", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }


    private static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "电商平台 API 文档",
                Version = "v1",
                Description = "一个微服务架构的电商平台实战项目"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "JWT Bearer 认证",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new string[] { }
                }
            });
        });
    }

}
