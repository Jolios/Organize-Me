namespace Organize_Me
{
    partial class TaskCard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskCard));
            this.bunifuLabel1 = new Bunifu.UI.WinForms.BunifuLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_TaskFailed = new Bunifu.UI.WinForms.BunifuImageButton();
            this.btn_TaskCompleted = new Bunifu.UI.WinForms.BunifuImageButton();
            this.bunifuPictureBox1 = new Bunifu.UI.WinForms.BunifuPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // bunifuLabel1
            // 
            this.bunifuLabel1.AutoEllipsis = false;
            this.bunifuLabel1.BackColor = System.Drawing.Color.White;
            this.bunifuLabel1.CursorType = null;
            this.bunifuLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuLabel1.ForeColor = System.Drawing.Color.Gray;
            this.bunifuLabel1.Location = new System.Drawing.Point(13, 44);
            this.bunifuLabel1.Name = "bunifuLabel1";
            this.bunifuLabel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bunifuLabel1.Size = new System.Drawing.Size(130, 15);
            this.bunifuLabel1.TabIndex = 5;
            this.bunifuLabel1.Text = "23 May 2020-25 May 2024";
            this.bunifuLabel1.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.bunifuLabel1.TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 21);
            this.label1.TabIndex = 4;
            this.label1.Text = "Task Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(67, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Full Name";
            // 
            // btn_TaskFailed
            // 
            this.btn_TaskFailed.ActiveImage = null;
            this.btn_TaskFailed.AllowAnimations = true;
            this.btn_TaskFailed.AllowBuffering = false;
            this.btn_TaskFailed.AllowZooming = false;
            this.btn_TaskFailed.BackColor = System.Drawing.Color.White;
            this.btn_TaskFailed.ErrorImage = ((System.Drawing.Image)(resources.GetObject("btn_TaskFailed.ErrorImage")));
            this.btn_TaskFailed.FadeWhenInactive = false;
            this.btn_TaskFailed.Flip = Bunifu.UI.WinForms.BunifuImageButton.FlipOrientation.Normal;
            this.btn_TaskFailed.Image = ((System.Drawing.Image)(resources.GetObject("btn_TaskFailed.Image")));
            this.btn_TaskFailed.ImageActive = null;
            this.btn_TaskFailed.ImageLocation = null;
            this.btn_TaskFailed.ImageMargin = 11;
            this.btn_TaskFailed.ImageSize = new System.Drawing.Size(24, 23);
            this.btn_TaskFailed.ImageZoomSize = new System.Drawing.Size(35, 34);
            this.btn_TaskFailed.InitialImage = ((System.Drawing.Image)(resources.GetObject("btn_TaskFailed.InitialImage")));
            this.btn_TaskFailed.Location = new System.Drawing.Point(197, 45);
            this.btn_TaskFailed.Name = "btn_TaskFailed";
            this.btn_TaskFailed.Rotation = 0;
            this.btn_TaskFailed.ShowActiveImage = true;
            this.btn_TaskFailed.ShowCursorChanges = true;
            this.btn_TaskFailed.ShowImageBorders = true;
            this.btn_TaskFailed.ShowSizeMarkers = false;
            this.btn_TaskFailed.Size = new System.Drawing.Size(35, 34);
            this.btn_TaskFailed.TabIndex = 9;
            this.btn_TaskFailed.ToolTipText = "";
            this.btn_TaskFailed.WaitOnLoad = false;
            this.btn_TaskFailed.Zoom = 11;
            this.btn_TaskFailed.ZoomSpeed = 10;
            this.btn_TaskFailed.Click += new System.EventHandler(this.btn_TaskFailed_Click);
            // 
            // btn_TaskCompleted
            // 
            this.btn_TaskCompleted.ActiveImage = null;
            this.btn_TaskCompleted.AllowAnimations = true;
            this.btn_TaskCompleted.AllowBuffering = false;
            this.btn_TaskCompleted.AllowZooming = false;
            this.btn_TaskCompleted.BackColor = System.Drawing.Color.White;
            this.btn_TaskCompleted.ErrorImage = ((System.Drawing.Image)(resources.GetObject("btn_TaskCompleted.ErrorImage")));
            this.btn_TaskCompleted.FadeWhenInactive = false;
            this.btn_TaskCompleted.Flip = Bunifu.UI.WinForms.BunifuImageButton.FlipOrientation.Normal;
            this.btn_TaskCompleted.Image = ((System.Drawing.Image)(resources.GetObject("btn_TaskCompleted.Image")));
            this.btn_TaskCompleted.ImageActive = null;
            this.btn_TaskCompleted.ImageLocation = null;
            this.btn_TaskCompleted.ImageMargin = 10;
            this.btn_TaskCompleted.ImageSize = new System.Drawing.Size(25, 24);
            this.btn_TaskCompleted.ImageZoomSize = new System.Drawing.Size(35, 34);
            this.btn_TaskCompleted.InitialImage = ((System.Drawing.Image)(resources.GetObject("btn_TaskCompleted.InitialImage")));
            this.btn_TaskCompleted.Location = new System.Drawing.Point(197, 5);
            this.btn_TaskCompleted.Name = "btn_TaskCompleted";
            this.btn_TaskCompleted.Rotation = 0;
            this.btn_TaskCompleted.ShowActiveImage = true;
            this.btn_TaskCompleted.ShowCursorChanges = true;
            this.btn_TaskCompleted.ShowImageBorders = true;
            this.btn_TaskCompleted.ShowSizeMarkers = false;
            this.btn_TaskCompleted.Size = new System.Drawing.Size(35, 34);
            this.btn_TaskCompleted.TabIndex = 8;
            this.btn_TaskCompleted.ToolTipText = "";
            this.btn_TaskCompleted.WaitOnLoad = false;
            this.btn_TaskCompleted.Zoom = 10;
            this.btn_TaskCompleted.ZoomSpeed = 10;
            this.btn_TaskCompleted.Click += new System.EventHandler(this.btn_TaskCompleted_Click);
            // 
            // bunifuPictureBox1
            // 
            this.bunifuPictureBox1.AllowFocused = false;
            this.bunifuPictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.bunifuPictureBox1.BorderRadius = 0;
            this.bunifuPictureBox1.IsCircle = false;
            this.bunifuPictureBox1.Location = new System.Drawing.Point(13, 73);
            this.bunifuPictureBox1.Name = "bunifuPictureBox1";
            this.bunifuPictureBox1.Size = new System.Drawing.Size(49, 49);
            this.bunifuPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.bunifuPictureBox1.TabIndex = 6;
            this.bunifuPictureBox1.TabStop = false;
            this.bunifuPictureBox1.Type = Bunifu.UI.WinForms.BunifuPictureBox.Types.Custom;
            // 
            // TaskCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btn_TaskFailed);
            this.Controls.Add(this.btn_TaskCompleted);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bunifuPictureBox1);
            this.Controls.Add(this.bunifuLabel1);
            this.Controls.Add(this.label1);
            this.Name = "TaskCard";
            this.Size = new System.Drawing.Size(235, 133);
            ((System.ComponentModel.ISupportInitialize)(this.bunifuPictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuUserControl bunifuUserControl1;
        public Bunifu.UI.WinForms.BunifuLabel bunifuLabel1;
        public Bunifu.UI.WinForms.BunifuPictureBox bunifuPictureBox1;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label2;
        private Bunifu.UI.WinForms.BunifuImageButton btn_TaskCompleted;
        private Bunifu.UI.WinForms.BunifuImageButton btn_TaskFailed;
    }
}
