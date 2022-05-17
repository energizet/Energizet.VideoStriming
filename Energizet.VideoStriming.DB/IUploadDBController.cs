using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energizet.VideoStriming.DB
{
	public interface IUploadDBController
	{
		public Task<bool> UploadAsync(Guid videoId, string name, string discription);
		public Task SaveFileQuality(Guid videoId, int quality, Common.Models.Format format, long size);
	}
}
