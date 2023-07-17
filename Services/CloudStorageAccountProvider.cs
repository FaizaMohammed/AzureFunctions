using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageAccessFunction.Services.Interfaces;

namespace StorageAccessFunction.Services
{
    public class CloudStorageAccountProvider : ICloudStorageAccountProvider
	{
		private readonly IStorageCredentialsProvider _credentialsProvider;

		public CloudStorageAccountProvider(IStorageCredentialsProvider credentialsProvider)
		{
			_credentialsProvider = credentialsProvider;
		}

		public CloudStorageAccount GetCloudStorageAccount()
		{
			StorageCredentials credentials = _credentialsProvider.GetStorageCredentials();
			return new CloudStorageAccount(credentials, useHttps: true);
		}
	}

}
