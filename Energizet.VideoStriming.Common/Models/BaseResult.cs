using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energizet.VideoStriming.Common.Models
{
	public abstract class BaseResult : IDisposable
	{
		public Meta Meta { get; set; }
		public object Data { get; set; }

		public BaseResult SetMeta(Status status, string message = null, string stackTrace = null)
		{
			Meta = new Meta
			{
				Status = status,
				Message = message,
				StackTrace = stackTrace,
			};
			return this;
		}

		public BaseResult SetData(object data = default)
		{
			Data = data;
			return this;
		}

		public void Dispose()
		{
			if (Data is IDisposable data)
			{
				data?.Dispose();
			}
		}

		~BaseResult()
		{
			Dispose();
		}
	}
}
