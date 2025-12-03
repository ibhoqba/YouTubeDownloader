namespace YouTubeDownloader
{
    using System.Windows.Forms;

  
        partial class MainForm
        {
            /// <summary>
            /// Required designer variable.
            /// </summary>
            private System.ComponentModel.IContainer components = null;

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support – do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblUrl = new Label();
            txtUrl = new TextBox();
            lblOutputPath = new Label();
            txtOutputPath = new TextBox();
            btnDownloadVideo = new Button();
            btnDownloadPlaylist = new Button();
            btnResumeFailed = new Button();
            lblQuality = new Label();
            comboQuality = new ComboBox();
            chkSubtitles = new CheckBox();
            lblCurrentCaption = new Label();
            lblCurrent = new Label();
            lblAuthCaption = new Label();
            lblAuthStatus = new Label();
            txtLog = new TextBox();
            lblLog = new Label();
            btnBrowseOutput = new Button();
            progressBar1 = new ProgressBar();
            SuspendLayout();
            // 
            // lblUrl
            // 
            lblUrl.AutoSize = true;
            lblUrl.Location = new Point(12, 15);
            lblUrl.Name = "lblUrl";
            lblUrl.Size = new Size(81, 15);
            lblUrl.TabIndex = 0;
            lblUrl.Text = "YouTube URL:";
            // 
            // txtUrl
            // 
            txtUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtUrl.Location = new Point(110, 12);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new Size(560, 23);
            txtUrl.TabIndex = 1;
            // 
            // lblOutputPath
            // 
            lblOutputPath.AutoSize = true;
            lblOutputPath.Location = new Point(12, 47);
            lblOutputPath.Name = "lblOutputPath";
            lblOutputPath.Size = new Size(75, 15);
            lblOutputPath.TabIndex = 2;
            lblOutputPath.Text = "Output Path:";
            // 
            // txtOutputPath
            // 
            txtOutputPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtOutputPath.Location = new Point(110, 44);
            txtOutputPath.Name = "txtOutputPath";
            txtOutputPath.Size = new Size(480, 23);
            txtOutputPath.TabIndex = 3;
            // 
            // btnDownloadVideo
            // 
            btnDownloadVideo.Location = new Point(110, 116);
            btnDownloadVideo.Name = "btnDownloadVideo";
            btnDownloadVideo.Size = new Size(140, 30);
            btnDownloadVideo.TabIndex = 8;
            btnDownloadVideo.Text = "Download Video";
            btnDownloadVideo.UseVisualStyleBackColor = true;
            btnDownloadVideo.Click += btnDownloadVideo_Click;
            // 
            // btnDownloadPlaylist
            // 
            btnDownloadPlaylist.Location = new Point(260, 116);
            btnDownloadPlaylist.Name = "btnDownloadPlaylist";
            btnDownloadPlaylist.Size = new Size(150, 30);
            btnDownloadPlaylist.TabIndex = 9;
            btnDownloadPlaylist.Text = "Download Playlist";
            btnDownloadPlaylist.UseVisualStyleBackColor = true;
            btnDownloadPlaylist.Click += btnDownloadPlaylist_Click;
            // 
            // btnResumeFailed
            // 
            btnResumeFailed.Location = new Point(420, 116);
            btnResumeFailed.Name = "btnResumeFailed";
            btnResumeFailed.Size = new Size(160, 30);
            btnResumeFailed.TabIndex = 10;
            btnResumeFailed.Text = "Resume Failed Downloads";
            btnResumeFailed.UseVisualStyleBackColor = true;
            btnResumeFailed.Click += btnResumeFailed_Click;
            // 
            // lblQuality
            // 
            lblQuality.AutoSize = true;
            lblQuality.Location = new Point(12, 82);
            lblQuality.Name = "lblQuality";
            lblQuality.Size = new Size(48, 15);
            lblQuality.TabIndex = 5;
            lblQuality.Text = "Quality:";
            // 
            // comboQuality
            // 
            comboQuality.DropDownStyle = ComboBoxStyle.DropDownList;
            comboQuality.FormattingEnabled = true;
            comboQuality.Location = new Point(110, 79);
            comboQuality.Name = "comboQuality";
            comboQuality.Size = new Size(121, 23);
            comboQuality.TabIndex = 6;
            // 
            // chkSubtitles
            // 
            chkSubtitles.AutoSize = true;
            chkSubtitles.Location = new Point(250, 81);
            chkSubtitles.Name = "chkSubtitles";
            chkSubtitles.Size = new Size(127, 19);
            chkSubtitles.TabIndex = 7;
            chkSubtitles.Text = "Download subtitles";
            chkSubtitles.UseVisualStyleBackColor = true;
            // 
            // lblCurrentCaption
            // 
            lblCurrentCaption.AutoSize = true;
            lblCurrentCaption.Location = new Point(12, 162);
            lblCurrentCaption.Name = "lblCurrentCaption";
            lblCurrentCaption.Size = new Size(77, 15);
            lblCurrentCaption.TabIndex = 11;
            lblCurrentCaption.Text = "Current item:";
            // 
            // lblCurrent
            // 
            lblCurrent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblCurrent.Location = new Point(110, 162);
            lblCurrent.Name = "lblCurrent";
            lblCurrent.Size = new Size(560, 15);
            lblCurrent.TabIndex = 12;
            lblCurrent.Text = "Ready";
            // 
            // lblAuthCaption
            // 
            lblAuthCaption.AutoSize = true;
            lblAuthCaption.Location = new Point(12, 185);
            lblAuthCaption.Name = "lblAuthCaption";
            lblAuthCaption.Size = new Size(70, 15);
            lblAuthCaption.TabIndex = 13;
            lblAuthCaption.Text = "Auth status:";
            // 
            // lblAuthStatus
            // 
            lblAuthStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblAuthStatus.Location = new Point(110, 185);
            lblAuthStatus.Name = "lblAuthStatus";
            lblAuthStatus.Size = new Size(560, 15);
            lblAuthStatus.TabIndex = 14;
            lblAuthStatus.Text = "(not tested)";
            // 
            // txtLog
            // 
            txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtLog.Location = new Point(12, 235);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(658, 230);
            txtLog.TabIndex = 16;
            // 
            // lblLog
            // 
            lblLog.AutoSize = true;
            lblLog.Location = new Point(12, 215);
            lblLog.Name = "lblLog";
            lblLog.Size = new Size(70, 15);
            lblLog.TabIndex = 15;
            lblLog.Text = "Activity log:";
            // 
            // btnBrowseOutput
            // 
            btnBrowseOutput.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowseOutput.Location = new Point(596, 43);
            btnBrowseOutput.Name = "btnBrowseOutput";
            btnBrowseOutput.Size = new Size(74, 25);
            btnBrowseOutput.TabIndex = 4;
            btnBrowseOutput.Text = "Browse...";
            btnBrowseOutput.UseVisualStyleBackColor = true;
            btnBrowseOutput.Click += btnBrowseOutput_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(237, 203);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(100, 23);
            progressBar1.TabIndex = 17;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(684, 481);
            Controls.Add(progressBar1);
            Controls.Add(txtLog);
            Controls.Add(lblLog);
            Controls.Add(lblAuthStatus);
            Controls.Add(lblAuthCaption);
            Controls.Add(lblCurrent);
            Controls.Add(lblCurrentCaption);
            Controls.Add(btnResumeFailed);
            Controls.Add(btnDownloadPlaylist);
            Controls.Add(btnDownloadVideo);
            Controls.Add(chkSubtitles);
            Controls.Add(comboQuality);
            Controls.Add(lblQuality);
            Controls.Add(btnBrowseOutput);
            Controls.Add(txtOutputPath);
            Controls.Add(lblOutputPath);
            Controls.Add(txtUrl);
            Controls.Add(lblUrl);
            MinimumSize = new Size(700, 520);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "YouTube Download Manager";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUrl;
            private System.Windows.Forms.TextBox txtUrl;
            private System.Windows.Forms.Label lblOutputPath;
            private System.Windows.Forms.TextBox txtOutputPath;
            private System.Windows.Forms.Button btnBrowseOutput;
            private System.Windows.Forms.Label lblQuality;
            private System.Windows.Forms.ComboBox comboQuality;
            private System.Windows.Forms.CheckBox chkSubtitles;
            private System.Windows.Forms.Button btnDownloadVideo;
            private System.Windows.Forms.Button btnDownloadPlaylist;
            private System.Windows.Forms.Button btnResumeFailed;
            private System.Windows.Forms.Label lblCurrentCaption;
            private System.Windows.Forms.Label lblCurrent;
            private System.Windows.Forms.Label lblAuthCaption;
            private System.Windows.Forms.Label lblAuthStatus;
            private System.Windows.Forms.Label lblLog;
            private System.Windows.Forms.TextBox txtLog;
        private ProgressBar progressBar1;
    }
    }


