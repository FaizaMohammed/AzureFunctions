using Microsoft.WindowsAzure.Storage.Table;
using StorageAccessFunction.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageAccessFunction.Services
{
    public class TableReferenceProvider : ITableReferenceProvider
	{
		public ICloudTableClientProvider _client;
        public TableReferenceProvider(ICloudTableClientProvider cloudTableClientProvider)
        {
			_client = cloudTableClientProvider;
            
        }
        public CloudTableClient getTableReference()
		{
			return _client.GetCloudTableClient();
		}
	}
}
