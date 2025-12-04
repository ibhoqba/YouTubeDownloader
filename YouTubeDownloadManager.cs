namespace YouTubeDownloader
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Collections.Generic;

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
            var args = $"-J \"{url}\"";
            var json = await RunProcessCaptureAsync(YtDlpPath, args);
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

        private async Task<string> RunProcessCaptureAsync(string fileName, string arguments, int timeoutSeconds = 120)
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

            Log($"▶ Running: {fileName} {arguments}");

            proc.Start();

            var outputTask = proc.StandardOutput.ReadToEndAsync();
            var errorTask = proc.StandardError.ReadToEndAsync();

            var waitTask = Task.Run(async () =>
            {
                await proc.WaitForExitAsync();
            });

            var completed = await Task.WhenAny(waitTask, Task.Delay(TimeSpan.FromSeconds(timeoutSeconds)));

            if (completed != waitTask)
            {
                try { proc.Kill(entireProcessTree: true); } catch { }
                throw new TimeoutException($"yt-dlp did not finish within {timeoutSeconds} seconds.");
            }

            var output = await outputTask;
            var error = await errorTask;

            if (proc.ExitCode != 0)
            {
                if (!string.IsNullOrWhiteSpace(error))
                    throw new Exception(error);
                throw new Exception($"yt-dlp exited with code {proc.ExitCode}");
            }

            return output;
        }

        private string BuildOutputTemplate(bool isPlaylist)
        {
            Directory.CreateDirectory(OutputPath);

            if (isPlaylist)
            {
                // C:\Downloads\اسم القائمة\001 - عنوان المقطع.ext
                return Path.Combine(
                    OutputPath,
                    "%(playlist_title)s",
                    "%(playlist_index)03d - %(title)s.%(ext)s"
                );
            }

            return Path.Combine(OutputPath, "%(title)s.%(ext)s");
        }

        private string BuildFormat(string quality)
        {
            if (string.Equals(quality, "audio", StringComparison.OrdinalIgnoreCase))
                return "bestaudio/best";

            if (string.Equals(quality, "best", StringComparison.OrdinalIgnoreCase))
                return "best";

            var digits = Regex.Match(quality, @"\d+").Value;
            if (!string.IsNullOrEmpty(digits))
                return $"best[height<={digits}]";

            return "best[height<=720]";
        }

        public async Task<(bool Success, string Message)> TestAuthenticationAsync()
        {
            try
            {
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
                var (title, _) = await GetVideoInfoAsync(url, quality);
                SetCurrentItem($"Downloading: {title}");
                Log($"🎬 Video: {title}");
                Log($"📏 Quality: {quality}");

                var format = BuildFormat(quality);
                var outtmpl = BuildOutputTemplate(isPlaylist: false);

                var subsArgs = downloadSubs
                    ? "--writesubtitles --writeautomaticsub --sub-langs en --embed-subs"
                    : "";

                var args =
                    $"-f \"{format}\" -o \"{outtmpl}\" --no-overwrites --continue --ignore-errors " +
                    $"--retries 10 --fragment-retries 20 --skip-unavailable-fragments " +
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

                var subsArgs = downloadSubs
                    ? "--writesubtitles --writeautomaticsub --sub-langs en --embed-subs"
                    : "";

                var args =
                    $"-f \"{format}\" -o \"{outtmpl}\" --yes-playlist --no-overwrites --continue --ignore-errors " +
                    $"--retries 10 --fragment-retries 20 --skip-unavailable-fragments " +
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

                if (line.Contains("[download]"))
                {
                    var m = Regex.Match(line, @"(\d+(\.\d+)?)%");
                    if (m.Success && double.TryParse(m.Groups[1].Value, out var perc))
                    {
                        DownloadManager.UpdateDownloadState(url, "downloading", perc);
                        OnProgress?.Invoke(perc);
                    }
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
            var all = new HashSet<string>(failed);
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

        /// <summary>
        /// Download a single playlist item (one index) - used in per-video loop from the UI.
        /// </summary>
        public async Task<string> DownloadPlaylistItemAsync(
            string playlistUrl,
            int index,
            string quality,
            bool downloadSubs)
        {
            Log($"🎬 Downloading playlist item #{index}...");
            SetCurrentItem($"Downloading playlist item #{index}...");

            var format = BuildFormat(quality);
            var outtmpl = BuildOutputTemplate(isPlaylist: true);

            var subsArgs = downloadSubs
                ? "--writesubtitles --writeautomaticsub --sub-langs en --embed-subs"
                : "";

            var args =
                $"-f \"{format}\" -o \"{outtmpl}\" --yes-playlist --playlist-items {index} " +
                "--no-overwrites --continue --ignore-errors " +
                "--retries 10 --fragment-retries 20 --skip-unavailable-fragments " +
                $"--socket-timeout 30 {subsArgs} \"{playlistUrl}\"";

            Log($"▶ Download cmd: {YtDlpPath} {args}");

            await RunDownloadWithProgress(playlistUrl, args, quality);

            Log($"✅ Playlist item #{index} downloaded!");
            SetCurrentItem("Ready");
            return "downloaded";
        }

        public async Task<MediaListInfo> GetMediaInfoAsync(string url, string quality)
        {
            var json = await RunProcessCaptureAsync(
                YtDlpPath,
                $"--flat-playlist -J \"{url}\"",
                timeoutSeconds: 120);

            var result = new MediaListInfo();

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("entries", out var entries) && entries.ValueKind == JsonValueKind.Array)
            {
                result.IsPlaylist = true;
                result.PlaylistTitle = root.TryGetProperty("title", out var pt)
                    ? pt.GetString() ?? "Playlist"
                    : "Playlist";

                int index = 0;
                foreach (var entry in entries.EnumerateArray())
                {
                    index++;

                    string id = entry.TryGetProperty("id", out var idProp)
                        ? idProp.GetString() ?? ""
                        : "";

                    string title = entry.TryGetProperty("title", out var tProp)
                        ? tProp.GetString() ?? $"Item {index}"
                        : $"Item {index}";

                    var item = new MediaItem
                    {
                        Id = id,
                        Title = title,
                        // رابط مباشر للفيديو
                        Url = string.IsNullOrEmpty(id)
                            ? url
                            : $"https://www.youtube.com/watch?v={id}"
                    };

                    if (entry.TryGetProperty("duration", out var durProp) &&
                        durProp.ValueKind == JsonValueKind.Number)
                    {
                        var seconds = durProp.GetDouble();
                        item.Duration = TimeSpan.FromSeconds(seconds);
                    }

                    if (entry.TryGetProperty("filesize", out var fsProp) &&
                        fsProp.ValueKind == JsonValueKind.Number)
                    {
                        item.FileSize = fsProp.GetInt64();
                    }
                    else if (entry.TryGetProperty("filesize_approx", out var fsaProp) &&
                             fsaProp.ValueKind == JsonValueKind.Number)
                    {
                        item.FileSize = fsaProp.GetInt64();
                    }

                    result.Items.Add(item);
                }
            }
            else
            {
                // فيديو منفرد
                var item = new MediaItem
                {
                    Id = root.TryGetProperty("id", out var idProp) ? idProp.GetString() ?? "" : "",
                    Title = root.TryGetProperty("title", out var tProp) ? tProp.GetString() ?? "Untitled" : "Untitled",
                    Url = url
                };

                if (root.TryGetProperty("duration", out var durProp) &&
                    durProp.ValueKind == JsonValueKind.Number)
                {
                    var seconds = durProp.GetDouble();
                    item.Duration = TimeSpan.FromSeconds(seconds);
                }

                if (root.TryGetProperty("filesize", out var fsProp) &&
                    fsProp.ValueKind == JsonValueKind.Number)
                {
                    item.FileSize = fsProp.GetInt64();
                }
                else if (root.TryGetProperty("filesize_approx", out var fsaProp) &&
                         fsaProp.ValueKind == JsonValueKind.Number)
                {
                    item.FileSize = fsaProp.GetInt64();
                }

                result.IsPlaylist = false;
                result.PlaylistTitle = item.Title;
                result.Items.Add(item);
            }

            Log(result.IsPlaylist
                ? $"✅ Playlist info loaded: {result.PlaylistTitle} ({result.Items.Count} items)"
                : $"✅ Video info loaded: {result.PlaylistTitle}");

            return result;
        }

        public async Task<long?> GetFileSizeForVideoAsync(string url, string quality)
        {
            var format = BuildFormat(quality);
            var args =
                $"-f \"{format}\" --skip-download --print \"%(filesize,filesize_approx)d\" \"{url}\"";

            try
            {
                Log($"▶ Size cmd: {YtDlpPath} {args}");
                var output = await RunProcessCaptureAsync(YtDlpPath, args, timeoutSeconds: 60);
                var line = output.Trim();
                if (long.TryParse(line, out var size) && size > 0)
                    return size;
            }
            catch (Exception ex)
            {
                Log($"⚠️ Error getting file size: {ex.Message}");
            }
            return null;
        }
    }

}
