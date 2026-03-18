using Consul.AspNetCore;
using Zhaoxi.MSACommerce.AuthServer;
using Zhaoxi.MSACommerce.Consul.ServiceDiscovery;
using Zhaoxi.MSACommerce.Consul.ServiceRegistration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpApi(builder.Configuration);

//添加consul
var serviceCheck = builder.Configuration.GetSection("ServiceCheck").Get<ServiceCheckConfiguration>();
serviceCheck ??= new ServiceCheckConfiguration();
builder.Services.AddConsul();builder.Services.AddConsulService(serviceConfiguration =>
                                                               {
                                                                   serviceConfiguration.ServiceAddress = new Uri(builder.Configuration["urls"] ?? builder.Configuration["applicationUrl"]);
                                                               }, serviceCheck);
//添加consul服务发现
builder.Services.AddConsulDiscovery();
//添加健康检查
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

//中间件 AllowAny策略名称
app.UseCors("AllowAny"); //对应 configureCors方法里面的.AddPolicy("AllowAny", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
// 添加consul服务注册中间件
app.UseHealthChecks(serviceCheck.Path);


app.UseAuthorization();

app.MapControllers();

app.Run();
