using Energizet.VideoStriming.Common.Models.FileInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energizet.VideoStriming.DB
{
	public interface IVideoInfoController
	{
		public Task<IEnumerable<VideoInfo>> GetVideosAsync();
		public Task<VideoInfo> GetVideoAsync(Guid id);
		public Task<VideoInfoForDownload> GetVideoAsync(Guid id, int quality);
	}
}
