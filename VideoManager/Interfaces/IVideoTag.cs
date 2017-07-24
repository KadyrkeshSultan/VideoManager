using System;
using System.Collections.Generic;
using VideoManager.Model;

namespace VideoManager.Interfaces
{
    internal interface IVideoTag
    {
        VideoTag GetTag(Guid Id);

        void DeleteVideoTag(VideoTag rec);

        List<VideoTag> GetTagList(Guid Id);

        bool Save();
    }
}
