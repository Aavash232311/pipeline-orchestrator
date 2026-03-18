using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

#pragma warning disable ASPIREHOSTINGPYTHON001
var fastApiService = builder.AddPythonApp(
        "fastapi-service",
        "../pipeline-orchestrator.PythonService",
        "../pipeline-orchestrator.PythonService/app/main.py"
    )
    .WithHttpEndpoint(port: 8000, env: "PORT");
#pragma warning restore ASPIREHOSTINGPYTHON001

builder.AddProject<Projects.pipeline_orchestrator>("webapi")
       .WithEnvironment("PIPELINE_URL", "http://localhost:8000");

builder.Build().Run();