using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageAccessFunction.Services.Interfaces
{
	public interface IKeyvaultRepository
	{
		public  Task<IActionResult> PushtoKeyVault(dynamic data);
	}
}
