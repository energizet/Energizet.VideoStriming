using Energizet.VideoStriming.DB.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energizet.VideoStriming.DB
{
	public class UploadDBController : IUploadDBController
	{
		private readonly EntitiesContext _db;

		public UploadDBController(EntitiesContext db)
		{
			_db = db;
		}

		public async Task<bool> UploadAsync(Guid videoId, string name, string discription)
		{
			var video = new Video
			{
				Id = videoId,
				Name = name,
				Discription = discription,
				Views = 0,
				Status = 0,
			};

			await _db.Videos.AddAsync(video);
			await _db.SaveChangesAsync();

			return true;
		}

		public async Task SaveFileQuality(Guid videoId, int quality, Common.Models.Format format, long size)
		{
			var formatItem = _db.Formats.Where(item => item.Format1 == format.FormatName).FirstOrDefault();

			if (formatItem == null)
			{
				formatItem = new Format
				{
					Id = Guid.NewGuid(),
					Format1 = format.FormatName,
				};
				await _db.Formats.AddAsync(formatItem);
			}

			var qualityItem = _db.Qualitys.Where(item => item.Quality1 == quality).FirstOrDefault();

			if (qualityItem == null)
			{
				qualityItem = new Quality
				{
					Id = Guid.NewGuid(),
					Quality1 = quality,
				};
				await _db.Qualitys.AddAsync(qualityItem);
			}

			var videoQuality = new VideoQuality
			{
				Id = Guid.NewGuid(),
				VideoId = videoId,
				QualityId = qualityItem.Id,
				FormatId = formatItem.Id,
				Size = size
			};
			await _db.VideoQualitys.AddAsync(videoQuality);

			await _db.SaveChangesAsync();
		}
	}
}
