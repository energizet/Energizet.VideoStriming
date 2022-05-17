using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energizet.VideoStriming.Common.Models.FileInfo
{
	public class VideoInfoForDownload: BaseVideoInfo
	{
		public int Quality { get; set; }
		public long Size { get; set; }

	}
}