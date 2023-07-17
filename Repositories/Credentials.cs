using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageAccessFunction.Repositories.Interfaces;

namespace StorageAccessFunction.Repositories
{
    public class Credentials : ICredentials
    {

        public CloudTable table { get; set; }
        public void SetCredentials()
        {
            StorageCredentials creds = new StorageCredentials(Environment.GetEnvironmentVariable("accountName"), Environment.GetEnvironmentVariable("accountKey"));

            CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);

            CloudTableClient client = account.CreateCloudTableClient();

            table = client.GetTableReference("Storage");
        }
    }
}
