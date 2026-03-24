using Zhaoxi.MSACommerce.HttpApi.Common;
using Zhaoxi.MSACommerce.SearchService.HttpApi;
using Zhaoxi.MSACommerce.SearchService.Infrastructure;
using Zhaoxi.MSACommerce.SearchService.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddUseCase();

builder.Services.AddHttpApi();

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