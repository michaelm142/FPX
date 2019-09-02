namespace ComponentModel
{
    partial class CameraEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.fieldOfViewTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nearPlaneTextBox = new System.Windows.Forms.TextBox();
            this.farPlaneLabel = new System.Windows.Forms.Label();
            this.farPlaneTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Field of View:";
            // 
            // fieldOfViewTextBox
            // 
            this.fieldOfViewTextBox.Location = new System.Drawing.Point(140, 17);
            this.fieldOfViewTextBox.Name = "fieldOfViewTextBox";
            this.fieldOfViewTextBox.Size = new System.Drawing.Size(100, 20);
            this.fieldOfViewTextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Near Plane:";
            // 
            // nearPlaneTextBox
            // 
            this.nearPlaneTextBox.Location = new System.Drawing.Point(140, 55);
            this.nearPlaneTextBox.Name = "nearPlaneTextBox";
            this.nearPlaneTextBox.Size = new System.Drawing.Size(100, 20);
            this.nearPlaneTextBox.TabIndex = 3;
            // 
            // farPlaneLabel
            // 
            this.farPlaneLabel.AutoSize = true;
            this.farPlaneLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.farPlaneLabel.Location = new System.Drawing.Point(5, 94);
            this.farPlaneLabel.Name = "farPlaneLabel";
            this.farPlaneLabel.Size = new System.Drawing.Size(102, 25);
            this.farPlaneLabel.TabIndex = 4;
            this.farPlaneLabel.Text = "Far Plane:";
            // 
            // farPlaneTextBox
            // 
            this.farPlaneTextBox.Location = new System.Drawing.Point(140, 100);
            this.farPlaneTextBox.Name = "farPlaneTextBox";
            this.farPlaneTextBox.Size = new System.Drawing.Size(100, 20);
            this.farPlaneTextBox.TabIndex = 5;
            // 
            // CameraEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.farPlaneTextBox);
            this.Controls.Add(this.farPlaneLabel);
            this.Controls.Add(this.nearPlaneTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.fieldOfViewTextBox);
            this.Controls.Add(this.label1);
            this.Name = "CameraEditor";
            this.Size = new System.Drawing.Size(498, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fieldOfViewTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox nearPlaneTextBox;
        private System.Windows.Forms.Label farPlaneLabel;
        private System.Windows.Forms.TextBox farPlaneTextBox;
    }
}
