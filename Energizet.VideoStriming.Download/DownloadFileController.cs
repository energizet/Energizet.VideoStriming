using Energizet.VideoStriming.Common.Models;
using Energizet.VideoStriming.Common.Models.Download;
using Energizet.VideoStriming.Common.Models.FileInfo;

namespace Energizet.VideoStriming.Download
{
	public sealed class DownloadFileController : IDownloadController
	{
		private readonly string _path;
		private readonly int _blockSize;

		public DownloadFileController(string path, int blockSize = 1 * 1024 * 1024)
		{
			_path = path;
			_blockSize = blockSize;
		}

		public async Task<FilePartData> GetFilePartAsync(Guid id, VideoInfoForDownload videoInfo, int partIndex)
		{
			return await Task.Run(async () =>
			{
				var info = new FileInfo($"{_path}{id}/{videoInfo.Quality}.{videoInfo.Format}");
				//var info = new FileInfo($"{_path}{id}/source.{videoInfo.Format}");

				if (info == null)
				{
					return new FilePartData
					{
						MIME = "",
						Buffer = new byte[0],
					};
				}

				using var reader = new FileStream(info.FullName, FileMode.Open);

				var blockSize = GetBlockSize(info.Length, partIndex, _blockSize);
				var buffer = new byte[blockSize];
				reader.Position = partIndex * _blockSize;
				await reader.ReadAsync(buffer, 0, buffer.Length);/**/

				/*var buffer = new byte[info.Length];
				await reader.ReadAsync(buffer, 0, buffer.Length);/**/

				return new FilePartData
				{
					MIME = "video/mp4",
					Buffer = buffer,
				};
			});
		}

		private int GetBlockSize(long size, int blockIndex, int blockSize)
		{
			var realBlockSize = size - blockIndex * blockSize;
			if (realBlockSize < 0)
			{
				return 0;
			}

			if (realBlockSize > blockSize)
			{
				return blockSize;
			}

			return (int)realBlockSize;
		}
	}
}