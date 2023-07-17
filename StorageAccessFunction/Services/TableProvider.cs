using Microsoft.WindowsAzure.Storage.Table;
using StorageAccessFunction.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageAccessFunction.Services
{
    public class TableProvider : ITableProvider
	{
		private readonly ITableReferenceProvider _tableReferenceProvider;
        public TableProvider(ITableReferenceProvider tableReferenceProvider)
        {
			_tableReferenceProvider = tableReferenceProvider; 
        }
        public CloudTableClient getTable()
		{
		var result=	_tableReferenceProvider.getTableReference();
			return result;
		}
	}
}
