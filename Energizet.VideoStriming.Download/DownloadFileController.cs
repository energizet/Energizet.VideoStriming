using Energizet.VideoStriming.Common.Models;
using Energizet.VideoStriming.Common.Models.Download;
using Energizet.VideoStriming.Common.Models.FileInfo;

namespace Energizet.VideoStriming.Download
{
	public sealed class DownloadFileController : IDownloadController
	{
		private readonly string _path;

		public DownloadFileController(string path)
		{
			_path = path;
		}

		public byte[] GetBytes(Guid id, int quality, int from, int to)
		{
			throw new NotImplementedException();
		}
	}
}