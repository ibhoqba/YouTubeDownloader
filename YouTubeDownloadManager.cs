
namespace YouTubeDownloader
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class YouTubeDownloadManager
    {
        public event Action<double>? OnProgress;

        public string OutputPath { get; set; }
        public int MaxRetries { get; set; } = 3;
        public double SizeTolerance { get; set; } = 0.9;
        public string YtDlpPath { get; set; } = "yt-dlp.exe";

        public SmartDownloadManager DownloadManager { get; }
        public event Action<string>? OnLog;
        public event Action<string>? OnCurrentItemChanged;

        public YouTubeDownloadManager(string outputPath = "downloads", double sizeTolerance = 0.9)
        {
            OutputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outputPath);
            SizeTolerance = sizeTolerance;
            DownloadManager = new SmartDownloadManager();
        }

        private void Log(string msg)
        {
            OnLog?.Invoke(msg);
            Console.WriteLine(msg);
        }

        private void SetCurrentItem(string text)
        {
            OnCurrentItemChanged?.Invoke(text);
        }

        private async Task<(string Title, long? FileSize)> GetVideoInfoAsync(string url, string quality)
        {
            // استخدام yt-dlp -J للحصول على JSON
            var args = $"-J \"{url}\"";
            var json = await RunProcessCaptureAsync(YtDlpPath, args);
            //
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                string title = root.GetProperty("title").GetString() ?? "Unknown_Video";

                long? size = null;
                if (root.TryGetProperty("filesize", out var fsProp) && fsProp.ValueKind == JsonValueKind.Number)
                    size = fsProp.GetInt64();
                else if (root.TryGetProperty("filesize_approx", out var fsaProp) && fsaProp.ValueKind == JsonValueKind.Number)
                    size = fsaProp.GetInt64();

                return (title, size);
            }
            catch
            {
                return ("Error Unknown_Video", null);
            }
        }

        private async Task<string> RunProcessCaptureAsync(string fileName, string arguments)
        {
            var psi = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var proc = new Process { StartInfo = psi };
            proc.Start();
            var output = await proc.StandardOutput.ReadToEndAsync();
            var error = await proc.StandardError.ReadToEndAsync();
            await proc.WaitForExitAsync();

            if (proc.ExitCode != 0 && !string.IsNullOrWhiteSpace(error))
                throw new Exception(error);

            return output;
        }

        private string BuildFormat(string quality)
        {
            if (string.Equals(quality, "audio", StringComparison.OrdinalIgnoreCase))
                return "bestaudio/best";

            if (string.Equals(quality, "best", StringComparison.OrdinalIgnoreCase))
                return "best";

            // مثل 1080p / 720p
            var digits = Regex.Match(quality, @"\d+").Value;
            if (!string.IsNullOrEmpty(digits))
                return $"best[height<={digits}]";

            return "best[height<=720]";
        }

        private string BuildOutputTemplate(bool isPlaylist)
        {
            Directory.CreateDirectory(OutputPath);
            if (isPlaylist)
            {
                // downloads/%(playlist_title)s/%(title)s.%(ext)s
                return Path.Combine(OutputPath, "%(playlist_title)s", "%(title)s.%(ext)s");
            }
            return Path.Combine(OutputPath, "%(title)s.%(ext)s");
        }

        public async Task<(bool Success, string Message)> TestAuthenticationAsync()
        {
            try
            {
                // مجرد طلب بسيط للصفحة الرئيسية
                await RunProcessCaptureAsync(YtDlpPath, "--dump-single-json \"https://www.youtube.com\"");
                return (true, "✅ Authentication / basic access OK");
            }
            catch (Exception ex)
            {
                var msg = ex.Message.Contains("Sign in to confirm you're not a bot", StringComparison.OrdinalIgnoreCase)
                    ? "❌ Bot detection triggered"
                    : $"❌ Error: {ex.Message}";
                return (false, msg);
            }
        }

        public async Task<string> DownloadVideoAsync(string url, string quality, bool downloadSubs)
        {
            try
            {
                Log("🔍 Fetching video information...");
                //   if (url.Contains("list="))
                //  https://youtu.be/McifeJjrvpI?si=O_qVDTWf5mfVHwbt

                var (title, size) = await GetVideoInfoAsync(url, quality);
                SetCurrentItem($"Downloading: {title}");
                Log($"🎬 Video: {title}");
                Log($"📏 Quality: {quality}");

                var format = BuildFormat(quality);
                var outtmpl = BuildOutputTemplate(isPlaylist: false);

                // خيارات الترجمة
                //var subsArgs = downloadSubs
                //    ? "--writesubtitles --writeautomaticsub --sub-langs en --embed-subs"
                //    : "";

                //var args =
                //    $"-f \"{format}\" -o \"{outtmpl}\" --no-overwrites --continue --ignore-errors " +
                //    "--restrict-filenames --retries 10 --fragment-retries 20 --skip-unavailable-fragments " +
                //    "--socket-timeout 30 {subsArgs} \"{url}\"";
                var subsArgs = downloadSubs
                ? "--writesubtitles --writeautomaticsub --sub-langs en --embed-subs"
                     : "";

                var args =
                    $"-f \"{format}\" -o \"{outtmpl}\" --no-overwrites --continue --ignore-errors " +
                    $"--restrict-filenames --retries 10 --fragment-retries 20 --skip-unavailable-fragments " +
                    $"--socket-timeout 30 {subsArgs} \"{url}\"";

                await RunDownloadWithProgress(url, args, quality);

                Log("✅ Download completed successfully!");
                SetCurrentItem("Ready");
                return "downloaded";
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sign in to confirm you're not a bot", StringComparison.OrdinalIgnoreCase))
                {
                    Log("❌ Bot detection triggered!");
                    return "auth_error";
                }
                Log($"❌ Error: {ex.Message}");
                return "error";
            }
        }

        public async Task<string> DownloadPlaylistAsync(string url, string quality, bool downloadSubs)
        {
            try
            {
                Log("🔍 Fetching playlist information...");
                var format = BuildFormat(quality);
                var outtmpl = BuildOutputTemplate(isPlaylist: true);

                //var subsArgs = downloadSubs
                //    ? "--writesubtitles --writeautomaticsub --sub-langs en --embed-subs"
                //    : "";

                //var args =
                //    $"-f \"{format}\" -o \"{outtmpl}\" --yes-playlist --no-overwrites --continue --ignore-errors " +
                //    "--restrict-filenames --retries 10 --fragment-retries 20 --skip-unavailable-fragments " +
                //    "--socket-timeout 30 {subsArgs} \"{url}\"";
                var subsArgs = downloadSubs
                 ? "--writesubtitles --writeautomaticsub --sub-langs en --embed-subs"
                 : "";

                var args =
                    $"-f \"{format}\" -o \"{outtmpl}\" --yes-playlist --no-overwrites --continue --ignore-errors " +
                    $"--restrict-filenames --retries 10 --fragment-retries 20 --skip-unavailable-fragments " +
                    $"--socket-timeout 30 {subsArgs} \"{url}\"";

                SetCurrentItem("Downloading playlist...");
                await RunDownloadWithProgress(url, args, quality);

                Log("✅ Playlist download completed!");
                SetCurrentItem("Ready");
                return "downloaded";
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Sign in to confirm you're not a bot", StringComparison.OrdinalIgnoreCase))
                {
                    Log("❌ Bot detection triggered!");
                    return "auth_error";
                }
                Log($"❌ Error: {ex.Message}");
                return "error";
            }
        }

        private async Task RunDownloadWithProgress(string url, string arguments, string quality)
        {
            var psi = new ProcessStartInfo
            {
                FileName = YtDlpPath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            var proc = new Process { StartInfo = psi, EnableRaisingEvents = true };

            var tcs = new TaskCompletionSource<bool>();

            proc.OutputDataReceived += (s, e) =>
            {

                if (string.IsNullOrWhiteSpace(e.Data))
                    return;

                var line = e.Data;
                Log(line);

                // مثال سطر: [download]  42.3% of ...
                if (line.Contains("[download]"))
                {

                    var m = Regex.Match(line, @"(\d+(\.\d+)?)%");
                    if (m.Success)
                    {
                        if (double.TryParse(m.Groups[1].Value, out var perc))
                        {
                            DownloadManager.UpdateDownloadState(url, "downloading", perc);

                            // 🔥 نرسل التقدم للواجهة
                            OnProgress?.Invoke(perc);
                        }
                    }

                    //var m = Regex.Match(line, @"(\d+(\.\d+)?)%");
                    //if (m.Success)
                    //{
                    //    if (double.TryParse(m.Groups[1].Value, out var perc))
                    //    {
                    //        DownloadManager.UpdateDownloadState(
                    //            url,
                    //            "downloading",
                    //            perc,
                    //            expectedSize: 0,
                    //            downloadedSize: 0,
                    //            quality: quality
                    //        );
                    //    }
                    //}
                }
            };

            proc.ErrorDataReceived += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                    Log(e.Data);
            };

            proc.Exited += (s, e) =>
            {
                if (proc.ExitCode == 0)
                {
                    DownloadManager.ClearState(url);
                    tcs.TrySetResult(true);
                }
                else
                {
                    DownloadManager.UpdateDownloadState(url, "error");
                    tcs.TrySetException(new Exception($"yt-dlp exited with code {proc.ExitCode}"));
                }
                proc.Dispose();
            };

            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();

            await tcs.Task;
        }

        public async Task<int> ResumeFailedDownloadsAsync()
        {
            Log("🔄 Checking for failed downloads to resume...");

            var failed = DownloadManager.GetFailedDownloads();
            var incomplete = DownloadManager.GetIncompleteDownloads();
            var all = new System.Collections.Generic.HashSet<string>(failed);
            foreach (var u in incomplete)
                all.Add(u);

            int resumed = 0;
            int errors = 0;

            foreach (var url in all)
            {
                var state = DownloadManager.DownloadState[url];
                var quality = string.IsNullOrWhiteSpace(state.Quality) ? "720p" : state.Quality;

                Log($"🔄 Resuming: {url}");
                Log($"   Previous progress: {state.Progress:F1}%");
                Log($"   Quality: {quality}");

                try
                {
                    string result;
                    if (url.Contains("playlist?list="))
                        result = await DownloadPlaylistAsync(url, quality, downloadSubs: false);
                    else
                        result = await DownloadVideoAsync(url, quality, downloadSubs: false);

                    if (result == "downloaded")
                    {
                        resumed++;
                        Log($"✅ Successfully resumed: {url}");
                    }
                    else
                    {
                        errors++;
                        Log($"❌ Failed to resume: {url}");
                    }
                }
                catch (Exception ex)
                {
                    errors++;
                    Log($"❌ Error resuming {url}: {ex.Message}");
                }
            }

            if (resumed > 0 || errors > 0)
            {
                Log("\n📊 Resume Summary:");
                Log($"   ✅ Successfully resumed: {resumed}");
                Log($"   ❌ Failed to resume: {errors}");
            }
            else
            {
                Log("ℹ️ No failed downloads found to resume");
            }

            return resumed;
        }
    }

}
