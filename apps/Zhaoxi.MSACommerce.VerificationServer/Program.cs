using Consul.AspNetCore;
using Zhaoxi.MSACommerce.Consul.ServiceDiscovery;
using Zhaoxi.MSACommerce.Consul.ServiceRegistration;
using Zhaoxi.MSACommerce.VerificationServer;
using Zhaoxi.MSACommerce.VerificationServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureServices(builder.Configuration);

builder.Services.AddScoped<ISmsService, SmsService>();

var serviceCheck = builder.Configuration.GetSection("ServiceCheck").Get<ServiceCheckConfiguration>();
serviceCheck ??= new ServiceCheckConfiguration();

builder.Services.AddConsul();
builder.Services.AddConsulService(serviceConfiguration =>
{
    serviceConfiguration.ServiceAddress = new Uri(builder.Configuration["urls"] ?? builder.Configuration["applicationUrl"]);
}, serviceCheck);

builder.Services.AddConsulDiscovery();

builder.Services.AddHealthChecks();

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

app.UseCors("AllowAny");

app.UseHealthChecks(serviceCheck.Path);

app.UseAuthorization();

app.MapControllers();

app.Run();