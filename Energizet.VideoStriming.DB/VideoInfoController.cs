using Energizet.VideoStriming.Common.Models.FileInfo;
using Energizet.VideoStriming.DB.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energizet.VideoStriming.DB
{
	public class VideoInfoController : IVideoInfoController
	{
		private readonly EntitiesContext _db;

		public VideoInfoController(EntitiesContext db)
		{
			_db = db;
		}

		public async Task<IEnumerable<VideoInfo>> GetVideosAsync()
		{
			var videos = await _db.VideoQualitys
				.Include(item => item.Video)
				.Include(item => item.Quality)
				.Include(item => item.Format)
				.ToListAsync();

			var videosInfo = videos.GroupBy(item => item.VideoId, GetVideoInfo);

			return videosInfo;
		}

		public async Task<VideoInfo> GetVideoAsync(Guid id)
		{
			var videos = await _db.VideoQualitys
				.Include(item => item.Video)
				.Include(item => item.Quality)
				.Include(item => item.Format)
				.Where(item => item.VideoId == id)
				.ToListAsync();

			var videosInfo = videos.GroupBy(item => item.VideoId, GetVideoInfo).FirstOrDefault();

			if (videosInfo == null)
			{
				throw new NullReferenceException("Video not found");
			}

			return videosInfo;
		}

		private VideoInfo GetVideoInfo(Guid key, IEnumerable<VideoQuality> group)
		{
			var video = group.First();
			var videoInfo = new VideoInfo
			{
				Id = video.VideoId,
				Name = video.Video.Name,
				Discription = video.Video.Discription,
				Status = video.Video.Status,
				Views = video.Video.Views,
				Format = video.Format.Format1,

				Qualitys = group.Select(item => new QualityInfo
				{
					Quality = item.Quality.Quality1,
					Size = item.Size,
				})
			};
			return videoInfo;
		}
	}
}
