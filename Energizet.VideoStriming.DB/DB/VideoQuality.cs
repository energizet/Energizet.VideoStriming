using System;
using System.Collections.Generic;

namespace Energizet.VideoStriming.DB.DB
{
    public partial class VideoQuality
    {
        public Guid Id { get; set; }
        public Guid QualityId { get; set; }
        public Guid VideoId { get; set; }
        public Guid FormatId { get; set; }
        public long Size { get; set; }

        public virtual Format Format { get; set; } = null!;
        public virtual Quality Quality { get; set; } = null!;
        public virtual Video Video { get; set; } = null!;
    }
}
