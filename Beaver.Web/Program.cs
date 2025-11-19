using Beaver;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAutofac();  //Add this line
builder.Services.ReplaceConfiguration(builder.Configuration);

builder.Services.AddApplication<BeaverWebModule>();

var app = builder.Build();

app.InitializeApplication();

app.Run();
