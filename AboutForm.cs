using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace YouTubeDownloader
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }


        private void EmailLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("mailto:ib-1515@hotmail.com");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening email client: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}