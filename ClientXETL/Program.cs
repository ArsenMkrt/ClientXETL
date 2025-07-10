// Build DI container
using ClientXETL.Extensions;
using ClientXETL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

var configuration = new ConfigurationManager();
services.RegisterConfigurationServices(configuration);
services.RegisterLoggingServices(configuration);
services.RegisterClientXServices(configuration);

using var provider = services.BuildServiceProvider();

var loader = provider.GetRequiredService<ClientXETLService>();
await loader.RunAsync(CancellationToken.None);