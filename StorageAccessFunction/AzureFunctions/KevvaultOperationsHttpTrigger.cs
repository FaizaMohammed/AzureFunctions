using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using StorageAccessFunction.Services.Interfaces;

namespace StorageAccessFunction.AzureFunctions
{
    public  class KevvaultOperationsHttpTrigger
    {
        private IKeyvaultRepository keyVaultRepository;

        public KevvaultOperationsHttpTrigger(IKeyvaultRepository keyvaultRepository)
        {
            this.keyVaultRepository = keyvaultRepository;
        }

        [FunctionName("KevvaultOperationsHttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            try {

				var ret = await keyVaultRepository.PushtoKeyVault(req);
                return ret;
			}
            catch (Exception ex)
            {
				Console.WriteLine(ex.ToString());
				return new BadRequestResult();
            }

            
        }
    }
}
