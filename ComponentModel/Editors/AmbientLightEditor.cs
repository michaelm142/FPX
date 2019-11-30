using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComponentModel
{
    public partial class AmbientLightEditor : ComponentEditor
    {
        public AmbientLightEditor(AmbientLight Target)
        {
            InitializeComponent();
            this.Target = Target;
            colorPicker1.XnaColor = Target.DiffuseColor;
            colorPicker1.ColorChanged += ColorPicker1_ColorChanged;
        }

        private void ColorPicker1_ColorChanged(object sender, EventArgs e)
        {
            (Target as AmbientLight).DiffuseColor = colorPicker1.XnaColor;
        }
    }
}
