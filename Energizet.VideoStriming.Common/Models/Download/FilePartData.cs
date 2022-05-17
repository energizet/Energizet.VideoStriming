using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energizet.VideoStriming.Common.Models.Download
{
	public class FilePartData
	{
		public string MIME { get; set; }
		public byte[] Buffer { get; set; }
	}
}
