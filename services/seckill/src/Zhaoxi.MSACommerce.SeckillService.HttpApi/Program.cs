using System.Text.Json.Serialization;
using Zhaoxi.MSACommerce.Authentication.JwtBearer;
using Zhaoxi.MSACommerce.SeckillService.HttpApi;
using Zhaoxi.MSACommerce.HttpApi.Common;
using Zhaoxi.MSACommerce.SeckillService.Infrastructure;
using Zhaoxi.MSACommerce.SeckillService.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddUseCase();

builder.Services.AddHttpApi();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString;
    });

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