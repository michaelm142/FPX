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
using FPX;
using FPX.Editor;

using ComponentEditor = FPX.ComponentEditor;
using EditorAttribute = FPX.EditorAttribute;
using Component = FPX.Component;

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

            EditorGUI.Begin(Selection.selectedObject);

            foreach (Component c in Selection.selectedObject.Components)
            {
                var componentType = c.GetType();

                EditorGUI.BeginControl(c);
                var analyzerMethod = componentType.GetMethod("OnAnalyzerGUI");
                if (analyzerMethod != null)
                    analyzerMethod.Invoke(c, new object[] { });
                else
                {
                    foreach (var member in componentType.GetFields())
                    {
                        if (member.GetCustomAttribute(typeof(IgnoreInGUIAttribute)) != null)
                            continue;

                        var fieldType = member.FieldType;
                        if (fieldType == typeof(int))
                            EditorGUI.IntField(member.Name, (int)componentType.InvokeMember(member.Name, BindingFlags.GetField, null, c, new object[] { }));
                        if (fieldType == typeof(float))
                            EditorGUI.FloatField(member.Name, (float)componentType.InvokeMember(member.Name, BindingFlags.GetField, null, c, new object[] { }));
                        if (fieldType == typeof(string))
                            EditorGUI.StringField(member.Name, (string)componentType.InvokeMember(member.Name, BindingFlags.GetField, null, c, new object[] { }));
                        if (fieldType == typeof(Microsoft.Xna.Framework.Vector3))
                            EditorGUI.Vector3Field(member.Name, (Microsoft.Xna.Framework.Vector3)componentType.InvokeMember(member.Name, BindingFlags.GetField, null, c, new object[] { }));
                        if (fieldType == typeof(Microsoft.Xna.Framework.Quaternion))
                            EditorGUI.QuaternionField(member.Name, (Microsoft.Xna.Framework.Quaternion)componentType.InvokeMember(member.Name, BindingFlags.GetField, null, c, new object[] { }));
                        if (fieldType == typeof(Microsoft.Xna.Framework.Color))
                            EditorGUI.ColorField(member.Name, (Microsoft.Xna.Framework.Color)componentType.InvokeMember(member.Name, BindingFlags.GetField, null, c, new object[] { }));
                        if (fieldType == typeof(Enum))
                            EditorGUI.EnumField(member.Name, (Enum)componentType.InvokeMember(member.Name, BindingFlags.GetField, null, c, new object[] { }));
                        if (fieldType == typeof(Microsoft.Xna.Framework.Graphics.Model) || fieldType == typeof(Microsoft.Xna.Framework.Audio.SoundEffect))
                            EditorGUI.ObjectField(member.Name, componentType.InvokeMember(member.Name, BindingFlags.GetField, null, c, new object[] { }));
                        if (fieldType == typeof(Microsoft.Xna.Framework.Graphics.Texture2D))
                            EditorGUI.TextureField(member.Name, (Microsoft.Xna.Framework.Graphics.Texture2D)componentType.InvokeMember(member.Name, BindingFlags.GetField, null, c, new object[] { }));
                    }
                    foreach (var member in componentType.GetProperties())
                    {
                        if (member.GetCustomAttribute(typeof(IgnoreInGUIAttribute)) != null)
                            continue;

                        var fieldType = member.PropertyType;
                        if (member.GetSetMethod() == null)
                            continue;
                        if (fieldType == typeof(int))
                            EditorGUI.IntField(member.Name, (int)componentType.InvokeMember(member.Name, BindingFlags.GetProperty, null, c, new object[] { }));
                        if (fieldType == typeof(float))
                            EditorGUI.FloatField(member.Name, (float)componentType.InvokeMember(member.Name, BindingFlags.GetProperty, null, c, new object[] { }));
                        if (fieldType == typeof(string))
                            EditorGUI.StringField(member.Name, (string)componentType.InvokeMember(member.Name, BindingFlags.GetProperty, null, c, new object[] { }));
                        if (fieldType == typeof(Microsoft.Xna.Framework.Vector3))
                            EditorGUI.Vector3Field(member.Name, (Microsoft.Xna.Framework.Vector3)componentType.InvokeMember(member.Name, BindingFlags.GetProperty, null, c, new object[] { }));
                        if (fieldType == typeof(Microsoft.Xna.Framework.Quaternion))
                            EditorGUI.QuaternionField(member.Name, (Microsoft.Xna.Framework.Quaternion)componentType.InvokeMember(member.Name, BindingFlags.GetProperty, null, c, new object[] { }));
                        if (fieldType == typeof(Microsoft.Xna.Framework.Color))
                            EditorGUI.ColorField(member.Name, (Microsoft.Xna.Framework.Color)componentType.InvokeMember(member.Name, BindingFlags.GetProperty, null, c, new object[] { }));
                        if (fieldType == typeof(Enum))
                            EditorGUI.EnumField(member.Name, (Enum)componentType.InvokeMember(member.Name, BindingFlags.GetProperty, null, c, new object[] { }));
                        if (fieldType == typeof(Microsoft.Xna.Framework.Graphics.Model) || fieldType == typeof(Microsoft.Xna.Framework.Audio.SoundEffect) || fieldType == typeof(Microsoft.Xna.Framework.Graphics.Texture2D))
                            EditorGUI.ObjectField(member.Name, componentType.InvokeMember(member.Name, BindingFlags.GetField, null, c, new object[] { }));
                        if (fieldType == typeof(Microsoft.Xna.Framework.Graphics.Texture2D))
                            EditorGUI.TextureField(member.Name, (Microsoft.Xna.Framework.Graphics.Texture2D)componentType.InvokeMember(member.Name, BindingFlags.GetField, null, c, new object[] { }));
                    }
                }
                EditorGUI.EndControl();
            }

            EditorGUI.End();
        }

        private void AnalizerWindow_Resize(object sender, EventArgs e)
        {
            AnalyzerGUI(this, new SelectionEventArgs(Selection.SelectedObjects.ToArray()));
        }

        private void visibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var obj in Selection.SelectedObjects)
                obj.Visible = visibleCheckBox.Checked;
        }

        private void enabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var obj in Selection.SelectedObjects)
                obj.Enabled = enabledCheckBox.Checked;
        }
    }
}
