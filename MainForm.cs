using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace YouTubeDownloader
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        private readonly ConfigManager _config;
        private YouTubeDownloadManager? _downloader;

        public MainForm()
        {
            InitializeComponent(); // صمم العناصر بالـ Designer
            _config = new ConfigManager();

            txtOutputPath.Text = _config.GetLastDownloadPath();
            comboQuality.Items.AddRange(new object[] { "best", "1080p", "720p", "480p", "audio" });
            comboQuality.SelectedItem = _config.GetDefaultQuality();

            InitDownloader();
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
                MessageBox.Show("Please enter a YouTube URL.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _downloader.OutputPath = txtOutputPath.Text;
            _config.SetLastDownloadPath(txtOutputPath.Text);
            _config.SetDefaultQuality(comboQuality.SelectedItem?.ToString() ?? "720p");

            var quality = comboQuality.SelectedItem?.ToString() ?? "720p";
            var subs = chkSubtitles.Checked;

            var result = await _downloader.DownloadVideoAsync(url, quality, subs);

            if (result == "downloaded")
                AppendLog("🎉 Download completed successfully!");
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

            _downloader.OutputPath = txtOutputPath.Text;
            _config.SetLastDownloadPath(txtOutputPath.Text);
            _config.SetDefaultQuality(comboQuality.SelectedItem?.ToString() ?? "720p");

            var quality = comboQuality.SelectedItem?.ToString() ?? "720p";
            var subs = chkSubtitles.Checked;

            var result = await _downloader.DownloadPlaylistAsync(url, quality, subs);

            if (result == "downloaded")
                AppendLog("🎉 Playlist download completed!");
            else
                AppendLog("💥 Playlist download failed.");
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
}


}