using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energizet.VideoStriming.Common.Models.Download
{
	public class DownloadResult : BaseResult
	{
		public static BaseResult OK(object data = default)
		{
			return new DownloadResult().SetMeta(Status.Ok).SetData(data);
		}

		public static BaseResult Error(string message, string stackTrace = null)
		{
			return new DownloadResult().SetMeta(Status.Error, message: message, stackTrace: stackTrace);
		}
	}
}
