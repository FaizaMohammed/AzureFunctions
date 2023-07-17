using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace StorageAccessFunction.AzureFunctions
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            log.LogInformation("C# HTTP trigger function processed a request.");
            //{"client_id": "abc", "client_secret": "def", "scope": "xyz"}
            string clientId = data["client_id"];
            string client_secret = data["client_secret"];
            string scope = data["scope"];
            log.LogInformation($"clientId: {clientId} client_secret:{client_secret} scope:{scope} ");
            return new OkResult();
        }
    }
}
