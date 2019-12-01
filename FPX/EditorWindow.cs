using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentModel;
using System.IO;

using ComponentEditor = ComponentModel.ComponentEditor;
using EditorAttribute = ComponentModel.EditorAttribute;
using Component = ComponentModel.Component;
using Graphics = ComponentModel.Graphics;

namespace FPX
{
    public partial class EditorWindow : Form
    {
        public static EditorWindow Instance { get; private set; }

        public World simulation { get; set; }

        private SceneWindow sceneWindow;
        private HierarchyWindow hierarchyWindow;
        private AnalizerWindow analizerWindow;

        private GameView gameView1
        {
            get { return sceneWindow == null ? null : sceneWindow.gameView1; }
        }

        private ListBox heirarchyListBox
        {
            get { return hierarchyWindow == null ? null : hierarchyWindow.listBox1; }
        }

        List<Control> inspectorWindowControls = new List<Control>();

        public EditorWindow(string sceneName = null)
        {
            Instance = this;
            InitializeComponent();
            if (!DesignMode)
            {
                sceneWindow = new SceneWindow();
                hierarchyWindow = new HierarchyWindow();
                analizerWindow = new AnalizerWindow();

                sceneWindow.FormClosing += DockableWindowClosing;
                hierarchyWindow.FormClosing += DockableWindowClosing;
                analizerWindow.FormClosing += DockableWindowClosing;
                heirarchyListBox.SelectedIndexChanged += listBox1_SelectedIndexChanged;
                gameView1.MouseDown += gameView1_MouseDown;

                hierarchyWindow.Show();
                sceneWindow.Show();
                analizerWindow.Show();

                dockContainer1.AddToolWindow(sceneWindow);
                dockContainer1.AddToolWindow(hierarchyWindow);
                dockContainer1.AddToolWindow(analizerWindow);
            }

            gameView1.SceneObjectInstanciated += GameView1_SceneObjectInstanciated;
            if (!string.IsNullOrEmpty(sceneName))
                gameView1.LoadSim(sceneName);


        }

        private void DockableWindowClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Form f = sender as Form;
            f.Hide();
        }

        private void GameView1_SceneObjectInstanciated(object sender, EventArgs e)
        {
            heirarchyListBox.Items.Add(sender as GameObject);
        }

        private int GetHeirarchyLevel(GameObject obj, int level = 0)
        {
            if (obj.transform.parent == null)
                return level;

            return GetHeirarchyLevel(obj.transform.parent.gameObject, level + 1);
        }

        private void loadSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Scenes | *.xml";
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
                gameView1.LoadSim(dialog.FileName);
        }

        private void InputUpdate(object sender, EventArgs e)
        {
            if (sceneWindow != null)
                gameView1.InputUpdate();
        }

        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Graphics.Mode = "Default";
            defaultToolStripMenuItem.Checked = true;
            deferredToolStripMenuItem.Checked = false;
            deferredDebugToolStripMenuItem.Checked = false;

        }

        private void deferredToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Graphics.Mode = "Deferred";
            defaultToolStripMenuItem.Checked = false;
            deferredToolStripMenuItem.Checked = true;
            deferredDebugToolStripMenuItem.Checked = false;
        }

        private void deferredDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Graphics.Mode = "DeferredDebug";
            deferredDebugToolStripMenuItem.Checked = true;
            defaultToolStripMenuItem.Checked = false;
            deferredToolStripMenuItem.Checked = false;
        }

        private void outputGPUTexturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Graphics.instance.renderer._debug_OutuptGBuffers();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listbox = sender as ListBox;
            analizerWindow.SelectedChanged(listbox.SelectedItem as GameObject);
        }

        private void gameView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                heirarchyListBox.SelectedIndex = -1;
        }

        private void saveSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Scene Files | *.xml";

            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
                Scene.Active.Save(dialog.FileName);
        }

        private void sceneViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sceneWindow.Visible)
                sceneWindow.Hide();
            else
                sceneWindow.Show();

            sceneViewToolStripMenuItem.Checked = sceneWindow.Visible;
        }

        private void hierarchyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hierarchyWindow.Visible)
                hierarchyWindow.Hide();
            else
                hierarchyWindow.Show();

            hierarchyToolStripMenuItem.Checked = hierarchyWindow.Visible;
        }

        private void analizerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (analizerWindow.Visible)
                analizerWindow.Hide();
            else
                analizerWindow.Show();

            analizerToolStripMenuItem.Checked = analizerWindow.Visible;
        }
    }
}
