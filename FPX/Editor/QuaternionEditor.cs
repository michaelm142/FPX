using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using ComponentModel;

namespace FPX.Editor
{
    public partial class QuaternionEditor : UserControl
    {
        public float xValue
        {
            get
            {
                float val = 0.0f;
                if (float.TryParse(posXBox.Text, out val))
                    return val;

                return 0.0f;
            }

            set
            {
                posXBox.Text = Math.Round(value, 2).ToString();
            }
        }

        public float yValue
        {
            get
            {
                float val = 0.0f;
                if (float.TryParse(posYBox.Text, out val))
                    return val;

                return 0.0f;
            }

            set
            {
                posYBox.Text = Math.Round(value, 2).ToString();
            }
        }

        public float zValue
        {
            get
            {
                float val = 0.0f;
                if (float.TryParse(posZBox.Text, out val))
                    return val;

                return 0.0f;
            }

            set
            {
                posZBox.Text = Math.Round(value, 2).ToString();
            }
        }

        private Vector3 vector
        {
            get { return new Vector3(xValue, yValue, zValue); }

            set
            {
                xValue = value.X;
                yValue = value.Y;
                zValue = value.Z;
            }
        }

        public Quaternion Value
        {
            get { return LinearAlgebraUtil.QuaternionFromEuler(vector); }

            set { vector = value.GetEulerAngles(); }
        }

        public string VectorName
        {
            get { return Title.Text; }

            set { Title.Text = value; }
        }

        private bool isProperty;

        private string memberName;
        private GroupBox Title;
        private TextBox posZBox;
        private TextBox posYBox;
        private TextBox posXBox;
        private Label label3;
        private Label label2;
        private Label label1;
        private ComponentModel.Component component;

        private QuaternionEditor()
        {
            InitializeComponent();
        }

        public QuaternionEditor(ComponentModel.Component component, string memberName)
        {
            InitializeComponent();

            this.component = component;
            this.memberName = memberName;
            if (component.GetType().GetFields().ToList().Find(f => f.Name == memberName) == null)
            {
                if (component.GetType().GetProperties().ToList().Find(p => p.Name == memberName) != null)
                    isProperty = true;
                else
                    throw new InvalidOperationException(string.Format("Component {0} does not contain a field or property named {1}", component.GetType().ToString(), memberName));
            }
        }

        private void ValueChanged(object sender, EventArgs e)
        {

            if (isProperty)
                component.GetType().GetProperty(memberName).GetSetMethod().Invoke(component, new object[] { Value });
            else
                component.GetType().GetField(memberName).SetValue(component, Value);
        }

        public static int DefaultLayoutHeight
        {
            get
            {
                QuaternionEditor v = new QuaternionEditor();
                return v.Height;
            }
        }

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
            this.Title.Location = new System.Drawing.Point(0, 0);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(425, 50);
            this.Title.TabIndex = 1;
            this.Title.TabStop = false;
            this.Title.Text = "Name";
            // 
            // posZBox
            // 
            this.posZBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.posZBox.Location = new System.Drawing.Point(316, 20);
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
            this.posYBox.Location = new System.Drawing.Point(176, 24);
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
            this.posXBox.Location = new System.Drawing.Point(37, 20);
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
            this.label3.Location = new System.Drawing.Point(296, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Z";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(156, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Y";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X";
            // 
            // QuaternionEditor
            // 
            this.Controls.Add(this.Title);
            this.Name = "QuaternionEditor";
            this.Size = new System.Drawing.Size(428, 50);
            this.Title.ResumeLayout(false);
            this.Title.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
