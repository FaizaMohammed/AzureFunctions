using StorageAccessFunction.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageAccessFunction.Services
{
	public class KeyvaultNameProvider : IKeyvaultNameProvider
	{
		public string getKeyvaultName()
		{
		return	Environment.GetEnvironmentVariable("KEY_VAULT_NAME");
		}
	}
}
