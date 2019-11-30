namespace FPX
{
    partial class SceneWindow
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
            this.gameView1 = new FPX.GameView();
            this.SuspendLayout();
            // 
            // gameView1
            // 
            this.gameView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameView1.Location = new System.Drawing.Point(0, 0);
            this.gameView1.Name = "gameView1";
            // this.gameView1.simulation = null;
            this.gameView1.Size = new System.Drawing.Size(800, 450);
            this.gameView1.TabIndex = 0;
            // 
            // SceneWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.gameView1);
            this.Name = "SceneWindow";
            this.Text = "SceneWindow";
            this.ResumeLayout(false);

        }

        #endregion

        public GameView gameView1;
    }
}