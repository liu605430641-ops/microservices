using Consul.AspNetCore;
using Zhaoxi.MSACommerce.Consul.ServiceDiscovery;
using Zhaoxi.MSACommerce.Consul.ServiceRegistration;
using Zhaoxi.MSACommerce.LoadBalancer;
using Zhaoxi.MSACommerce.ProductDetailPage;
using Zhaoxi.MSACommerce.ProductDetailPage.Apis;
using Zhaoxi.MSACommerce.ProductDetailPage.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IDetailPageService, DetailPageService>();
builder.Services.AddScoped<IStaticPageService, StaticPageService>();

builder.Services.ConfigureServices(builder.Configuration);

var serviceCheck = builder.Configuration.GetSection("ServiceCheck").Get<ServiceCheckConfiguration>();
serviceCheck ??= new ServiceCheckConfiguration();

builder.Services.AddConsul();
builder.Services.AddConsulService(serviceConfiguration =>
{
    serviceConfiguration.ServiceAddress = new Uri(builder.Configuration["urls"] ?? builder.Configuration["applicationUrl"]);
}, serviceCheck);

builder.Services.AddConsulDiscovery();

builder.Services.AddHealthChecks();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHealthChecks(serviceCheck.Path);

app.UseStaticPageMiddleware(builder.Configuration["StaticPagePath"] ?? "wwwroot");

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();