using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageAccessFunction.Services.Interfaces
{
	public interface IKeyvaultNameProvider
	{
		public string getKeyvaultName();
	}
}
