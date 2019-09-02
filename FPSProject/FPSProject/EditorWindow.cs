﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace FPSProject
{
    public partial class EditorWindow : Form
    {
        private Point AddComponentButtonStartLocation;

        List<Control> inspectorWindowControls = new List<Control>();

        public EditorWindow(string sceneName = null)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(sceneName))
                gameView1.LoadSim(sceneName);

            gameView1.SceneObjectInstanciated += GameView1_SceneObjectInstanciated;
            AddComponentButtonStartLocation = addComponentButton.Location;
        }

        private void GameView1_SceneObjectInstanciated(object sender, EventArgs e)
        {
            listBox1.Items.Add(sender);
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
            gameView1.InputUpdate();
        }

        private void EditorWindow_Load(object sender, EventArgs e)
        {
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

        public void AddComponent(Component c)
        {
            var attributes = c.GetType().GetCustomAttributes(true).ToList();
            var editorAttr = attributes.Find(a => a is EditorAttribute) as EditorAttribute;

            if (editorAttr == null)
                return;

            
            ComponentEditor editor = Activator.CreateInstance(editorAttr.EditorType, new object[] { c }) as ComponentEditor;
            editor.UpdateTarget();
            editor.Width = InspectorWindow.Width;
            editor.BorderStyle = BorderStyle.FixedSingle;
            InspectorWindow.Controls.Add(editor);

            Point componentButtonLocation = addComponentButton.Location;
            componentButtonLocation.Y += editor.Height;
            addComponentButton.Location = componentButtonLocation;

            Point editorLocation = editor.Location;
            for (int i = 0; i < inspectorWindowControls.Count; i++)
            {
                editorLocation.Y += inspectorWindowControls[i].Height;
            }
            editor.Location = editorLocation;
            inspectorWindowControls.Add(editor);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var c in inspectorWindowControls)
                InspectorWindow.Controls.Remove(c);
            inspectorWindowControls.Clear();
            addComponentButton.Location = AddComponentButtonStartLocation;

            if (listBox1.SelectedItem == null)
            {
                addComponentButton.Location = AddComponentButtonStartLocation;
                return;
            }

            GameObject obj = listBox1.SelectedItem as GameObject;
            foreach (var comp in obj.Components)
                AddComponent(comp);
        }

        private void gameView1_Enter(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = -1;
        }

        private void gameView1_MouseDown(object sender, MouseEventArgs e)
        {
            listBox1.SelectedIndex = -1;
        }

        private void outputButtonLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.Log("Button Location: {0}", addComponentButton.Location);
        }

        private void invalidateButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addComponentButton.Invalidate();
            addComponentButton.BringToFront();
        }

        private void saveSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Scene Files | *.xml";

            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
                Scene.Active.Save(dialog.FileName);
        }
    }
}
