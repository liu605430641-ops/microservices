using Microsoft.EntityFrameworkCore;
using Zhaoxi.MSACommerce.Authentication.JwtBearer;
using Zhaoxi.MSACommerce.CategoryService.HttpApi;
using Zhaoxi.MSACommerce.CategoryService.Infrastructure;
using Zhaoxi.MSACommerce.CategoryService.Infrastructure.Data;
using Zhaoxi.MSACommerce.CategoryService.UseCases;
using Zhaoxi.MSACommerce.HttpApi.Common;
using Zhaoxi.MSACommerce.Infrastructure.Common.Interceptors;

var builder = WebApplication.CreateBuilder(args);

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