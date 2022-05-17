using System;
using System.Collections.Generic;

namespace Energizet.VideoStriming.DB.DB
{
    public partial class Quality
    {
        public Quality()
        {
            VideoQualities = new HashSet<VideoQuality>();
        }

        public Guid Id { get; set; }
        public int Quality1 { get; set; }

        public virtual ICollection<VideoQuality> VideoQualities { get; set; }
    }
}
