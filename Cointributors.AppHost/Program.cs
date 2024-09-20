var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Cointributors_Web>("cointributors-web");

builder.Build().Run();
