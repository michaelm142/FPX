using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;    
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crom.Controls;
using ComponentModel;
using FPX.Editor;

using ComponentEditor = ComponentModel.ComponentEditor;
using EditorAttribute = ComponentModel.EditorAttribute;
using Component = ComponentModel.Component;

namespace FPX
{
    public partial class AnalizerWindow : DockableToolWindow
    {
        private List<Control> componentControls = new List<Control>();

        private Point AddComponentButtonStartLocation;

        public AnalizerWindow()
        {
            InitializeComponent();
            AddComponentButtonStartLocation = addComponentButton.Location;
            EditorGUI.TargetControl = this;
        }

        public void AddComponent(Component c)
        {
            var attributes = c.GetType().GetCustomAttributes(true).ToList();
            var editorAttr = attributes.Find(a => a is EditorAttribute) as EditorAttribute;

            if (editorAttr == null)
                return;


            ComponentEditor editor = Activator.CreateInstance(editorAttr.EditorType, new object[] { c }) as ComponentEditor;
            editor.UpdateTarget();
            editor.Width = Width;
            editor.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(editor);

            Point componentButtonLocation = addComponentButton.Location;
            componentButtonLocation.Y += editor.Height;
            addComponentButton.Location = componentButtonLocation;

            Point editorLocation = editor.Location;
            for (int i = 0; i < componentControls.Count; i++)
            {
                editorLocation.Y += componentControls[i].Height;
            }
            editor.Location = editorLocation;
            componentControls.Add(editor);
        }

        public void SelectedChanged(GameObject selectedObject)
        {
            EditorGUI.Begin();

            EditorGUI.End();
        }
    }
}
