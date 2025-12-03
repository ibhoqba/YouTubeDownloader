using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace YouTubeDownloader
{
    static class Program
    {
        private static Mutex mutex = null;

        [STAThread]
        static void Main()
        {
            const string appName = "YouTubeDownloader";
            bool createdNew;

            mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                // Another instance is already running
                MessageBox.Show("Another instance of YouTube Downloader is already running!\n" +
                               "Please close the existing instance before opening a new one.",
                               "YouTube Downloader",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            // Keep the mutex referenced until the application closes
            GC.KeepAlive(mutex);
        }
    }
}