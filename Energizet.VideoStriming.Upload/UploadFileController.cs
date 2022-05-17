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

		public async Task<Func<Func<int, Format, long, Task>, Task>> UploadAsync(IFormFile file, Guid fileId)
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

			return async (saveQuality) => await Convert(dirInfo.FullName, fileName, format, saveQuality);
		}

		private async Task Convert(string dirPath, string fileName, Format format, Func<int, Format, long, Task> saveQuality)
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
		private async Task Convert(string dirPath, string fileName, Format format, Point resolution, int fps, Func<int, Format, long, Task> saveQuality)
		{



			var saveToPath = GetFilePath(dirPath, resolution.Y, formatName);
			await FileHelper.SaveFileAsync(filePath, saveToPath, resolution, format.FPS > fps ? fps : format.FPS);
			await saveQuality(resolution.Y, format, GetSize(saveToPath));
		}

		private async Task<int> ConvertSegment(
			string dirPath,
			string sourceName,
			Format format,
			Point resolution,
			int fps,
			TimeSpan from,
			TimeSpan lenght
		)
		{
			var sourcePath = $"{dirPath}{sourceName}";
			var formatName = format.FormatName.ToLower();
			var quality = resolution.Y;

			await FileHelper.SaveFileAsync(sourcePath, , resolution, fps, , )
		}

		private IEnumerable<Segment> GetSegments(long timeMs, int sizeSegmentInSec = 60)
		{
			var size = timeMs * 10_000;
			var segmentSize = sizeSegmentInSec * 10_000_000;
			var segmentCount = size / segmentSize;

			var segments = Enumerable.Range(0, (int)segmentCount).Select(item => new Segment
			{
				From = new TimeSpan(item * 10_000_000),
				Lenght = new TimeSpan(segmentSize),
			}).ToList();

			var lastSegmentSize = size - segmentCount * segmentSize;

			if (lastSegmentSize > 0)
			{
				segments.Add(new Segment
				{
					From = new TimeSpan(segmentCount * segmentSize),
					Lenght = new TimeSpan(segmentSize),
				});
			}
			return segments;
		}

		private string GetFilePath(string dir, int quality, int segment, string format)
		{
			var path = $"{dir}{quality}/{segment}.{format}";
			Directory.CreateDirectory(path);
			return path;
		}

		private long GetSize(string sourcePath)
		{
			var fileInfo = new FileInfo(sourcePath);
			return fileInfo.Length;
		}

		private class Segment
		{
			public TimeSpan From { get; set; }
			public TimeSpan Lenght { get; set; }
		}
	}
}