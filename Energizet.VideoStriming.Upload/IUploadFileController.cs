using Energizet.VideoStriming.Common.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energizet.VideoStriming.Upload
{
	public interface IUploadFileController
	{
		public Task<Func<Func<int, Format, long, Task>, Task>> UploadAsync(IFormFile file, Guid fileId);
	}
}
