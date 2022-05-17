using Energizet.VideoStriming.DB;
using Energizet.VideoStriming.Download;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Energizet.VideoStriming.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[DisableRequestSizeLimit]
	public class DownloadController : ControllerBase
	{
		private readonly IDownloadController _download;
		private readonly IVideoInfoController _videoInfo;

		public DownloadController(IDownloadController downloadController, IVideoInfoController videoInfoController)
		{
			_download = downloadController;
			_videoInfo = videoInfoController;
		}

		[HttpGet("{id}/{quality}/{part}")]
		public async Task Get(Guid id, int quality, int part)
		{
			var video = await _videoInfo.GetVideoAsync(id, quality);
			var filePart = await _download.GetFilePartAsync(id, video, part);

			Response.Headers["Content-Type"] = filePart.MIME;
			Response.Headers["Content-Length"] = filePart.Buffer.Length.ToString();
			await Response.Body.WriteAsync(filePart.Buffer, 0, filePart.Buffer.Length);
		}
	}
}
