using Aspire.Hosting.Azure;
var builder = DistributedApplication.CreateBuilder(args);

builder.AddAzureContainerAppEnvironment("app-env");
var sqlScriptStorage = builder.AddAzureStorage("sql-script-storage").RunAsEmulator();
var appSql = builder.AddAzureSqlServer("sql").RunAsContainer();
  
#pragma warning disable ASPIREAZURE003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
appSql.WithAdminDeploymentScriptStorage(sqlScriptStorage);
#pragma warning restore ASPIREAZURE003 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

var appDb = appSql.AddDatabase("db");

var apiService = builder.AddProject<Projects.aspire_sql_deploy_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(appDb);

builder.Build().Run();
