using Energizet.VideoStriming.DB.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energizet.VideoStriming.DB
{
	public interface IUploadDBController
	{
		public Task<Video> UploadAsync(Guid videoId, string name, string discription);
		public Task SaveFileQuality(Guid videoId, int quality, Common.Models.Format format, long size);
		public Task<bool> SetDoneStatus(Guid videoId);
	}
}
