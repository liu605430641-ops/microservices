var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.UseDefaultFiles(new DefaultFilesOptions{DefaultFileNames = ["index.html"] });
app.UseStaticFiles();
app.Run();