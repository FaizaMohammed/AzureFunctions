using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StorageAccessFunction.Services.Interfaces;
using StorageAccessFunction.TableRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageAccessFunction.Services
{
	public class KeyvaultRepository : IKeyvaultRepository
	{
		private readonly IKeyvaultNameProvider _keyVaultNameProvider;
		private readonly string _keyVaultName;
		private readonly ILogger log;
		public KeyvaultRepository(IKeyvaultNameProvider keyVaultNameProv, ILogger log)
		{
			this.log = log;
			_keyVaultNameProvider = keyVaultNameProv;
			_keyVaultName = _keyVaultNameProvider.getKeyvaultName();
		}

		public async Task<IActionResult> PushtoKeyVault(dynamic values)
		{
			//string keyVaultName = Environment.GetEnvironmentVariable("KEY_VAULT_NAME");
			var kvUri = "https://" + _keyVaultName + ".vault.azure.net";

			var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
			var secretName = "test";
			//Create Secret on Key Vault
			KeyVaultSecret keyVaultSecret = new KeyVaultSecret(secretName, "1234");
			await client.SetSecretAsync(keyVaultSecret);

			//await client.SetSecretAsync(secretName, "ABCD");
			var secret = await client.GetSecretAsync(secretName);
			log.LogInformation($"The secret for the key:{secretName} is {secret}");
			return new OkResult();
		}
	}
}
