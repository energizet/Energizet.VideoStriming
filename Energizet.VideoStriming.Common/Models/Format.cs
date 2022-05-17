using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energizet.VideoStriming.Common.Models
{
	public class Format
	{
		public double FPSf { get; set; }
		public string FormatName { get; set; }
		public Point Resolution { get; set; }
		public long TimeMs { get; set; }
		public bool IsWhiteFormat => FormatName != null;

		public int FPS
		{
			get
			{
				if (FPSf >= 120)
				{
					return 120;
				}

				if (FPSf >= 60)
				{
					return 60;
				}

				if (FPSf >= 30)
				{
					return 30;
				}

				if (FPSf < 1)
				{
					return 1;
				}

				return Convert.ToInt32(FPSf);
			}
		}
	}
}
