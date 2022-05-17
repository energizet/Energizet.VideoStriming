using Energizet.VideoStriming.Common.Models;
using Energizet.VideoStriming.Common.Models.Download;
using Energizet.VideoStriming.Common.Models.Upload;
using Energizet.VideoStriming.Upload.Helpers;
using Microsoft.AspNetCore.Http;
using System.Drawing;

namespace Energizet.VideoStriming.Upload
{
	public sealed class UploadFileController : IUploadFileController
	{
		private readonly string _path;

		public UploadFileController(string path)
		{
			_path = path;
		}

		public async Task<UploadCallbacks.SaveFiles> UploadAsync(IFormFile file, Guid fileId)
		{
			var tmpDirInfo = Directory.CreateDirectory($"{_path}tmp/");
			var tmpFilePath = $"{tmpDirInfo.FullName}{fileId}";
			using (var stream = new FileStream(tmpFilePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			var format = await FileHelper.GerFormatAsync(tmpFilePath);

			if (!format.IsWhiteFormat)
			{
				File.Delete(tmpFilePath);
				throw new ArgumentException("File must be video");
			}

			var dirInfo = Directory.CreateDirectory($"{_path}{fileId}/");
			var fileName = $"source.{format.FormatName}";
			var filePath = $"{dirInfo.FullName}{fileName}";
			File.Move(tmpFilePath, filePath);

			return (saveQuality) => Convert(dirInfo.FullName, fileName, format, saveQuality);
		}

		private async Task Convert(string dirPath, string fileName, Format format, UploadCallbacks.SaveQuality saveQuality)
		{
			if (format.Resolution.Y >= 360)
			{
				await Convert(dirPath, fileName, format, new Point(640, 360), 30, saveQuality);
			}

			if (format.Resolution.Y >= 480)
			{
				await Convert(dirPath, fileName, format, new Point(850, 480), 30, saveQuality);
			}

			if (format.Resolution.Y >= 720)
			{
				await Convert(dirPath, fileName, format, new Point(1280, 720), 60, saveQuality);
			}

			if (format.Resolution.Y >= 1080)
			{
				await Convert(dirPath, fileName, format, new Point(1920, 1080), 60, saveQuality);
			}

			if (format.Resolution.Y >= 2160)
			{
				await Convert(dirPath, fileName, format, new Point(3840, 2160), 60, saveQuality);
			}

		}
		private async Task Convert(string dirPath, string fileName, Format format, Point resolution, int fps, UploadCallbacks.SaveQuality saveQuality)
		{
			var filePath = $"{dirPath}{fileName}";
			var formatName = format.FormatName.ToLower();


			var saveToPath = GetFilePath(dirPath, resolution.Y, formatName);
			await FileHelper.SaveFileAsync(filePath, saveToPath, resolution, format.FPS > fps ? fps : format.FPS);
			await saveQuality(resolution.Y, format, GetSize(saveToPath));
		}

		private string GetFilePath(string dir, int quality, string format)
		{
			return $"{dir}{quality}.{format}";
		}

		private long GetSize(string filePath)
		{
			var fileInfo = new FileInfo(filePath);
			return fileInfo.Length;
		}

		public class UploadCallbacks
		{
			public delegate Task SaveFiles(SaveQuality saveQuality);
			public delegate Task SaveQuality(int quality, Format format, long size);
		}
	}
}