using Zhaoxi.MSACommerce.SeckillService.Infrastructure;
using Zhaoxi.MSACommerce.SecKillSyncWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.ConfigureQuartz(builder.Configuration);

var host = builder.Build();

host.Run();
