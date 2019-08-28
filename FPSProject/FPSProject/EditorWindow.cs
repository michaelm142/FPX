using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentModel;
using System.IO;

namespace FPSProject
{
    public partial class EditorWindow : Form
    {
        public EditorWindow(string sceneName = null)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(sceneName))
                gameView1.LoadSim(sceneName);
        }

        private void loadSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Scenes | *.xml";
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
                gameView1.LoadSim(Path.GetFileNameWithoutExtension(dialog.FileName));
        }

        private void EditorWindow_Load(object sender, EventArgs e)
        {
        }
    }
}
