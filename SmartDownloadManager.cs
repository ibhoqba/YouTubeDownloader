using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace YouTubeDownloader
{
public class SmartDownloadManager
    {
        private readonly string _statePath;
        private readonly Dictionary<string, DownloadState> _state;
        //
        public IReadOnlyDictionary<string, DownloadState> DownloadState => _state;

        public SmartDownloadManager(string stateFile = "download_state.json")
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _statePath = Path.Combine(baseDir, stateFile);
            _state = LoadState();
        }

        private Dictionary<string, DownloadState> LoadState()
        {
            if (!File.Exists(_statePath))
                return new Dictionary<string, DownloadState>();

            try
            {
                var json = File.ReadAllText(_statePath);
                var dict = JsonSerializer.Deserialize<Dictionary<string, DownloadState>>(json)
                           ?? new Dictionary<string, DownloadState>();

                // تنظيف أقدم من 7 أيام
                var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var cleaned = new Dictionary<string, DownloadState>();
                foreach (var kvp in dict)
                {
                    var ts = kvp.Value.Timestamp;
                    if (now - ts < 7 * 24 * 3600)
                        cleaned[kvp.Key] = kvp.Value;
                }
                if (cleaned.Count != dict.Count)
                    SaveState(cleaned);

                return cleaned;
            }
            catch
            {
                return new Dictionary<string, DownloadState>();
            }
        }
       

        private void SaveState(Dictionary<string, DownloadState>? dict = null)
        {
            try
            {
                var data = dict ?? _state;
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(_statePath, json);
            }
            catch
            {
                // تجاهل أو سجل الخطأ
            }
        }

        public void UpdateDownloadState(
            string url,
            string status,
            double progress = 0,
            int retryCount = 0,
            long expectedSize = 0,
            long downloadedSize = 0,
            string quality = "")
        {
            _state[url] = new DownloadState
            {
                Status = status,
                Progress = progress,
                RetryCount = retryCount,
                ExpectedSize = expectedSize,
                DownloadedSize = downloadedSize,
                Quality = quality,
                LastUpdated = DateTime.UtcNow.ToString("o"),
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
            SaveState();
        }

        public long GetExpectedSize(string url)
        {
            return _state.TryGetValue(url, out var s) ? s.ExpectedSize : 0;
        }

        public string GetQuality(string url)
        {
            return _state.TryGetValue(url, out var s) ? s.Quality ?? "" : "";
        }

        public void ClearState(string url)
        {
            if (_state.Remove(url))
                SaveState();
        }

        public List<string> GetFailedDownloads()
        {
            var list = new List<string>();
            foreach (var kvp in _state)
            {
                if (kvp.Value.Status == "error" || kvp.Value.Status == "retrying")
                    list.Add(kvp.Key);
            }
            return list;
        }

        public List<string> GetIncompleteDownloads()
        {
            var list = new List<string>();
            foreach (var kvp in _state)
            {
                if (kvp.Value.Status == "downloading" && kvp.Value.Progress < 95.0)
                    list.Add(kvp.Key);
            }
            return list;
        }
    }


}