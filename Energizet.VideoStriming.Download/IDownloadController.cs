using Energizet.VideoStriming.Common.Models;
using Energizet.VideoStriming.Common.Models.Download;
using Energizet.VideoStriming.Common.Models.FileInfo;

namespace Energizet.VideoStriming.Download
{
	public interface IDownloadController
	{
		public Task<FilePartData> GetFilePartAsync(Guid id, VideoInfoForDownload videoInfo, int partIndex);
	}
}