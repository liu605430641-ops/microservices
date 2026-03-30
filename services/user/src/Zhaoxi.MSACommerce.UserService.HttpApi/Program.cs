using Zhaoxi.MSACommerce.Authentication.JwtBearer;
using Zhaoxi.MSACommerce.Configuration;
using Zhaoxi.MSACommerce.HttpApi.Common;
using Zhaoxi.MSACommerce.UserService.HttpApi;
using Zhaoxi.MSACommerce.UserService.Infrastructure;
using Zhaoxi.MSACommerce.UserService.UseCases;

var builder = WebApplication.CreateBuilder(args);

const string appName = "user-service";

builder.Configuration.AddConfigCenter(appName);

builder.Services.AddSerilogLoki(builder.Configuration, appName);

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