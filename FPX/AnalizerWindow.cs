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
            Selection.OnSelectionChanged += AnalyzerGUI;
        }

        public void AnalyzerGUI(object sender, SelectionEventArgs e)
        {
            if (Selection.selectedObject == null)
                return;

            EditorGUI.Begin();

            foreach (Component c in Selection.selectedObject.Components)
            {
                var componentType = c.GetType();

                EditorGUI.BeginControl(c);
                foreach (var member in componentType.GetFields())
                {
                    var fieldType = member.FieldType;
                    if (fieldType == typeof(int))
                        EditorGUI.IntField(member.Name, (int)componentType.InvokeMember(member.Name, BindingFlags.GetField, null, c, new object[] { }));
                    if (fieldType == typeof(float))
                        EditorGUI.FloatField(member.Name, (float)componentType.InvokeMember(member.Name, BindingFlags.GetField, null, c, new object[] { }));
                    if (fieldType == typeof(string))
                        EditorGUI.StringField(member.Name, (string)componentType.InvokeMember(member.Name, BindingFlags.GetField, null, c, new object[] { }));
                }
                EditorGUI.EndControl();
            }

            EditorGUI.End();
        }

        private void AnalizerWindow_Resize(object sender, EventArgs e)
        {
            AnalyzerGUI(this, new SelectionEventArgs(Selection.SelectedObjects.ToArray()));
        }
    }
}
