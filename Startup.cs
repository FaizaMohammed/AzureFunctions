using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

using StorageAccessFunction.Repositories;
using StorageAccessFunction.Repositories.Interfaces;
using StorageAccessFunction.Services;
using StorageAccessFunction.Services.Interfaces;
using StorageAccessFunction.TableRepo;
using System;

[assembly: FunctionsStartup(typeof(MyNamespace.Startup))]

namespace MyNamespace
{
    public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			builder.Services.AddHttpClient();
			builder.Services.AddSingleton<IStorageCredentialsProvider, EnvironmentStorageCredentialsProvider>();
			builder.Services.AddSingleton<ICloudStorageAccountProvider, CloudStorageAccountProvider>();
			builder.Services.AddSingleton<ICloudTableClientProvider, CloudTableClientProvider>();
			builder.Services.AddSingleton<IPmsRepository, Pmsrepository>();
			builder.Services.AddSingleton<IEnvtTableNameProvider, EnvtTableNameProvider>();
			builder.Services.AddSingleton<ITableProvider, TableProvider>();
			builder.Services.AddSingleton<ITableReferenceProvider, TableReferenceProvider>();
			builder.Services.AddSingleton<IKeyvaultRepository, KeyvaultRepository>();
			builder.Services.AddSingleton<IKeyvaultNameProvider, KeyvaultNameProvider>();


			builder.Services.AddTransient<ILogger>((s) => {
				return new Logger();
			});

			
		}
	}
}