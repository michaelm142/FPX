namespace FPX
{
    partial class EditorWindow
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renderModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deferredToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deferredDebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputGPUTexturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sceneViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hierarchyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.RenderInterval = new System.Windows.Forms.Timer(this.components);
            this.dockContainer1 = new Crom.Controls.DockContainer();
            this.analizerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.windowsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(991, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadSceneToolStripMenuItem,
            this.saveSceneToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadSceneToolStripMenuItem
            // 
            this.loadSceneToolStripMenuItem.Name = "loadSceneToolStripMenuItem";
            this.loadSceneToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.loadSceneToolStripMenuItem.Text = "Load Scene";
            this.loadSceneToolStripMenuItem.Click += new System.EventHandler(this.loadSceneToolStripMenuItem_Click);
            // 
            // saveSceneToolStripMenuItem
            // 
            this.saveSceneToolStripMenuItem.Name = "saveSceneToolStripMenuItem";
            this.saveSceneToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.saveSceneToolStripMenuItem.Text = "Save Scene";
            this.saveSceneToolStripMenuItem.Click += new System.EventHandler(this.saveSceneToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(131, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renderModeToolStripMenuItem,
            this.outputGPUTexturesToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // renderModeToolStripMenuItem
            // 
            this.renderModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultToolStripMenuItem,
            this.deferredToolStripMenuItem,
            this.deferredDebugToolStripMenuItem});
            this.renderModeToolStripMenuItem.Name = "renderModeToolStripMenuItem";
            this.renderModeToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.renderModeToolStripMenuItem.Text = "Render Mode";
            // 
            // defaultToolStripMenuItem
            // 
            this.defaultToolStripMenuItem.Checked = true;
            this.defaultToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
            this.defaultToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.defaultToolStripMenuItem.Text = "Default";
            this.defaultToolStripMenuItem.Click += new System.EventHandler(this.defaultToolStripMenuItem_Click);
            // 
            // deferredToolStripMenuItem
            // 
            this.deferredToolStripMenuItem.Name = "deferredToolStripMenuItem";
            this.deferredToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.deferredToolStripMenuItem.Text = "Deferred";
            this.deferredToolStripMenuItem.Click += new System.EventHandler(this.deferredToolStripMenuItem_Click);
            // 
            // deferredDebugToolStripMenuItem
            // 
            this.deferredDebugToolStripMenuItem.Name = "deferredDebugToolStripMenuItem";
            this.deferredDebugToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.deferredDebugToolStripMenuItem.Text = "Deferred Debug";
            this.deferredDebugToolStripMenuItem.Click += new System.EventHandler(this.deferredDebugToolStripMenuItem_Click);
            // 
            // outputGPUTexturesToolStripMenuItem
            // 
            this.outputGPUTexturesToolStripMenuItem.Name = "outputGPUTexturesToolStripMenuItem";
            this.outputGPUTexturesToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.outputGPUTexturesToolStripMenuItem.Text = "Output GPU Textures";
            this.outputGPUTexturesToolStripMenuItem.Click += new System.EventHandler(this.outputGPUTexturesToolStripMenuItem_Click);
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sceneViewToolStripMenuItem,
            this.hierarchyToolStripMenuItem,
            this.analizerToolStripMenuItem});
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.windowsToolStripMenuItem.Text = "Windows";
            // 
            // sceneViewToolStripMenuItem
            // 
            this.sceneViewToolStripMenuItem.Name = "sceneViewToolStripMenuItem";
            this.sceneViewToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.sceneViewToolStripMenuItem.Text = "Scene View";
            this.sceneViewToolStripMenuItem.Click += new System.EventHandler(this.sceneViewToolStripMenuItem_Click);
            // 
            // hierarchyToolStripMenuItem
            // 
            this.hierarchyToolStripMenuItem.Name = "hierarchyToolStripMenuItem";
            this.hierarchyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.hierarchyToolStripMenuItem.Text = "Hierarchy";
            this.hierarchyToolStripMenuItem.Click += new System.EventHandler(this.hierarchyToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // RenderInterval
            // 
            this.RenderInterval.Enabled = true;
            this.RenderInterval.Interval = 16;
            this.RenderInterval.Tick += new System.EventHandler(this.InputUpdate);
            // 
            // dockContainer1
            // 
            this.dockContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dockContainer1.BackColor = System.Drawing.SystemColors.Window;
            this.dockContainer1.BottomPanelHeight = 150;
            this.dockContainer1.LeftPanelWidth = 150;
            this.dockContainer1.Location = new System.Drawing.Point(0, 27);
            this.dockContainer1.MinimumSize = new System.Drawing.Size(504, 528);
            this.dockContainer1.Name = "dockContainer1";
            this.dockContainer1.RightPanelWidth = 150;
            this.dockContainer1.SelectToolWindowsOnHoover = false;
            this.dockContainer1.Size = new System.Drawing.Size(1003, 528);
            this.dockContainer1.TabButtonNotSelectedColor = System.Drawing.Color.DarkGray;
            this.dockContainer1.TabButtonSelectedBackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(242)))), ((int)(((byte)(200)))));
            this.dockContainer1.TabButtonSelectedBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(215)))), ((int)(((byte)(157)))));
            this.dockContainer1.TabButtonSelectedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(111)))));
            this.dockContainer1.TabButtonSelectedColor = System.Drawing.Color.Black;
            this.dockContainer1.TabButtonShowSelection = false;
            this.dockContainer1.TabIndex = 4;
            this.dockContainer1.TopPanelHeight = 150;
            // 
            // analizerToolStripMenuItem
            // 
            this.analizerToolStripMenuItem.Name = "analizerToolStripMenuItem";
            this.analizerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.analizerToolStripMenuItem.Text = "Analizer";
            this.analizerToolStripMenuItem.Click += new System.EventHandler(this.analizerToolStripMenuItem_Click);
            // 
            // EditorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 548);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.dockContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EditorWindow";
            this.Text = "EditorWindow";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSceneToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer RenderInterval;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renderModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deferredToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deferredDebugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputGPUTexturesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSceneToolStripMenuItem;
        private Crom.Controls.DockContainer dockContainer1;
        private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sceneViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hierarchyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem analizerToolStripMenuItem;
    }
}