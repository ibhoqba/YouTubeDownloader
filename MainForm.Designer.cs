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
            dgvItems = new DataGridView();
            colTitle = new DataGridViewTextBoxColumn();
            colSize = new DataGridViewTextBoxColumn();
            colTime = new DataGridViewTextBoxColumn();
            colDownload = new DataGridViewCheckBoxColumn();
            colResume = new DataGridViewCheckBoxColumn();
            colPause = new DataGridViewButtonColumn();
            colProgress = new DataGridViewTextBoxColumn();
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            btnGetAllSizes = new Button();
            btnGetSelectedSizes = new Button();
            btnDeselectAll = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // lblUrl
            // 
            lblUrl.AutoSize = true;
            lblUrl.Location = new Point(6, 14);
            lblUrl.Name = "lblUrl";
            lblUrl.Size = new Size(81, 15);
            lblUrl.TabIndex = 0;
            lblUrl.Text = "YouTube URL:";
            // 
            // txtUrl
            // 
            txtUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtUrl.Location = new Point(87, 11);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new Size(555, 23);
            txtUrl.TabIndex = 1;
            txtUrl.TextChanged += txtUrl_TextChanged;
            // 
            // lblOutputPath
            // 
            lblOutputPath.AutoSize = true;
            lblOutputPath.Location = new Point(6, 40);
            lblOutputPath.Name = "lblOutputPath";
            lblOutputPath.Size = new Size(75, 15);
            lblOutputPath.TabIndex = 2;
            lblOutputPath.Text = "Output Path:";
            // 
            // txtOutputPath
            // 
            txtOutputPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtOutputPath.Location = new Point(79, 40);
            txtOutputPath.Name = "txtOutputPath";
            txtOutputPath.Size = new Size(431, 23);
            txtOutputPath.TabIndex = 3;
            // 
            // btnDownloadVideo
            // 
            btnDownloadVideo.Location = new Point(107, 95);
            btnDownloadVideo.Name = "btnDownloadVideo";
            btnDownloadVideo.Size = new Size(140, 30);
            btnDownloadVideo.TabIndex = 8;
            btnDownloadVideo.Text = "Download Video";
            btnDownloadVideo.UseVisualStyleBackColor = true;
            btnDownloadVideo.Click += btnDownloadVideo_Click;
            // 
            // btnDownloadPlaylist
            // 
            btnDownloadPlaylist.Location = new Point(253, 95);
            btnDownloadPlaylist.Name = "btnDownloadPlaylist";
            btnDownloadPlaylist.Size = new Size(118, 30);
            btnDownloadPlaylist.TabIndex = 9;
            btnDownloadPlaylist.Text = "Download Playlist";
            btnDownloadPlaylist.UseVisualStyleBackColor = true;
            btnDownloadPlaylist.Click += btnDownloadPlaylist_Click;
            // 
            // btnResumeFailed
            // 
            btnResumeFailed.Location = new Point(577, 94);
            btnResumeFailed.Name = "btnResumeFailed";
            btnResumeFailed.Size = new Size(92, 30);
            btnResumeFailed.TabIndex = 10;
            btnResumeFailed.Text = "Resume Failed Downloads";
            btnResumeFailed.UseVisualStyleBackColor = true;
            btnResumeFailed.Click += btnResumeFailed_Click;
            // 
            // lblQuality
            // 
            lblQuality.AutoSize = true;
            lblQuality.Location = new Point(19, 81);
            lblQuality.Name = "lblQuality";
            lblQuality.Size = new Size(48, 15);
            lblQuality.TabIndex = 5;
            lblQuality.Text = "Quality:";
            // 
            // comboQuality
            // 
            comboQuality.DropDownStyle = ComboBoxStyle.DropDownList;
            comboQuality.FormattingEnabled = true;
            comboQuality.Location = new Point(73, 73);
            comboQuality.Name = "comboQuality";
            comboQuality.Size = new Size(121, 23);
            comboQuality.TabIndex = 6;
            // 
            // chkSubtitles
            // 
            chkSubtitles.AutoSize = true;
            chkSubtitles.Location = new Point(200, 69);
            chkSubtitles.Name = "chkSubtitles";
            chkSubtitles.Size = new Size(127, 19);
            chkSubtitles.TabIndex = 7;
            chkSubtitles.Text = "Download subtitles";
            chkSubtitles.UseVisualStyleBackColor = true;
            // 
            // lblCurrentCaption
            // 
            lblCurrentCaption.AutoSize = true;
            lblCurrentCaption.Location = new Point(6, 128);
            lblCurrentCaption.Name = "lblCurrentCaption";
            lblCurrentCaption.Size = new Size(77, 15);
            lblCurrentCaption.TabIndex = 11;
            lblCurrentCaption.Text = "Current item:";
            // 
            // lblCurrent
            // 
            lblCurrent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblCurrent.Location = new Point(107, 128);
            lblCurrent.Name = "lblCurrent";
            lblCurrent.Size = new Size(523, 15);
            lblCurrent.TabIndex = 12;
            lblCurrent.Text = "Ready";
            // 
            // lblAuthCaption
            // 
            lblAuthCaption.AutoSize = true;
            lblAuthCaption.Location = new Point(-3, 143);
            lblAuthCaption.Name = "lblAuthCaption";
            lblAuthCaption.Size = new Size(70, 15);
            lblAuthCaption.TabIndex = 13;
            lblAuthCaption.Text = "Auth status:";
            // 
            // lblAuthStatus
            // 
            lblAuthStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblAuthStatus.Location = new Point(107, 143);
            lblAuthStatus.Name = "lblAuthStatus";
            lblAuthStatus.Size = new Size(546, 15);
            lblAuthStatus.TabIndex = 14;
            lblAuthStatus.Text = "(not tested)";
            // 
            // txtLog
            // 
            txtLog.Dock = DockStyle.Fill;
            txtLog.Location = new Point(3, 421);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(678, 57);
            txtLog.TabIndex = 16;
            // 
            // lblLog
            // 
            lblLog.AutoSize = true;
            lblLog.Location = new Point(0, 158);
            lblLog.Name = "lblLog";
            lblLog.Size = new Size(70, 15);
            lblLog.TabIndex = 15;
            lblLog.Text = "Activity log:";
            // 
            // btnBrowseOutput
            // 
            btnBrowseOutput.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowseOutput.Location = new Point(568, 40);
            btnBrowseOutput.Name = "btnBrowseOutput";
            btnBrowseOutput.Size = new Size(74, 25);
            btnBrowseOutput.TabIndex = 4;
            btnBrowseOutput.Text = "Browse...";
            btnBrowseOutput.UseVisualStyleBackColor = true;
            btnBrowseOutput.Click += btnBrowseOutput_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(125, 162);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(401, 23);
            progressBar1.TabIndex = 17;
            // 
            // dgvItems
            // 
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItems.Columns.AddRange(new DataGridViewColumn[] { colTitle, colSize, colTime, colDownload, colResume, colPause, colProgress });
            dgvItems.Dock = DockStyle.Fill;
            dgvItems.Location = new Point(3, 200);
            dgvItems.Name = "dgvItems";
            dgvItems.RowHeadersVisible = false;
            dgvItems.Size = new Size(678, 215);
            dgvItems.TabIndex = 18;
            // 
            // colTitle
            // 
            colTitle.Name = "colTitle";
            // 
            // colSize
            // 
            colSize.Name = "colSize";
            // 
            // colTime
            // 
            colTime.Name = "colTime";
            // 
            // colDownload
            // 
            colDownload.Name = "colDownload";
            // 
            // colResume
            // 
            colResume.Name = "colResume";
            // 
            // colPause
            // 
            colPause.Name = "colPause";
            // 
            // colProgress
            // 
            colProgress.Name = "colProgress";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(dgvItems, 0, 1);
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(txtLog, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 47.1291847F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 52.8708153F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            tableLayoutPanel1.Size = new Size(684, 481);
            tableLayoutPanel1.TabIndex = 19;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnGetAllSizes);
            groupBox1.Controls.Add(btnGetSelectedSizes);
            groupBox1.Controls.Add(btnDeselectAll);
            groupBox1.Controls.Add(txtUrl);
            groupBox1.Controls.Add(lblLog);
            groupBox1.Controls.Add(progressBar1);
            groupBox1.Controls.Add(lblAuthCaption);
            groupBox1.Controls.Add(lblAuthStatus);
            groupBox1.Controls.Add(lblCurrentCaption);
            groupBox1.Controls.Add(lblUrl);
            groupBox1.Controls.Add(lblOutputPath);
            groupBox1.Controls.Add(lblCurrent);
            groupBox1.Controls.Add(txtOutputPath);
            groupBox1.Controls.Add(btnBrowseOutput);
            groupBox1.Controls.Add(btnResumeFailed);
            groupBox1.Controls.Add(lblQuality);
            groupBox1.Controls.Add(btnDownloadPlaylist);
            groupBox1.Controls.Add(comboQuality);
            groupBox1.Controls.Add(btnDownloadVideo);
            groupBox1.Controls.Add(chkSubtitles);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(678, 191);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            // 
            // btnGetAllSizes
            // 
            btnGetAllSizes.Location = new Point(389, 69);
            btnGetAllSizes.Name = "btnGetAllSizes";
            btnGetAllSizes.Size = new Size(78, 55);
            btnGetAllSizes.TabIndex = 20;
            btnGetAllSizes.Text = "جلب حجم المحدد فقط";
            btnGetAllSizes.UseVisualStyleBackColor = true;
            btnGetAllSizes.Click += btnGetAllSizes_Click;
            // 
            // btnGetSelectedSizes
            // 
            btnGetSelectedSizes.Location = new Point(465, 69);
            btnGetSelectedSizes.Name = "btnGetSelectedSizes";
            btnGetSelectedSizes.Size = new Size(88, 55);
            btnGetSelectedSizes.TabIndex = 19;
            btnGetSelectedSizes.Text = "الحصول على الحجم";
            btnGetSelectedSizes.UseVisualStyleBackColor = true;
            btnGetSelectedSizes.Click += btnGetSelectedSizes_Click;
            // 
            // btnDeselectAll
            // 
            btnDeselectAll.Location = new Point(532, 155);
            btnDeselectAll.Name = "btnDeselectAll";
            btnDeselectAll.Size = new Size(140, 30);
            btnDeselectAll.TabIndex = 18;
            btnDeselectAll.Text = "الغاء تحديد الكل";
            btnDeselectAll.UseVisualStyleBackColor = true;
            btnDeselectAll.Click += btnDeselectAll_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(684, 481);
            Controls.Add(tableLayoutPanel1);
            MinimumSize = new Size(700, 520);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "YouTube Download Manager";
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            // 



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
        private DataGridView dgvItems;
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private DataGridViewTextBoxColumn colTitle;
        private DataGridViewTextBoxColumn colSize;
        private DataGridViewTextBoxColumn colTime;
        private DataGridViewCheckBoxColumn colDownload;
        private DataGridViewCheckBoxColumn colResume;
        private DataGridViewButtonColumn colPause;
        private DataGridViewTextBoxColumn colProgress;
        private Button btnDeselectAll;
        private Button btnGetAllSizes;
        private Button btnGetSelectedSizes;
    }
    }


