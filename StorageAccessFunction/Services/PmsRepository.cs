using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageAccessFunction.TableRepo;
using StorageAccessFunction.Services.Interfaces;

namespace StorageAccessFunction.Services
{
    public class Pmsrepository : IPmsRepository
    {
		private readonly CloudTable table;
        private readonly ITableProvider _tableProvider;
        private readonly CloudTableClient _client;

		private readonly string tableName;

		bool forInsert = false;
        public Pmsrepository(ITableProvider tableProvider,IEnvtTableNameProvider envtTableNameProvider)
        {
            _tableProvider = tableProvider;
			tableName=envtTableNameProvider.getTableName();
			_client = _tableProvider.getTable();
           table= _client.GetTableReference(tableName);

        }
        public async Task<List<TableData>> GetDataAsync()
        {
            try
            {
                TableQuery<TableData> query = new TableQuery<TableData>();
                var ser = new List<TableData>();
                var res = await table.ExecuteQuerySegmentedAsync(new TableQuery<TableData>(), null);
                if (res != null)
                {
					ser.AddRange(res.Results);
				}
				return ser;

            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }

        //public List<TableData> GetData()
        //{
        //	var result = GetDataAsync();
        //	return result.Result;

        //}
        //public void InsertData(TableData tableData)
        //{
        //	InsertEntity<TableData>(tableData);
        //}


        public async Task InsertEntity<T>(T entity, bool forInsert = true) where T : TableEntity
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task InsertEntity()
        {
            throw new NotImplementedException();
        }
    }

}
