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

		[HttpGet("{id}/{quality}")]
		public async Task Get(Guid id, int quality)
		{
			//var video = await _videoInfo.GetVideoAsync(id, quality);
			//var filePart = await _download.GetFilePartAsync(id, video, part);
			//
			//Response.Headers["Content-Type"] = filePart.MIME;
			//Response.Headers["Content-Length"] = filePart.Buffer.Length.ToString();
			//await Response.Body.WriteAsync(filePart.Buffer, 0, filePart.Buffer.Length);

			var bufferSize = 1000000L;
			var buffer = new byte[bufferSize];

			var fileInfo = new FileInfo($"files/{id}/{quality}.MP4");
			using var fileStream = fileInfo.OpenRead();
			var totalSize = fileStream.Length;

			Request.Headers.TryGetValue("range", out var rangeHeader);
			var isFrom = TryParseRange(rangeHeader.ToString(), 0, out var fromByte);
			var isTo = TryParseRange(rangeHeader.ToString(), 1, out var toByte);

			if (!isFrom)
			{
				fromByte = 0;
			}

			if (!isTo)
			{
				toByte = totalSize - 1;
			}

			fileStream.Position = fromByte;
			var sendByteSize = toByte - fromByte + 1;

			Response.StatusCode = 206;
			Response.Headers["accept-ranges"] = "bytes";
			Response.Headers["Content-Length"] = $"{sendByteSize}";
			Response.Headers["Content-Range"] = $"bytes {fromByte}-{toByte}/{totalSize}";
			Response.Headers["Content-Type"] = "video/mp4";

			while (sendByteSize > 0)
			{
				var count = sendByteSize > bufferSize ? bufferSize : sendByteSize;
				var sizeOfReadedBuffer = await fileStream.ReadAsync(buffer, 0, (int)count);
				sendByteSize -= sizeOfReadedBuffer;

				await Response.Body.WriteAsync(buffer, 0, sizeOfReadedBuffer);

			}
		}

		private bool TryParseRange(string rangeHeader, int posBytes, out long bytes)
		{
			if (rangeHeader == null)
			{
				bytes = 0;
				return false;
			}

			var range = rangeHeader.Split('=');
			if (range.Length < 2)
			{
				bytes = 0;
				return false;
			}

			var rangeArr = range[1].Split('-');
			if (rangeArr.Length < 2)
			{
				bytes = 0;
				return false;
			}

			var isByte = long.TryParse(rangeArr[posBytes], out bytes);

			return isByte;
		}
	}
}
