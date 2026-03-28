using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Zhaoxi.MSACommerce.Authentication.JwtBearer;
using Zhaoxi.MSACommerce.WebGateway;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddOcelot(
    folder: "./ocelot",
    env: builder.Environment,
    mergeTo: MergeOcelotJson.ToMemory,
    optional: false, reloadOnChange: true);

builder.Services
    .AddOcelot()
    .AddConsul<IPConsulServiceBuilder>();

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
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "网关 V1");
        options.SwaggerEndpoint("/auth/swagger.json", "授权中心 V1");
        options.SwaggerEndpoint("/verification/swagger.json", "验证码服务器 V1");
        options.SwaggerEndpoint("/user/swagger.json", "用户服务 V1");
        options.SwaggerEndpoint("/category/swagger.json", "品类服务 V1");
        options.SwaggerEndpoint("/brand/swagger.json", "品牌服务 V1");
        options.SwaggerEndpoint("/search/swagger.json", "搜索服务 V1");
        options.SwaggerEndpoint("/cart/swagger.json", "购物车服务 V1");
        options.SwaggerEndpoint("/order/swagger.json", "订单服务 V1");
        options.SwaggerEndpoint("/payment/swagger.json", "支付服务 V1");
        options.SwaggerEndpoint("/seckill/swagger.json", "秒杀服务 V1");
    });
}

app.UseAuthentication();

app.UseOcelot().Wait();

app.Run();
