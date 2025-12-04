using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace YouTubeDownloader
{
    using System.Threading.Tasks;

    public partial class MainForm : Form
    {
        private readonly ConfigManager _config;
        private YouTubeDownloadManager? _downloader;
        private MediaListInfo? _currentMediaInfo;
        private string _currentUrl = "";
        private bool _playlistLoadedForUrl = false;
        private int _activeRowForProgress = -1;

        public MainForm()
        {
            InitializeComponent();
            _config = new ConfigManager();

            txtOutputPath.Text = _config.GetLastDownloadPath();
            comboQuality.Items.AddRange(new object[] { "best", "1080p", "720p", "480p", "audio" });
            comboQuality.SelectedItem = _config.GetDefaultQuality();

            InitDownloader();
            txtUrl.Text = _config.GetLastDownloadUrl();
        }

        private void FillGrid(MediaListInfo info)
        {
            dgvItems.Rows.Clear();

            foreach (var item in info.Items)
            {
                var sizeText = item.FileSize.HasValue ? FormatSize(item.FileSize.Value) : "";
                var timeText = item.Duration.HasValue ? item.Duration.Value.ToString(@"hh\:mm\:ss") : "";

                int rowIndex = dgvItems.Rows.Add(
                    item.Title,
                    sizeText,
                    timeText,
                    true,   // Download checked by default
                    false,  // Resume (مستقبلاً)
                    "Pause",
                    "0%"    // Progress
                );
                dgvItems.Rows[rowIndex].Tag = item;
            }
        }

        private string FormatSize(long bytes)
        {
            double size = bytes;
            string[] units = { "B", "KiB", "MiB", "GiB" };
            int unit = 0;
            while (size >= 1024 && unit < units.Length - 1)
            {
                size /= 1024;
                unit++;
            }
            return $"{size:0.##} {units[unit]}";
        }

        private void InitDownloader()
        {
            _downloader = new YouTubeDownloadManager(txtOutputPath.Text,
                sizeTolerance: _config.Config.SizeTolerance);
            _downloader.OnLog += AppendLog;
            _downloader.OnCurrentItemChanged += s =>
            {
                if (InvokeRequired)
                    BeginInvoke(new Action(() => lblCurrent.Text = s));
                else
                    lblCurrent.Text = s;
            };
            _downloader.OnProgress += p =>
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            progressBar1.Value = Math.Min(100, Math.Max(0, (int)p));
                        }
                        catch { }

                        if (_activeRowForProgress >= 0 && _activeRowForProgress < dgvItems.Rows.Count)
                        {
                            dgvItems.Rows[_activeRowForProgress]
                                     .Cells["colProgress"].Value = $"{p:0}%";
                        }
                    }));
                }
                else
                {
                    try
                    {
                        progressBar1.Value = Math.Min(100, Math.Max(0, (int)p));
                    }
                    catch { }

                    if (_activeRowForProgress >= 0 && _activeRowForProgress < dgvItems.Rows.Count)
                    {
                        dgvItems.Rows[_activeRowForProgress]
                                 .Cells["colProgress"].Value = $"{p:0}%";
                    }
                }
            };

            TestAuth();
        }

        private async void TestAuth()
        {
            if (_downloader == null) return;
            var (ok, msg) = await _downloader.TestAuthenticationAsync();
            lblAuthStatus.Text = msg;
        }

        private void AppendLog(string msg)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(AppendLog), msg);
                return;
            }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}{Environment.NewLine}");
        }

        private async void btnDownloadVideo_Click(object sender, EventArgs e)
        {
            if (_downloader == null) return;

            var url = txtUrl.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Please enter a video URL.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _config.SetLastDownloadUrl(url);
            _downloader.OutputPath = txtOutputPath.Text;
            _config.SetLastDownloadPath(txtOutputPath.Text);
            _config.SetDefaultQuality(comboQuality.SelectedItem?.ToString() ?? "720p");

            var quality = comboQuality.SelectedItem?.ToString() ?? "720p";
            var subs = chkSubtitles.Checked;

            AppendLog("📋 Loading video info...");
            var info = await _downloader.GetMediaInfoAsync(url, quality);
            _currentMediaInfo = info;
            _currentUrl = url;

            FillGrid(info);

            if (_currentMediaInfo.IsPlaylist)
            {
                // إذا الرابط Playlist نمنع تحميله هنا ونطلب من المستخدم استخدام زر الـ Playlist
                btnDownloadVideo.Enabled = false;
                AppendLog("ℹ️ This URL is a playlist. Use 'Download Playlist' and select videos from the grid.");
                return;
            }

            btnDownloadVideo.Enabled = true;

            // تحديث الحجم حسب الـ quality المختارة
            _ = Task.Run(async () =>
            {
                try
                {
                    var size = await _downloader.GetFileSizeForVideoAsync(url, quality);
                    if (size.HasValue)
                    {
                        var text = FormatSize(size.Value);
                        BeginInvoke(new Action(() =>
                        {
                            if (dgvItems.Rows.Count > 0)
                            {
                                dgvItems.Rows[0].Cells["colSize"].Value = text;
                            }
                        }));
                    }
                }
                catch (Exception ex)
                {
                    AppendLog($"⚠️ Error updating video size: {ex.Message}");
                }
            });

            // تحميل الفيديو المنفرد مع Progress للصف الأول
            _activeRowForProgress = 0;
            var result = await _downloader.DownloadVideoAsync(url, quality, subs);
            _activeRowForProgress = -1;

            if (result == "downloaded")
                AppendLog("🎉 Video downloaded successfully.");
            else if (result == "auth_error")
                AppendLog("💥 Download failed due to auth / bot detection.");
            else
                AppendLog("💥 Download failed.");
        }

        private async void btnDownloadPlaylist_Click(object sender, EventArgs e)
        {
            if (_downloader == null) return;

            var url = txtUrl.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Please enter a playlist URL.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _config.SetLastDownloadUrl(url);
            _downloader.OutputPath = txtOutputPath.Text;
            _config.SetLastDownloadPath(txtOutputPath.Text);
            _config.SetDefaultQuality(comboQuality.SelectedItem?.ToString() ?? "720p");

            var quality = comboQuality.SelectedItem?.ToString() ?? "720p";
            var subs = chkSubtitles.Checked;

            try
            {
                // المرحلة الأولى: تحميل معلومات القائمة وعرضها فقط
                if (_currentMediaInfo == null || _currentUrl != url || !_currentMediaInfo.IsPlaylist || !_playlistLoadedForUrl)
                {
                    AppendLog("📋 Loading playlist info...");
                    this.Cursor = Cursors.WaitCursor;

                    _currentMediaInfo = await _downloader.GetMediaInfoAsync(url, quality);
                    _currentUrl = url;

                    FillGrid(_currentMediaInfo);

                    if (_currentMediaInfo.IsPlaylist)
                    {
                        AppendLog($"ℹ️ Playlist \"{_currentMediaInfo.PlaylistTitle}\" loaded with {_currentMediaInfo.Items.Count} videos.");
                        AppendLog("✔️ Check the videos you want, then click 'Download Playlist' again to download them.");
                        btnDownloadVideo.Enabled = false;
                        _playlistLoadedForUrl = true;
                    }
                    else
                    {
                        AppendLog("ℹ️ This URL is not a playlist (single video).");
                        btnDownloadVideo.Enabled = true;
                        _playlistLoadedForUrl = false;
                    }

                    return; // لا نبدأ التحميل في أول ضغطة
                }

                // المرحلة الثانية: تحميل المقاطع المحددة فقط، واحدة واحدة
                if (_currentMediaInfo == null || !_currentMediaInfo.IsPlaylist)
                {
                    AppendLog("ℹ️ This URL is not a playlist. Nothing to download as playlist.");
                    return;
                }

                var selectedRows = new List<int>();
                for (int i = 0; i < dgvItems.Rows.Count; i++)
                {
                    var row = dgvItems.Rows[i];
                    bool toDownload = row.Cells["colDownload"].Value is bool b && b;
                    if (toDownload)
                        selectedRows.Add(i);
                }

                if (selectedRows.Count == 0)
                {
                    MessageBox.Show("No videos selected to download.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                AppendLog($"▶ Downloading {selectedRows.Count} selected videos from playlist...");

                foreach (int rowIndex in selectedRows)
                {
                    int playlistIndex = rowIndex + 1; // yt-dlp uses 1-based playlist index
                    var row = dgvItems.Rows[rowIndex];
                    var item = row.Tag as MediaItem;

                    AppendLog($"▶ Downloading item #{playlistIndex}: {item?.Title}");
                    _activeRowForProgress = rowIndex;
                    row.Cells["colProgress"].Value = "0%";

                    var result = await _downloader.DownloadPlaylistItemAsync(_currentUrl, playlistIndex, quality, subs);

                    if (result == "downloaded")
                    {
                        row.Cells["colProgress"].Value = "100%";
                        AppendLog($"✅ Done: {item?.Title}");
                    }
                    else if (result == "auth_error")
                    {
                        AppendLog($"💥 Auth/Bot detection error while downloading: {item?.Title}");
                        break; // ممكن توقف حسب رغبتك
                    }
                    else
                    {
                        AppendLog($"💥 Error downloading: {item?.Title}");
                    }
                }

                _activeRowForProgress = -1;
                AppendLog("🎉 Selected playlist videos download process finished.");
            }
            catch (TimeoutException tex)
            {
                AppendLog($"⏱ Timeout while loading/downloading playlist: {tex.Message}");
                MessageBox.Show("yt-dlp took too long to respond.", "Timeout",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                AppendLog($"❌ Error in playlist operation: {ex.Message}");
                MessageBox.Show("Error in playlist operation:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async Task UpdateRowSizeAsync(int rowIndex)
        {
            if (_downloader == null || _currentMediaInfo == null)
                return;
            if (rowIndex < 0 || rowIndex >= dgvItems.Rows.Count)
                return;

            var row = dgvItems.Rows[rowIndex];
            if (row.Tag is not MediaItem item)
                return;

            var quality = comboQuality.SelectedItem?.ToString() ?? "720p";

            long? size = null;
            try
            {
                size = await _downloader.GetFileSizeForVideoAsync(item.Url, quality);
            }
            catch (Exception ex)
            {
                AppendLog($"⚠️ Error getting size for row {rowIndex + 1}: {ex.Message}");
            }

            if (size.HasValue)
            {
                var text = FormatSize(size.Value);
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        row.Cells["colSize"].Value = text;
                    }));
                }
                else
                {
                    row.Cells["colSize"].Value = text;
                }
            }
        }

        private async void btnResumeFailed_Click(object sender, EventArgs e)
        {
            if (_downloader == null) return;
            var resumed = await _downloader.ResumeFailedDownloadsAsync();
            if (resumed > 0)
                AppendLog($"🎉 Successfully resumed {resumed} downloads!");
            else
                AppendLog("ℹ️ No downloads were resumed.");
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtOutputPath.Text = fbd.SelectedPath;
            }
        }

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {
        }

        private void comboQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.Cells["colDownload"] is DataGridViewCheckBoxCell cell)
                {
                    cell.Value = false;
                }
            }
        }

        private async void btnGetSelectedSizes_Click(object sender, EventArgs e)
        {
            if (_currentMediaInfo == null || dgvItems.Rows.Count == 0)
            {
                MessageBox.Show("No items loaded.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            AppendLog("📏 Getting sizes for selected items...");

            try
            {
                for (int i = 0; i < dgvItems.Rows.Count; i++)
                {
                    var row = dgvItems.Rows[i];
                    bool toDownload = row.Cells["colDownload"].Value is bool b && b;
                    if (!toDownload)
                        continue;

                    await UpdateRowSizeAsync(i);
                }

                AppendLog("✅ Sizes for selected items updated.");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async void btnGetAllSizes_Click(object sender, EventArgs e)
        {
            if (_currentMediaInfo == null || dgvItems.Rows.Count == 0)
            {
                MessageBox.Show("No items loaded.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            AppendLog("📏 Getting sizes for ALL items...");

            try
            {
                for (int i = 0; i < dgvItems.Rows.Count; i++)
                {
                    await UpdateRowSizeAsync(i);
                }

                AppendLog("✅ Sizes for all items updated.");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}
