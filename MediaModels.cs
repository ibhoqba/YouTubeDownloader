using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTubeDownloader
{
  
public class MediaItem
    {
        public string Id { get; set; } = "";
        public string Url { get; set; } = "";
        public string Title { get; set; } = "";
        public long? FileSize { get; set; }
        public TimeSpan? Duration { get; set; }
    }

    public class MediaListInfo
    {
        public bool IsPlaylist { get; set; }
        public string PlaylistTitle { get; set; } = "";
        public List<MediaItem> Items { get; set; } = new();
    }

}
