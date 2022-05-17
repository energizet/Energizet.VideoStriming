using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energizet.VideoStriming.Common.Models.FileInfo
{
	public class FileInfoResult : BaseResult
	{
		public static BaseResult OK(object data = default)
		{
			return new FileInfoResult().SetMeta(Status.Ok).SetData(data);
		}

		public static BaseResult Error(string message, string stackTrace = null)
		{
			return new FileInfoResult().SetMeta(Status.Error, message: message, stackTrace: stackTrace);
		}
	}
}
