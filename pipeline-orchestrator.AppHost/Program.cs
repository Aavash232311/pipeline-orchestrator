var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.pipeline_orchestrator>("pipeline-orchestrator");

builder.Build().Run();
