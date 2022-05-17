using Energizet.VideoStriming.Common.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Energizet.VideoStriming.Upload.UploadFileController;

namespace Energizet.VideoStriming.Upload
{
	public interface IUploadFileController
	{
		public Task<UploadCallbacks.SaveFiles> UploadAsync(IFormFile file, Guid fileId);
	}
}
