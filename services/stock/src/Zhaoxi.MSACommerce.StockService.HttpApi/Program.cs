using Zhaoxi.MSACommerce.Authentication.JwtBearer;
using Zhaoxi.MSACommerce.Configuration;
using Zhaoxi.MSACommerce.HttpApi.Common;
using Zhaoxi.MSACommerce.StockService.HttpApi;
using Zhaoxi.MSACommerce.StockService.Infrastructure;
using Zhaoxi.MSACommerce.StockService.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfigCenter("stock-service") ;
// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddUseCase();

builder.Services.AddHttpApi();

builder.Services.AddControllers();

builder.Services.AddJwtBearer(builder.Configuration);

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();