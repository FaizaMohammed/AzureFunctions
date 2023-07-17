using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageAccessFunction.Services.Interfaces;

namespace StorageAccessFunction.Services
{
    public class EnvtTableNameProvider : IEnvtTableNameProvider
	{
		public string getTableName()
		{
			return Environment.GetEnvironmentVariable("TableName");
		}
	}
}
