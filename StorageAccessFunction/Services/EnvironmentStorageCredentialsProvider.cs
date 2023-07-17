using Microsoft.WindowsAzure.Storage.Auth;
using StorageAccessFunction.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageAccessFunction.Services
{
    public class EnvironmentStorageCredentialsProvider : IStorageCredentialsProvider
	{
		public StorageCredentials GetStorageCredentials()
		{
			return new StorageCredentials(
				Environment.GetEnvironmentVariable("accountName"),
				Environment.GetEnvironmentVariable("accountKey")
			);
		}
	}

}
