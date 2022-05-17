using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energizet.VideoStriming.Common.Models
{
	public abstract class BaseVideoInfo
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Discription { get; set; }
		public long Views { get; set; }
		public int Status { get; set; }
		public string Format { get; set; }
	}
}
