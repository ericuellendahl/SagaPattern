var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ApiSaga>("apisaga");

builder.AddProject<Projects.ApiSagaPayment>("apisagapayment");

builder.Build().Run();
