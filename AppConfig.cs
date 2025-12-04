using System;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace YouTubeDownloader
{

    using System.Text.Json.Serialization;

    public class AppConfig
    {
        [JsonPropertyName("last_download_path")]
        public string LastDownloadPath { get; set; } = "downloads";

        [JsonPropertyName("last_playlist_file")]
        public string LastPlaylistFile { get; set; } = "";

        [JsonPropertyName("default_quality")]
        public string DefaultQuality { get; set; } = "720p";

        [JsonPropertyName("size_tolerance")]
        public double SizeTolerance { get; set; } = 0.9;

        [JsonPropertyName("download_subtitles")]
        public bool DownloadSubtitles { get; set; } = false;

        [JsonPropertyName("window_geometry")]
        public string WindowGeometry { get; set; } = "700x600";
        public string LastDownloadUrl { get; set; } = "";
    }
    public class DownloadState
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = "";

        [JsonPropertyName("progress")]
        public double Progress { get; set; }

        [JsonPropertyName("retry_count")]
        public int RetryCount { get; set; }

        [JsonPropertyName("expected_size")]
        public long ExpectedSize { get; set; }

        [JsonPropertyName("downloaded_size")]
        public long DownloadedSize { get; set; }

        [JsonPropertyName("quality")]
        public string Quality { get; set; } = "";

        [JsonPropertyName("last_updated")]
        public string LastUpdated { get; set; } = DateTime.UtcNow.ToString("o");

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}
