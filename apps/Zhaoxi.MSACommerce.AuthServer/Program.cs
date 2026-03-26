using Consul.AspNetCore;
using Zhaoxi.MSACommerce.Authentication.JwtBearer;
using Zhaoxi.MSACommerce.AuthServer;
using Zhaoxi.MSACommerce.Consul.ServiceDiscovery;
using Zhaoxi.MSACommerce.Consul.ServiceRegistration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureServices(builder.Configuration);

builder.Services.AddJwtBearer(builder.Configuration);

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAny");

app.UseHealthChecks(serviceCheck.Path);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
