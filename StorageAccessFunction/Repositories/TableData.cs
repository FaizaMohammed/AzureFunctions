using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageAccessFunction.TableRepo
{
	public class TableData : TableEntity
	{
		public string client_id { get; set; }
		public string scope { get; set; }
	}
}
