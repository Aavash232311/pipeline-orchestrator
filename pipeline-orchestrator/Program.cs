using pipeline_orchestrator.Data;
using Microsoft.EntityFrameworkCore;
using pipeline_orchestrator.Services;
using pipeline_orchestrator.Engines;

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

app.UseAuthorization();

app.MapControllers();

app.Run();
