using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energizet.VideoStriming.Common.Models.Upload
{
	public class UploadResult : BaseResult
	{
		public static BaseResult OK()
		{
			return new UploadResult().SetMeta(Status.Ok);
		}

		public static BaseResult OK(object data = default)
		{
			return new UploadResult().SetMeta(Status.Ok).SetData(data);
		}

		public static BaseResult Error(string message, string stackTrace = null)
		{
			return new UploadResult().SetMeta(Status.Error, message: message, stackTrace: stackTrace);
		}
	}
}
