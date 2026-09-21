using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<ChipsPocket_Api>("api");

builder.Build().Run();