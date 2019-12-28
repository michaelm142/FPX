using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using FPX.ComponentModel;
using System.IO;
using FPX.Editor;

using ComponentEditor = FPX.ComponentModel.ComponentEditor;
using EditorAttribute = FPX.ComponentModel.EditorAttribute;
using Component = FPX.ComponentModel.Component;
using Graphics = FPX.ComponentModel.Graphics;

namespace FPX
{
    public partial class EditorWindow : Form
    {
        public static EditorWindow Instance { get; private set; }

        public World simulation { get; set; }

        private SceneWindow sceneWindow;
        private HierarchyWindow hierarchyWindow;
        private AnalizerWindow analizerWindow;
        private IEnumerable<Crom.Controls.DockableToolWindow> LayoutWindows
        {
            get
            {
                for (int i = 0; i < 3; i++)
                {
                    switch (i)
                    {
                        case 0:
                            yield return sceneWindow;
                            break;
                        case 1:
                            yield return hierarchyWindow;
                            break;
                        case 2:
                            yield return analizerWindow;
                            break;
                    }
                }
            }
        }

        private GameView gameView1
        {
            get { return sceneWindow == null ? null : sceneWindow.gameView1; }
        }

        private ListBox heirarchyListBox
        {
            get { return hierarchyWindow == null ? null : hierarchyWindow.listBox1; }
        }

        static readonly string LayoutConfigFilename = "Layout.config";

        List<Control> inspectorWindowControls = new List<Control>();

        public EditorWindow(string sceneName = null)
        {
            Instance = this;
            InitializeComponent();
            EditorGUI.Initalize();
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

                sceneViewToolStripMenuItem.Checked = true;
                hierarchyToolStripMenuItem.Checked = true;
                analizerToolStripMenuItem.Checked = true;

                //hierarchyWindow.Show();
                //sceneWindow.Show();
                //analizerWindow.Show();

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
            Selection.Select(listbox.SelectedItem as GameObject);
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

        private void EditorWindow_Load(object sender, EventArgs e)
        {
            LoadConfig(Environment.CurrentDirectory + "//" + LayoutConfigFilename);
        }

        private void LoadConfig(string filename)
        {
            XmlDocument configDoc = new XmlDocument();
            using (FileStream file = new FileStream(filename, FileMode.OpenOrCreate))
                configDoc.Load(file);

            var configRoot = configDoc.SelectSingleNode("Config");

            foreach (var window in LayoutWindows)
            {
                var windowElement = configRoot.SelectSingleNode(window.Text);

                var sizeElement = windowElement.SelectSingleNode("Size");
                var locationElement = windowElement.SelectSingleNode("Location");
                var dockModeElement = windowElement.SelectSingleNode("DockMode");
                var widthAttribute = sizeElement.Attributes["Width"];
                var heightAttribute = sizeElement.Attributes["Height"];
                var visibleAttr = windowElement.Attributes["Visible"];
                var xAttr = locationElement.Attributes["X"];
                var yAttr = locationElement.Attributes["Y"];

                var dockModeValue = dockModeElement.InnerText;
                var dockMode = (Crom.Controls.zDockMode)Enum.Parse(typeof(Crom.Controls.zDockMode), dockModeValue);
                dockContainer1.DockToolWindow(window, dockMode);

                var width = int.Parse(widthAttribute.InnerText);
                window.Width = width;

                var height = int.Parse(heightAttribute.InnerText);
                window.Height = height;

                bool visible = bool.Parse(visibleAttr.InnerText);
                window.Visible = visible;

                var windowLocation = Point.Empty;
                windowLocation.X = int.Parse(xAttr.InnerText);
                windowLocation.Y = int.Parse(yAttr.InnerText);
                window.Location = windowLocation;

            }
        }

        private void SaveConfig(string filename)
        {
            XmlDocument configDoc = new XmlDocument();

            var configRoot = configDoc.CreateElement("Config");
            configDoc.AppendChild(configRoot);

            foreach (var window in LayoutWindows)
            {
                var windowElement = configDoc.CreateElement(window.Name);

                var sizeElement = configDoc.CreateElement("Size");
                var locationElement = configDoc.CreateElement("Location");
                var dockModeElement = configDoc.CreateElement("DockMode");
                var widthAttribute = configDoc.CreateAttribute("Width");
                var heightAttribute = configDoc.CreateAttribute("Height");
                var visibleAttr = configDoc.CreateAttribute("Visible");
                var xAttr = configDoc.CreateAttribute("X");
                var yAttr = configDoc.CreateAttribute("Y");

                dockModeElement.InnerText = window.DockMode.ToString();
                widthAttribute.InnerText = window.Width.ToString();
                heightAttribute.InnerText = window.Height.ToString();
                visibleAttr.InnerText = window.Visible.ToString();
                var windowLocation = window.Location;
                xAttr.InnerText = windowLocation.X.ToString();
                yAttr.InnerText = windowLocation.Y.ToString();

                sizeElement.Attributes.Append(widthAttribute);
                sizeElement.Attributes.Append(heightAttribute);

                locationElement.Attributes.Append(xAttr);
                locationElement.Attributes.Append(yAttr);

                windowElement.AppendChild(dockModeElement);
                windowElement.AppendChild(sizeElement);
                windowElement.AppendChild(locationElement);
                windowElement.Attributes.Append(visibleAttr);

                configRoot.AppendChild(windowElement);

            }

            using (FileStream file = new FileStream(filename, FileMode.OpenOrCreate | FileMode.Truncate))
                configDoc.Save(file);
        }

        private void EditorWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig(Environment.CurrentDirectory + "//" + LayoutConfigFilename);
        }
    }
}
