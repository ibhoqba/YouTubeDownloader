using System;
using System.IO;
using System.Text.Json;

namespace YouTubeDownloader
{

public class ConfigManager
    {
        private readonly string _configPath;
        public AppConfig Config { get; private set; }
        public string GetLastDownloadUrl() => Config.LastDownloadUrl;
        public ConfigManager(string fileName = "config.json")
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _configPath = Path.Combine(baseDir, fileName);
            Config = LoadConfig();
        }
        public void SetLastDownloadUrl(string url)
        {
            Config.LastDownloadUrl = url;
            Save();
        }
        private AppConfig LoadConfig()
        {
            try
            {
                if (File.Exists(_configPath))
                {
                    var json = File.ReadAllText(_configPath);
                    var cfg = JsonSerializer.Deserialize<AppConfig>(json);
                    return cfg ?? new AppConfig();
                }
            }
            catch
            {
                // ignore and use defaults
            }
            return new AppConfig();
        }

        public void Save()
        {
            try
            {
                var json = JsonSerializer.Serialize(Config, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(_configPath, json);
            }
            catch
            {
                // يمكن تسجيل الخطأ في Log خارجي لو أحببت
            }
        }

        // Helpers مشابهة للبايثون
        public string GetLastDownloadPath() => Config.LastDownloadPath;
        public void SetLastDownloadPath(string path)
        {
            Config.LastDownloadPath = path;
            Save();
        }

        public string GetLastPlaylistFile() => Config.LastPlaylistFile;
        public void SetLastPlaylistFile(string filePath)
        {
            Config.LastPlaylistFile = filePath;
            Save();
        }

        public string GetDefaultQuality() => Config.DefaultQuality;
        public void SetDefaultQuality(string q)
        {
            Config.DefaultQuality = q;
            Save();
        }
    }


}