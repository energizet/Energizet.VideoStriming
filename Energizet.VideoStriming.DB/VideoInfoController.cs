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
		private readonly int _blockSize;

		public VideoInfoController(EntitiesContext db, int blockSize = 1 * 1024 * 1024)
		{
			_db = db;
			_blockSize = blockSize;
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

		public async Task<VideoInfoForDownload> GetVideoAsync(Guid id, int quality)
		{
			var video = await _db.VideoQualitys
				.Include(item => item.Video)
				.Include(item => item.Quality)
				.Include(item => item.Format)
				.Where(item => item.VideoId == id)
				.Where(item => item.Quality.Quality1 == quality)
				.FirstOrDefaultAsync();

			if (video == null)
			{
				throw new NullReferenceException("Video not found");
			}

			return new VideoInfoForDownload
			{
				Id = video.VideoId,
				Name = video.Video.Name,
				Discription = video.Video.Discription,
				Status = video.Video.Status,
				Views = video.Video.Views,
				Format = video.Format.Format1,

				Size = video.Size,
				Quality = video.Quality.Quality1,
				//Parts = GetParts(video.Size, _blockSize),
			};
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

				Qualitys = group.Select(item => item.Quality.Quality1)
			};
			return videoInfo;
		}

		private IEnumerable<FilePart> GetParts(long size, int blockSize)
		{
			var blockCount = size / blockSize;
			var parts = Enumerable.Range(0, (int)blockCount).Select(item => new FilePart
			{
				Index = item,
				Size = blockSize,
			}).ToList();

			var lastBlockSize = size - blockCount * blockSize;

			if (lastBlockSize > 0)
			{
				parts.Add(new FilePart
				{
					Index = parts.Count,
					Size = lastBlockSize,
				});
			}
			return parts;
		}
	}
}
