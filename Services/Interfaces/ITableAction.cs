
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageAccessFunction.TableRepo;

namespace StorageAccessFunction.Services.Interfaces
{
    public interface IPmsRepository
    {
        public Task InsertEntity<T>(T entity, bool forInsert = true) where T : TableEntity;
        public Task<List<TableData>> GetDataAsync();
    }
}
