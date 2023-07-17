using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageAccessFunction.Services.Interfaces;

namespace StorageAccessFunction.Services
{
    public class CloudTableClientProvider : ICloudTableClientProvider
	{
		private readonly ICloudStorageAccountProvider _accountProvider;

		public CloudTableClientProvider(ICloudStorageAccountProvider accountProvider)
		{
			_accountProvider = accountProvider;
		}

		public CloudTableClient GetCloudTableClient()
		{
			CloudStorageAccount account = _accountProvider.GetCloudStorageAccount();
			return account.CreateCloudTableClient();
		}
	}

}
