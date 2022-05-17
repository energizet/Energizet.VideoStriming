using Energizet.VideoStriming.Common.Models;
using Energizet.VideoStriming.Common.Models.FileInfo;
using Energizet.VideoStriming.DB;
using Energizet.VideoStriming.DB.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Energizet.VideoStriming.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[DisableRequestSizeLimit]
	public class VideoInfoController : ControllerBase
	{
		private readonly IVideoInfoController _fileInfo;

		public VideoInfoController(IVideoInfoController videoInfoController)
		{
			_fileInfo = videoInfoController;
		}

		[HttpGet]
		public async Task<BaseResult> Get()
		{
			try
			{
				var videosInfo = await _fileInfo.GetVideosAsync();

				return FileInfoResult.OK(videosInfo);
			}
			catch (Exception ex)
			{
				return FileInfoResult.Error(ex.Message, ex.StackTrace);
			}
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<BaseResult> Get(Guid id)
		{
			try
			{
				var video = await _fileInfo.GetVideoAsync(id);

				return FileInfoResult.OK(video);
			}
			catch (Exception ex)
			{
				return FileInfoResult.Error(ex.Message, ex.StackTrace);
			}
		}

		[HttpGet]
		[Route("{id}/{quality}")]
		public async Task<BaseResult> Get(Guid id, int quality)
		{
			try
			{
				var video = await _fileInfo.GetVideoAsync(id, quality);

				return FileInfoResult.OK(video);
			}
			catch (Exception ex)
			{
				return FileInfoResult.Error(ex.Message, ex.StackTrace);
			}
		}
	}
}
