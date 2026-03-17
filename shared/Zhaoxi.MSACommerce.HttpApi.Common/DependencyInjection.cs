using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Zhaoxi.MSACommerce.HttpApi.Common.Infrastructure;
using Zhaoxi.MSACommerce.HttpApi.Common.Services;
using Zhaoxi.MSACommerce.UseCases.Common.Interfaces;

namespace Zhaoxi.MSACommerce.HttpApi.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddHttpApiCommon(this IServiceCollection services)
    {
        services.AddScoped<IUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddExceptionHandler<UseCaseExceptionHandler>();

        services.AddProblemDetails();

        ConfigureCors(services);

        ConfigureSwagger(services);
        return services;
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

    /// <summary>
    /// 通用的Swagger配置，适用于所有使用了这个Common库的服务。可以在这里添加一些全局的Swagger配置，比如全局的安全定义、全局的响应描述等。
    /// </summary>
    /// <param name="services"></param>
    private static void ConfigureSwagger(IServiceCollection services)
    {
        // 这里可以添加一些全局的Swagger配置，比如全局的安全定义、全局的响应描述等。
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
                               {
                                   options.SwaggerDoc("v1", new OpenApiInfo
                                                            {
                                                                Title       = "电商平台 API 文档",
                                                                Version     = "v1",
                                                                Description = "一个微服务架构的电商平台实战项目"
                                                            });

                                   options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                                                                           {
                                                                               Name         = "JWT Bearer 认证",
                                                                               In           = ParameterLocation.Header,
                                                                               Type         = SecuritySchemeType.Http,
                                                                               Scheme       = "Bearer",
                                                                               BearerFormat = "JWT"
                                                                           });
                                   options.AddSecurityRequirement(new OpenApiSecurityRequirement
                                                                  {
                                                                      {
                                                                          new OpenApiSecurityScheme
                                                                          {
                                                                              Reference = new OpenApiReference
                                                                                          {
                                                                                              Id   = "Bearer",
                                                                                              Type = ReferenceType.SecurityScheme
                                                                                          }
                                                                          },
                                                                          new string[] { }
                                                                      }
                                                                  });
                               });

    }
}
