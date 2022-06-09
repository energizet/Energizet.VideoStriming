using Energizet.VideoStriming.Common.Models;
using Energizet.VideoStriming.Common.Models.Download;
using Energizet.VideoStriming.Common.Models.FileInfo;

namespace Energizet.VideoStriming.Download
{
	public interface IDownloadController
	{
		public byte[] GetBytes(Guid id, int quality, int from, int to);
	}
}