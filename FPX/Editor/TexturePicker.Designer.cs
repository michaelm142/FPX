namespace FPX.Editor
{
    partial class TexturePicker
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
            this.textureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.textureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // textureBox
            // 
            this.textureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textureBox.Location = new System.Drawing.Point(80, 0);
            this.textureBox.Name = "textureBox";
            this.textureBox.Size = new System.Drawing.Size(118, 56);
            this.textureBox.TabIndex = 0;
            this.textureBox.TabStop = false;
            this.textureBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.textureBox_DragDrop);
            this.textureBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.textureBox_DragEnter);
            // 
            // TexturePicker
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textureBox);
            this.Name = "TexturePicker";
            this.Size = new System.Drawing.Size(198, 56);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.textureBox_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.textureBox_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.textureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox textureBox;
    }
}
