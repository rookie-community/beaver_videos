using Beaver;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAutofac();
builder.Services.ReplaceConfiguration(builder.Configuration);

await builder.AddApplicationAsync<AppModule>();

var app = builder.Build();

await app.InitializeApplicationAsync();

await app.RunAsync();
