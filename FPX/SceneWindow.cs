using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crom.Controls;

namespace FPX
{
    public partial class SceneWindow : DockableToolWindow
    {
        public SceneWindow()
        {
            InitializeComponent();
        }

        private void gameView1_MouseDown(object sender, MouseEventArgs e)
        {
            Focus();
        }
    }
}
