using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energizet.VideoStriming.Common.Models.FileInfo
{

	public class VideoInfo : BaseVideoInfo
	{
		public IEnumerable<QualityInfo> Qualitys { get; set; }
	}
}
