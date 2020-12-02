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

        private void gameView1_Resize(object sender, EventArgs e)
        {
            gameView1.simulation.GraphicsDevice.Reset();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ToolStripDropDown dropDown = new ToolStripDropDown();
            List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();
            foreach (var camera in GameObject.FindObjectsOfType<Camera>())
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = camera.Name;
                item.Text = camera.Name;
                item.Tag = camera;
                item.Click += CameraDropdownClick;
                items.Add(item);
            }
            dropDown.Items.AddRange(items.ToArray());

            dropDown.Show(this, Point.Empty);
        }

        private void CameraDropdownClick(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            Camera.Active = item.Tag as Camera;
        }
    }
}
