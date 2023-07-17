using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace StorageAccessFunction.AzureFunctions
{
    public class StorageAccess
    {
        private readonly CloudTable table;
        bool forInsert = false;

        public StorageAccess()
        {

            StorageCredentials creds = new StorageCredentials(Environment.GetEnvironmentVariable("accountName"), Environment.GetEnvironmentVariable("accountKey"));

            CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);

            CloudTableClient client = account.CreateCloudTableClient();

            table = client.GetTableReference("TestTable");
        }


        [FunctionName("StorageAccess")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                int option = data.option;
                var values = data.tableData;

                switch (option)
                {
                    case 1:
                        {
                            //forInsert = true;
                            var id = values["Id"];
                            var PartitionKey = values["PartitionKey"];
                            var RowKey = values["RowKey"];
                            Services sr = new Services();
                            sr.Id = id;
                            sr.Name = values["Name"];
                            sr.Description = values["Description"];
                            sr.PartitionKey = PartitionKey;
                            sr.RowKey = RowKey;
                            InsertData(sr, forInsert);
                            return new OkResult();
                        }
                    case 2:
                        {
                            var res = await GetDataAsync();
                            return new OkObjectResult(res);
                        }
                    case 4:
                        var ret = await RetrieveEntity<Services>("RowKey eq '8faf602f-c622-4a33-a64e-aed94278c1a7'");
                        return new OkObjectResult(ret);
                    case 3:
                        var service = await RetrieveEntity<Services>("RowKey eq '8faf602f-c622-4a33-a64e-aed94278c1a7'");
                        var serviceToDelete = service.FirstOrDefault();
                        var ret1 = DeleteEntity(serviceToDelete);
                        return new OkObjectResult(ret1);
                    default: break;

                }




            }
            catch (Exception ex)
            {
                return new NotFoundResult();
            }

            return new OkResult();
        }
        public async Task<List<Services>> GetDataAsync()
        {
            try
            {
                TableQuery<Services> query = new TableQuery<Services>();
                var ser = new List<Services>();
                var res = await table.ExecuteQuerySegmentedAsync(new TableQuery<Services>(), null);
                ser.AddRange(res.Results);
                return ser;

            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }

        public void InsertEntity<T>(T entity, bool forInsert = true) where T : TableEntity, new()
        {
            try
            {
                if (forInsert)
                {
                    var insertOperation = TableOperation.Insert(entity);
                    table.ExecuteAsync(insertOperation);
                }
                else
                {
                    var insertOrMergeOperation = TableOperation.InsertOrReplace(entity);
                    table.ExecuteAsync(insertOrMergeOperation);
                }
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }

        public async Task<List<T>> RetrieveEntity<T>(string Query = null) where T : TableEntity, new()
        {
            try
            {
                // Create the Table Query Object for Azure Table Storage  
                TableQuery<T> DataTableQuery = new TableQuery<T>();
                if (!string.IsNullOrEmpty(Query))
                {
                    DataTableQuery = new TableQuery<T>().Where(Query);
                }
                IEnumerable<T> IDataList = await table.ExecuteQuerySegmentedAsync(DataTableQuery, null);
                List<T> DataList = new List<T>();
                foreach (var singleData in IDataList)
                    DataList.Add(singleData);
                return DataList;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }


        public async void InsertData(Services entity, bool forInsert)
        {
            try
            {
                if (forInsert)
                {
                    var insertOperation = TableOperation.Insert(entity);
                    await table.ExecuteAsync(insertOperation);
                }
                else
                {
                    var insertOrMergeOperation = TableOperation.InsertOrReplace(entity);
                    await table.ExecuteAsync(insertOrMergeOperation);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteEntity<T>(T entity) where T : TableEntity, new()
        {
            try
            {
                var DeleteOperation = TableOperation.Delete(entity);
                table.ExecuteAsync(DeleteOperation);
                return true;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }




    }
    public class Services : TableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
