using System;
using System.Collections.Generic;

namespace Energizet.VideoStriming.DB.DB
{
    public partial class Format
    {
        public Format()
        {
            VideoQualities = new HashSet<VideoQuality>();
        }

        public Guid Id { get; set; }
        public string Format1 { get; set; } = null!;

        public virtual ICollection<VideoQuality> VideoQualities { get; set; }
    }
}
