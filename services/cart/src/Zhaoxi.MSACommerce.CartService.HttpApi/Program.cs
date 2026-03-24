using Zhaoxi.MSACommerce.Authentication.JwtBearer;
using Zhaoxi.MSACommerce.CartService.HttpApi;
using Zhaoxi.MSACommerce.CartService.Infrastructure;
using Zhaoxi.MSACommerce.CartService.UseCases;
using Zhaoxi.MSACommerce.HttpApi.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddUseCase();

builder.Services.AddHttpApi();

builder.Services.AddControllers();

builder.Services.AddJwtBearer(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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