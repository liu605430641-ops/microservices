using Consul;
using Zhaoxi.MSACommerce.Authentication.JwtBearer;
using Zhaoxi.MSACommerce.HttpApi.Common;
using Zhaoxi.MSACommerce.LoadBalancer;
using Zhaoxi.MSACommerce.OrderService.HttpApi;
using Zhaoxi.MSACommerce.OrderService.Infrastructure;
using Zhaoxi.MSACommerce.OrderService.UseCases;
using Zhaoxi.MSACommerce.OrderService.UseCases.Apis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddUseCase();

builder.Services.AddHttpApi();

builder.Services.AddControllers();

builder.Services.AddJwtBearer(builder.Configuration);

builder.Services.AddControllers();

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