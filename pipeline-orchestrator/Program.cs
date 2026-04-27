using pipeline_orchestrator.Data;
using Microsoft.EntityFrameworkCore;
using pipeline_orchestrator.Engines;
using System.Threading.RateLimiting;
using pipeline_orchestrator.Services;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
string connectionStringName = "DefaultConnection";
var connectionString = builder.Configuration.GetConnectionString(connectionStringName);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// python service pipeline as microservice 
builder.Services.AddHttpClient("PythonPipeline", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["PIPELINE_URL"] ?? "http://localhost:8000"
    );
});


// rate limitor, without it we can pace something like a DDOS attack.
builder.Services.AddRateLimiter(options =>
{
    // what happens if we have too many requests.
    options.OnRejected = async (context, token) =>
    {
        // we send that in HTTP header.
        context.HttpContext.Response.ContentType = "text/plain";
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync("Toooo many request. Please try again.", cancellationToken: token);
    };

    options.AddFixedWindowLimiter(policyName: "RateLimitorController", opt =>
    {
        opt.PermitLimit = 10;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2; // two request to wait in line before rejecting.
    });
});



builder.AddServiceDefaults();

/* Dependency Injections 💉 */
builder.Services.AddSingleton<Microservice>();
builder.Services.AddHttpClient<REST>();
builder.Services.AddSingleton<Screening>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseRateLimiter();

app.UseAuthorization(); 

app.MapControllers();

app.Run();