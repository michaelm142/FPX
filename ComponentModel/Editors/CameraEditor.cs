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
    public partial class CameraEditor : ComponentEditor
    {
        public CameraEditor(Camera target)
        {
            InitializeComponent();
            Target = target;
        }

    }
}
