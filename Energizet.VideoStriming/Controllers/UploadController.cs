using Energizet.VideoStriming.Common.Models;
using Energizet.VideoStriming.Common.Models.Download;
using Energizet.VideoStriming.Common.Models.Upload;
using Energizet.VideoStriming.DB;
using Energizet.VideoStriming.DB.DB;
using Energizet.VideoStriming.Upload;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Energizet.VideoStriming.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[DisableRequestSizeLimit]
	public class UploadController : ControllerBase
	{
		private readonly IUploadFileController _uploadFile;
		private readonly IUploadDBController _uploadDb;

		public UploadController(IUploadFileController uploadFileController, IUploadDBController uploadDBController)
		{
			_uploadFile = uploadFileController;
			_uploadDb = uploadDBController;
		}

		[HttpPost]
		[Consumes("multipart/form-data")]
		public async Task<BaseResult> Post(IFormFile file, string name, string discription)
		{
			try
			{
				var videoId = Guid.NewGuid();
				var uploadCallback = await _uploadFile.UploadAsync(file, videoId);
				await _uploadDb.UploadAsync(videoId, name, discription);

				ThreadPool.QueueUserWorkItem(async (state) => await DoBackgroud(videoId, uploadCallback));
				//await DoBackgroud(videoId, uploadCallback);

				return UploadResult.OK();
			}
			catch (Exception ex)
			{
				return UploadResult.Error(ex.Message, ex.StackTrace);
			}
		}

		private async Task DoBackgroud(Guid videoId, UploadFileController.UploadCallbacks.SaveFiles callback)
		{
			var task = callback?.Invoke((quality, format, size) => _uploadDb.SaveFileQuality(videoId, quality, format, size));
			if (task == null)
			{
				return;
			}

			await task;
		}
	}
}
