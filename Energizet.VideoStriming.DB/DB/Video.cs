using System;
using System.Collections.Generic;

namespace Energizet.VideoStriming.DB.DB
{
    public partial class Video
    {
        public Video()
        {
            VideoQualities = new HashSet<VideoQuality>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Discription { get; set; } = null!;
        public long Views { get; set; }
        public byte[]? Preview { get; set; }
        public int Status { get; set; }

        public virtual ICollection<VideoQuality> VideoQualities { get; set; }
    }
}
