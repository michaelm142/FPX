namespace FPX.Editor
{
    partial class VectorEditor
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
            this.Title = new System.Windows.Forms.GroupBox();
            this.posZBox = new System.Windows.Forms.TextBox();
            this.posYBox = new System.Windows.Forms.TextBox();
            this.posXBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Title.SuspendLayout();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Title.Controls.Add(this.posZBox);
            this.Title.Controls.Add(this.posYBox);
            this.Title.Controls.Add(this.posXBox);
            this.Title.Controls.Add(this.label3);
            this.Title.Controls.Add(this.label2);
            this.Title.Controls.Add(this.label1);
            this.Title.Location = new System.Drawing.Point(4, 4);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(418, 58);
            this.Title.TabIndex = 0;
            this.Title.TabStop = false;
            this.Title.Text = "Name";
            // 
            // posZBox
            // 
            this.posZBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.posZBox.Location = new System.Drawing.Point(309, 24);
            this.posZBox.Name = "posZBox";
            this.posZBox.Size = new System.Drawing.Size(100, 20);
            this.posZBox.TabIndex = 5;
            this.posZBox.Text = "0.0";
            this.posZBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.posZBox.TextChanged += new System.EventHandler(this.ValueChanged);
            // 
            // posYBox
            // 
            this.posYBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.posYBox.Location = new System.Drawing.Point(173, 24);
            this.posYBox.Name = "posYBox";
            this.posYBox.Size = new System.Drawing.Size(100, 20);
            this.posYBox.TabIndex = 4;
            this.posYBox.Text = "0.0";
            this.posYBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.posYBox.TextChanged += new System.EventHandler(this.ValueChanged);
            // 
            // posXBox
            // 
            this.posXBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.posXBox.Location = new System.Drawing.Point(37, 24);
            this.posXBox.Name = "posXBox";
            this.posXBox.Size = new System.Drawing.Size(100, 20);
            this.posXBox.TabIndex = 3;
            this.posXBox.Text = "0.0";
            this.posXBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.posXBox.TextChanged += new System.EventHandler(this.ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(289, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Z";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(153, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Y";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X";
            // 
            // VectorEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Title);
            this.Name = "VectorEditor";
            this.Size = new System.Drawing.Size(425, 68);
            this.Title.ResumeLayout(false);
            this.Title.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox Title;
        private System.Windows.Forms.TextBox posZBox;
        private System.Windows.Forms.TextBox posYBox;
        private System.Windows.Forms.TextBox posXBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
