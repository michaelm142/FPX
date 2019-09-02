namespace FPSProject
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
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renderModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deferredToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deferredDebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputGPUTexturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputButtonLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invalidateButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.RenderInterval = new System.Windows.Forms.Timer(this.components);
            this.InspectorWindow = new System.Windows.Forms.Panel();
            this.addComponentButton = new System.Windows.Forms.Button();
            this.gameView1 = new FPSProject.GameView();
            this.saveSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.InspectorWindow.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.debugToolStripMenuItem});
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
            this.loadSceneToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.loadSceneToolStripMenuItem.Text = "Load Scene";
            this.loadSceneToolStripMenuItem.Click += new System.EventHandler(this.loadSceneToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
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
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.outputButtonLocationToolStripMenuItem,
            this.invalidateButtonToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.debugToolStripMenuItem.Text = "Debug";
            // 
            // outputButtonLocationToolStripMenuItem
            // 
            this.outputButtonLocationToolStripMenuItem.Name = "outputButtonLocationToolStripMenuItem";
            this.outputButtonLocationToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.outputButtonLocationToolStripMenuItem.Text = "Output Button Location";
            this.outputButtonLocationToolStripMenuItem.Click += new System.EventHandler(this.outputButtonLocationToolStripMenuItem_Click);
            // 
            // invalidateButtonToolStripMenuItem
            // 
            this.invalidateButtonToolStripMenuItem.Name = "invalidateButtonToolStripMenuItem";
            this.invalidateButtonToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.invalidateButtonToolStripMenuItem.Text = "Invalidate Button";
            this.invalidateButtonToolStripMenuItem.Click += new System.EventHandler(this.invalidateButtonToolStripMenuItem_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 30);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(133, 433);
            this.listBox1.TabIndex = 1;
            this.listBox1.TabStop = false;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
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
            // InspectorWindow
            // 
            this.InspectorWindow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InspectorWindow.AutoScroll = true;
            this.InspectorWindow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.InspectorWindow.Controls.Add(this.addComponentButton);
            this.InspectorWindow.Location = new System.Drawing.Point(531, 30);
            this.InspectorWindow.Name = "InspectorWindow";
            this.InspectorWindow.Size = new System.Drawing.Size(448, 433);
            this.InspectorWindow.TabIndex = 3;
            // 
            // addComponentButton
            // 
            this.addComponentButton.Location = new System.Drawing.Point(162, 3);
            this.addComponentButton.Name = "addComponentButton";
            this.addComponentButton.Size = new System.Drawing.Size(151, 23);
            this.addComponentButton.TabIndex = 0;
            this.addComponentButton.Text = "Add Component";
            this.addComponentButton.UseVisualStyleBackColor = true;
            // 
            // gameView1
            // 
            this.gameView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gameView1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.gameView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gameView1.Location = new System.Drawing.Point(151, 30);
            this.gameView1.Name = "gameView1";
            this.gameView1.Size = new System.Drawing.Size(374, 433);
            this.gameView1.TabIndex = 2;
            this.gameView1.Enter += new System.EventHandler(this.gameView1_Enter);
            this.gameView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gameView1_MouseDown);
            // 
            // saveSceneToolStripMenuItem
            // 
            this.saveSceneToolStripMenuItem.Name = "saveSceneToolStripMenuItem";
            this.saveSceneToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveSceneToolStripMenuItem.Text = "Save Scene";
            this.saveSceneToolStripMenuItem.Click += new System.EventHandler(this.saveSceneToolStripMenuItem_Click);
            // 
            // EditorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 472);
            this.Controls.Add(this.InspectorWindow);
            this.Controls.Add(this.gameView1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EditorWindow";
            this.Text = "EditorWindow";
            this.Load += new System.EventHandler(this.EditorWindow_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.InspectorWindow.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSceneToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ImageList imageList1;
        private GameView gameView1;
        private System.Windows.Forms.Timer RenderInterval;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renderModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deferredToolStripMenuItem;
        private System.Windows.Forms.Panel InspectorWindow;
        private System.Windows.Forms.Button addComponentButton;
        private System.Windows.Forms.ToolStripMenuItem deferredDebugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputGPUTexturesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputButtonLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invalidateButtonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSceneToolStripMenuItem;
    }
}