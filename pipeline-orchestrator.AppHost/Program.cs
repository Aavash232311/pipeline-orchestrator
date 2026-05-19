using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var githubPat = builder.AddParameter("github-pat", secret: true);

if (githubPat is null)
{
    throw new InvalidOperationException("Github API key is missing.");
}

#pragma warning disable ASPIREHOSTINGPYTHON001
var fastApiService = builder.AddPythonApp(
        "fastapi-service",
        "../pipeline-orchestrator.PythonService",
        "../pipeline-orchestrator.PythonService/app/main.py"
    )
    .WithEnvironment("PORT", "8000"); 
#pragma warning restore ASPIREHOSTINGPYTHON001

var webApi = builder.AddProject<Projects.pipeline_orchestrator>("webapi")
    .WithEnvironment("GITHUB_PAT", githubPat)
    .WithReference(fastApiService)     
    .WaitFor(fastApiService);

// TypeScript client on same proxy
var client = builder.AddNpmApp("pipeline-orchestrator", "../pipeline-orchestrator.Client", "dev")
    .WithReference(webApi)
    .WaitFor(webApi)
    .WithHttpEndpoint(env: "PORT", port: 5173)
    .WithExternalHttpEndpoints();

builder.Build().Run();

builder.Build().Run();