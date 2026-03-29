using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Enums;
using Consul.AspNetCore;
using Winton.Extensions.Configuration.Consul;
using Zhaoxi.MSACommerce.Authentication.JwtBearer;
using Zhaoxi.MSACommerce.HttpApi.Common;
using Zhaoxi.MSACommerce.UserService.HttpApi;
using Zhaoxi.MSACommerce.UserService.Infrastructure;
using Zhaoxi.MSACommerce.UserService.UseCases;

var builder = WebApplication.CreateBuilder(args);


//注册consul地址
builder.Configuration.AddConsul("zhaoxi-msa-commerce/appsettings.json");

//注册apollo地址 
builder.Configuration
       .AddApollo(builder.Configuration.GetSection(nameof(ApolloOptions)))
       .AddDefault(ConfigFileFormat.Json)
       .AddNamespace("user-service.json");

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddUseCase();

builder.Services.AddHttpApi();

builder.Services.AddControllers();

builder.Services.AddJwtBearer(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpCommon();

app.MapControllers();

app.Run();
