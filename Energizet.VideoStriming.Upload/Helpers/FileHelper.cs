using CSMediaProperties;
using CSVideoConverter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energizet.VideoStriming.Upload.Helpers
{
	public class FileHelper
	{
		public static string User { get; private set; } = "";
		public static string Key { get; private set; } = "";
		public static string Path { get; private set; } = "";

		public static string[] WhiteListTypes { get; } = new[]
		{
			"AVI",
			//"FLV",
			//"MKV",
			//"MTS",
			//"MOV",
			"MP4",
			//"MPEG",
			//"WEBM",
			//"WMV",
		};

		public delegate void OperationOutputEventHandler(object sender, string output);
		public delegate void OperationProgressEventHandler(object sender, int res, OutputProperties props);
		public delegate void OperationDoneEventHandler(object sender, bool aborted = false);
		public delegate void OperationStartEventHandler(object sender);
		public delegate void OperationClosedEventHandler(object sender);

		public static void Init(string user, string key, string path = "")
		{
			User = user;
			Key = key;
			Path = path;
		}

		public static async Task<Common.Models.Format> GerFormatAsync(string filePath)
		{
			using var converter = new VideoConverter(User, Key, Path);
			var mediaProperties = await converter.Open(filePath);

			var formatName = mediaProperties.Format.FormatName.ToLower();
			var videoStream = mediaProperties.Streams.Stream.FirstOrDefault(stream => stream.CodecType == "video");
			var isWidth = int.TryParse(videoStream?.Width, out var width);
			var isHeight = int.TryParse(videoStream?.Height, out var height);

			var avgFps = videoStream?.AvgFrameRate.Split('/') ?? new[] { "0", "1" };
			var isDTop = int.TryParse(avgFps[0], out var dTop);
			var isDBottom = int.TryParse(avgFps[1], out var dBottom);
			var fps = 0.0;
			if (isDTop && isDBottom)
			{
				fps = 1.0 * dTop / dBottom;
			}
			var isTime = long.TryParse(videoStream?.DurationTs, out var timeMs);

			var format = new Common.Models.Format
			{
				FormatName = WhiteListTypes.FirstOrDefault(type => formatName.Contains(type.ToLower())),
				Resolution = new Point(isWidth ? width : 0, isHeight ? height : 0),
				FPSf = fps,
				TimeMs = isTime ? timeMs : 0,
			};

			return format;
		}

		public static async Task<bool> SaveFileAsync(
			string filePath,
			string saveToPath,
			Point resolution,
			int fps,
			OperationStartEventHandler startHandler = null,
			OperationDoneEventHandler doneHandler = null,
			OperationProgressEventHandler progressHandler = null
		)
		{
			using var converter = new VideoConverter(User, Key, Path);
			converter.FileSource = filePath;
			converter.FileDestination = saveToPath;

			converter.VideoResolutionX = resolution.X.ToString();
			converter.VideoResolutionY = resolution.Y.ToString();
			converter.VideoFPS = fps.ToString();

			converter.VideoCodec = "h264";
			converter.AudioCodec = "aac";

			if (startHandler != null)
			{
				converter.OperationStart += new VideoConverter.OperationStartEventHandler(startHandler);
			}

			if (progressHandler != null)
			{
				converter.OperationProgress += new VideoConverter.OperationProgressEventHandler(progressHandler);
			}

			if (doneHandler != null)
			{
				converter.OperationDone += new VideoConverter.OperationDoneEventHandler(doneHandler);
			}

			var tcs = new TaskCompletionSource<bool>();
			converter.OperationDone += (s, aborted) => tcs.SetResult(!aborted);
			converter.OperationClosed += (s) => tcs.SetResult(false);

			converter.Run();

			return await tcs.Task;
		}
	}
}
