namespace YouTubeDownloader
{
    partial class AboutForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        //private void InitializeComponent()
        //{
        //    this.components = new System.ComponentModel.Container();
        //    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //    this.ClientSize = new System.Drawing.Size(800, 450);
        //    this.Text = "AboutForm";
        //}
        private void InitializeComponent()
        {
            this.Text = "About Me";
            this.Size = new Size(400, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var mainPanel = new TableLayoutPanel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Padding = new Padding(20);
            mainPanel.RowCount = 7;

            // Title
            var titleLabel = new Label();
            titleLabel.Text = "🎬 YouTube Downloader";
            titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(titleLabel, 0, 0);

            // Developer label
            var developerLabel1 = new Label();
            developerLabel1.Text = "Developed by:";
            developerLabel1.Font = new Font("Arial", 10, FontStyle.Bold);
            developerLabel1.TextAlign = ContentAlignment.MiddleCenter;
            developerLabel1.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(developerLabel1, 0, 1);

            // Name
            var nameLabel = new Label();
            nameLabel.Text = "Ebrahim Thabit";
            nameLabel.Font = new Font("Arial", 12, FontStyle.Bold);
            nameLabel.TextAlign = ContentAlignment.MiddleCenter;
            nameLabel.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(nameLabel, 0, 2);

            // Email
            var emailPanel = new FlowLayoutPanel();
            emailPanel.FlowDirection = FlowDirection.LeftToRight;
            emailPanel.Dock = DockStyle.Fill;

            var emailStaticLabel = new Label();
            emailStaticLabel.Text = "Email:";
            emailStaticLabel.AutoSize = true;

            var emailLinkLabel = new LinkLabel();
            emailLinkLabel.Text = "ib-1515@hotmail.com";
            emailLinkLabel.AutoSize = true;
            emailLinkLabel.LinkClicked += EmailLinkLabel_LinkClicked;

            emailPanel.Controls.Add(emailStaticLabel);
            emailPanel.Controls.Add(emailLinkLabel);

            mainPanel.Controls.Add(emailPanel, 0, 3);

            // Features title
            var featuresTitle = new Label();
            featuresTitle.Text = "Key Features:";
            featuresTitle.Font = new Font("Arial", 10, FontStyle.Bold);
            featuresTitle.TextAlign = ContentAlignment.MiddleLeft;
            featuresTitle.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(featuresTitle, 0, 4);

            // Features list
            var featuresList = new ListBox();
            featuresList.Items.AddRange(new object[]
            {
                "✅ Download videos and playlists",
                "✅ Multiple quality options",
                "✅ Smart resume functionality",
                "✅ Anti-bot protection",
                "✅ Batch download support",
                "✅ English subtitles",
                "✅ Remember settings and paths"
            });
            featuresList.BorderStyle = BorderStyle.None;
            featuresList.Enabled = false;
            featuresList.BackColor = this.BackColor;
            featuresList.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(featuresList, 0, 5);

            // Close button
            var closeButton = new Button();
            closeButton.Text = "Close";
            closeButton.Click += (s, e) => this.Close();
            closeButton.Size = new Size(100, 30);
            closeButton.Anchor = AnchorStyles.None;
            mainPanel.Controls.Add(closeButton, 0, 6);

            // Set row styles
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

            this.Controls.Add(mainPanel);
        }

        #endregion
    }
}