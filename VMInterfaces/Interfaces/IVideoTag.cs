using System;
using System.Collections.Generic;
using VMModels.Model;

namespace VMInterfaces
{
    internal interface IVideoTag
    {
        VideoTag GetTag(Guid Id);

        void DeleteVideoTag(VideoTag rec);

        List<VideoTag> GetTagList(Guid Id);

        bool Save();
    }
}
