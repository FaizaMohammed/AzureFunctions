using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StorageAccessFunction.TableRepo;
using StorageAccessFunction.Services.Interfaces;

namespace StorageAccessFunction.AzureFunctions
{
    public  class PushToRepo
    {
        private readonly IPmsRepository _tableAction;
        public PushToRepo(IPmsRepository tableAction)
        {
            _tableAction = tableAction;
        }
        [FunctionName("PushToRepo")]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
           
            
            try
            {
				string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
				dynamic data = JsonConvert.DeserializeObject(requestBody);
				var values = data;
                if (values != null)
                {
                    TableData sr = new TableData();
                    sr.RowKey = values["RowKey"];
                    sr.scope = values["scope"];
                    sr.PartitionKey = values["PartitionKey"];
                    sr.client_id = values["client_id"];
                    //TableData td = setValues(values);
                    await _tableAction.InsertEntity(sr);
                    var dataval = await _tableAction.GetDataAsync();
                    return new OkObjectResult(dataval);
                }
            }
            catch  (JsonReaderException ex)
            {
                return new BadRequestObjectResult("Invalid JSON payload.");
            }
            catch (Exception ex)
            {
				return new BadRequestObjectResult(ex);

			}
            return new BadRequestResult();
		}
        public static TableData setValues(dynamic values)
        {
            TableData sr = new TableData();
            sr.RowKey = values["RowKey"];
            sr.scope = values["scope"];
            sr.PartitionKey = values["PartitionKey"];
            sr.client_id = values["client_id"];
            return sr;
        }
    }
}
