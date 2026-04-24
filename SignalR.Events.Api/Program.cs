using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi;
using SignalR.Events.Api.Hubs;
using SignalR.Events.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMvcCore().AddApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "SignalR Events API",
        Description = "API for demonstrating SignalR events with background workers"
    });
});

// Add SignalR
builder.Services.AddSignalR();

// Add CORS for SignalR
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register background workers
builder.Services.AddHostedService<CurrentUserEventWorker>();
builder.Services.AddHostedService(provider => 
    new RandomUserEventWorker(
        provider.GetRequiredService<IHubContext<EventHub>>(),
        provider.GetRequiredService<ILogger<RandomUserEventWorker>>(),
        "RandomWorker-1"));
builder.Services.AddHostedService(provider => 
    new RandomUserEventWorker(
        provider.GetRequiredService<IHubContext<EventHub>>(),
        provider.GetRequiredService<ILogger<RandomUserEventWorker>>(),
        "RandomWorker-2"));
builder.Services.AddHostedService(provider => 
    new RandomUserEventWorker(
        provider.GetRequiredService<IHubContext<EventHub>>(),
        provider.GetRequiredService<ILogger<RandomUserEventWorker>>(),
        "RandomWorker-3"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapSwagger();
app.UseSwagger();

app.UseCors();

// HTTPS redirection disabled - using HTTP only
// if (!app.Environment.IsDevelopment())
// {
//     app.UseHttpsRedirection();
// }

app.UseAuthorization();

app.MapControllers();

// Map SignalR hub endpoint
app.MapHub<EventHub>("/eventhub");

app.Run();
