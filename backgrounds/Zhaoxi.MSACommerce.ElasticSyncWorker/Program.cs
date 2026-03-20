using Consul.AspNetCore;
using MassTransit;
using Zhaoxi.MSACommerce.ElasticSyncWorker;
using Zhaoxi.MSACommerce.ElasticSyncWorker.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddConsul();

var host = builder.Build();
host.Run();