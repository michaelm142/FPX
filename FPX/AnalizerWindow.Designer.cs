namespace FPX
{
    partial class AnalizerWindow
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
            this.addComponentButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // addComponentButton
            // 
            this.addComponentButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addComponentButton.Location = new System.Drawing.Point(12, 12);
            this.addComponentButton.Name = "addComponentButton";
            this.addComponentButton.Size = new System.Drawing.Size(339, 23);
            this.addComponentButton.TabIndex = 0;
            this.addComponentButton.Text = "Add Component";
            this.addComponentButton.UseVisualStyleBackColor = true;
            // 
            // AnalizerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 609);
            this.Controls.Add(this.addComponentButton);
            this.Name = "AnalizerWindow";
            this.Text = "AnalizerWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addComponentButton;
    }
}