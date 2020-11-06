namespace FPX
{
    partial class AssetWindow
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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssetWindow));
            this.assetListView = new System.Windows.Forms.ListView();
            this.assetWindowImageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.folderUpButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // assetListView
            // 
            this.assetListView.AllowDrop = true;
            this.assetListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetListView.HideSelection = false;
            this.assetListView.LargeImageList = this.assetWindowImageList;
            this.assetListView.Location = new System.Drawing.Point(0, 28);
            this.assetListView.Name = "assetListView";
            this.assetListView.Size = new System.Drawing.Size(369, 439);
            this.assetListView.TabIndex = 0;
            this.assetListView.UseCompatibleStateImageBehavior = false;
            this.assetListView.DragLeave += new System.EventHandler(this.assetListView_DragLeave);
            this.assetListView.DoubleClick += new System.EventHandler(this.assetListView_DoubleClick);
            this.assetListView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.assetListView_MouseDown);
            // 
            // assetWindowImageList
            // 
            this.assetWindowImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("assetWindowImageList.ImageStream")));
            this.assetWindowImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.assetWindowImageList.Images.SetKeyName(0, "FileType_256x.png");
            this.assetWindowImageList.Images.SetKeyName(1, "3DScene_256x.png");
            this.assetWindowImageList.Images.SetKeyName(2, "SoundFile_256x.png");
            this.assetWindowImageList.Images.SetKeyName(3, "Image_256x.png");
            this.assetWindowImageList.Images.SetKeyName(4, "Folder_256x.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.folderUpButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(369, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // folderUpButton
            // 
            this.folderUpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.folderUpButton.Image = ((System.Drawing.Image)(resources.GetObject("folderUpButton.Image")));
            this.folderUpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.folderUpButton.Name = "folderUpButton";
            this.folderUpButton.Size = new System.Drawing.Size(23, 22);
            this.folderUpButton.Text = "toolStripButton1";
            this.folderUpButton.Click += new System.EventHandler(this.folderUpButton_Click);
            // 
            // AssetWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 467);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.assetListView);
            this.Name = "AssetWindow";
            this.Text = "AssetWindow";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.AssetWindow_Load);
            this.Validated += new System.EventHandler(this.AssetWindow_Validated);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListView assetListView;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ImageList assetWindowImageList;
        private System.Windows.Forms.ToolStripButton folderUpButton;
    }
}