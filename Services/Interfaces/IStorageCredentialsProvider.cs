using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageAccessFunction.Services.Interfaces
{
    public interface IStorageCredentialsProvider
    {
        public StorageCredentials GetStorageCredentials();
    }
}
