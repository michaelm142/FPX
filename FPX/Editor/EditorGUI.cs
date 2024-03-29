﻿using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using ColorPicker = FPX.ColorPicker;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace FPX.Editor
{
    public static partial class EditorGUI
    {
        private static Control _targetControl;
        public static Control TargetControl
        {
            get { return _targetControl; }

            set
            {
                if (_targetControl == null)
                {
                    TitleControl = value.Controls.Find("titlePanel", true)[0];
                    AddComponentButton = value.Controls.Find("addComponentButton", true)[0] as Button;
                }

                _targetControl = value;
            }
        }

        private static Button AddComponentButton;
        private static Control TitleControl;

        private static List<GUIValue> Values = new List<GUIValue>();

        private const int ControlHeight = 30;

        public static void Initalize()
        {
            Selection.OnSelectionChanged += Selection_OnSelectionChanged;
        }

        private static void Selection_OnSelectionChanged(object sender, SelectionEventArgs e)
        {
            Values.Clear();
        }

        public static void Begin(GameObject gameObject)
        {
            UpdateValues();

            TargetControl.Controls.Clear();
            TargetControl.Controls.Add(TitleControl);

            CheckBox visibleCheckBox = TitleControl.Controls.Find("visibleCheckBox", true)[0] as CheckBox;
            CheckBox enabledCheckBox = TitleControl.Controls.Find("enabledCheckBox", true)[0] as CheckBox;
            TextBox nameTextBox = TitleControl.Controls.Find("nameTextBox", true)[0] as TextBox;

            visibleCheckBox.Checked = gameObject.Visible;
            enabledCheckBox.Checked = gameObject.Enabled;
            nameTextBox.Text = gameObject.Name;

            visibleCheckBox.Tag = gameObject;
            enabledCheckBox.Tag = gameObject;
            nameTextBox.Tag = gameObject;

            visibleCheckBox.CheckedChanged += VisibleCheckBox_CheckedChanged;
            enabledCheckBox.CheckedChanged += EnabledCheckBox_CheckedChanged;
            nameTextBox.TextChanged += NameTextBox_TextChanged;
        }

        private static void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (Selection.selectedObject == null) return;

            TextBox nameBox = sender as TextBox;
            Selection.selectedObject.Name = nameBox.Text;
        }

        private static void EnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Selection.selectedObject == null) return;

            CheckBox enabledbox = sender as CheckBox;
            Selection.selectedObject.Enabled = enabledbox.Checked;
        }

        private static void VisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Selection.selectedObject == null) return;

            CheckBox checkbox = sender as CheckBox;
            Selection.selectedObject.Visible = checkbox.Checked;
        }

        public static void End()
        {
            int controlHeight = 0;
            foreach (Control c in TargetControl.Controls)
                controlHeight += c.Height;

            var buttonLocation = AddComponentButton.Location;
            buttonLocation.Y = controlHeight;
            AddComponentButton.Location = buttonLocation;

            TargetControl.Controls.Add(AddComponentButton);
            TargetControl.Invalidate(true);
        }

        public static void BeginControl(FPX.Component component)
        {
            if (TargetControl is Panel)
                throw new InvalidOperationException("EndControl must be called before BeginControl");

            var componentType = component.GetType();
            string[] componentTypeInfo = component.GetType().ToString().Split(new char[] { '.' });
            string componentName = componentTypeInfo[componentTypeInfo.Length - 1];


            int panelPos = 0;
            foreach (Control c in TargetControl.Controls)
            {
                if (c is Panel)
                {
                    var panel = c as Panel;
                    panelPos += panel.Height;
                }
            }

            Panel p = new Panel();
            p.Location = new Point(0, panelPos);
            p.Name = "panel1";
            p.Size = new Size(TargetControl.Width - 20, 100);
            p.TabIndex = 5;
            p.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            p.BorderStyle = BorderStyle.Fixed3D;
            p.Tag = component;

            Label nameLabel = new Label();
            nameLabel.Location = new Point(0, 0);
            nameLabel.Text = componentName;
            nameLabel.Name = "nameLabel";
            nameLabel.AutoSize = true;

            p.Controls.Add(nameLabel);

            TargetControl.Controls.Add(p);
            TargetControl = p;
        }

        public static void EndControl()
        {
            int controlHeight = 30;
            foreach (var value in Values)
            {
                if (value.valueType == ValueType.Vector3)
                    controlHeight += VectorEditor.DefaultLayoutHeight;
                else if (value.valueType == ValueType.Quaternion)
                    controlHeight += QuaternionEditor.DefaultLayoutHeight;
                else
                    controlHeight += ControlHeight;
            }
            TargetControl.Height = controlHeight;

            var activeComponent = TargetControl.Tag as FPX.Component;

            for (int i = 0, y = 30; i < Values.Count; i++)
            {
                var value = Values[i];

                if (value.valueType == ValueType.Vector3)
                {
                    VectorEditor editor = new VectorEditor(activeComponent, value.Label);
                    editor.Value = (Vector3)value.Data;
                    editor.Location = new Point(0, y);
                    editor.Size = new Size(TargetControl.Width, editor.Size.Height);
                    editor.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                    editor.VectorName = value.Label;
                    editor.Tag = value;

                    y += editor.Height;

                    TargetControl.Controls.Add(editor);
                }
                else if (value.valueType == ValueType.Quaternion)
                {
                    QuaternionEditor editor = new QuaternionEditor(activeComponent, value.Label);
                    editor.Value = (Quaternion)value.Data;
                    editor.Location = new Point(0, y);
                    editor.Size = new Size(TargetControl.Width, editor.Size.Height);
                    editor.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                    editor.VectorName = value.Label;
                    editor.Tag = value;

                    y += editor.Height;

                    TargetControl.Controls.Add(editor);
                }
                else if (value.valueType == ValueType.Color)
                {
                    Label label = new Label();
                    label.AutoSize = true;
                    label.Location = new Point(0, y);
                    label.Name = "GUI Label";
                    label.Text = value.Label;

                    ColorPicker colorBox = new ColorPicker();
                    colorBox.Location = new Point(TargetControl.Width / 2, y);
                    colorBox.Name = "GUI Value Box";
                    colorBox.Size = new Size(TargetControl.Width / 2, ControlHeight);
                    colorBox.TabIndex = 1;
                    colorBox.Text = value.Data.ToString();
                    colorBox.XnaColor = (XnaColor)value.Data;
                    colorBox.ColorChanged += ColorBox_ColorChanged;
                    colorBox.Tag = value;
                    y += ControlHeight;

                    TargetControl.Controls.Add(label);
                    TargetControl.Controls.Add(colorBox);
                }
                else if (value.valueType == ValueType.Enum)
                {
                    Label label = new Label();
                    label.AutoSize = true;
                    label.Location = new Point(0, y);
                    label.Name = "GUI Label";
                    label.Text = value.Label;


                    ComboBox enumComboBox = new ComboBox();
                    enumComboBox.Location = new Point(TargetControl.Width / 2, y);
                    enumComboBox.Name = "GUI Value Box";
                    enumComboBox.Size = new Size(TargetControl.Width / 2, ControlHeight);
                    enumComboBox.TabIndex = 1;
                    enumComboBox.Text = value.Data.ToString();
                    enumComboBox.Tag = value;
                    enumComboBox.Items.AddRange(Enum.GetValues(value.Data.GetType()) as object[]);
                    enumComboBox.SelectedValueChanged += EnumComboBox_SelectedValueChanged;
                    y += ControlHeight;

                    TargetControl.Controls.Add(label);
                    TargetControl.Controls.Add(enumComboBox);
                }
                else if (value.valueType == ValueType.Object)
                {
                    Label label = new Label();
                    label.AutoSize = true;
                    label.Location = new Point(0, y);
                    label.Name = "GUI Label";
                    label.Text = value.Label;


                    ObjectDropField dropField = new ObjectDropField();
                    dropField.Location = new Point(TargetControl.Width / 2, y);
                    dropField.Name = "GUI Value Box";
                    dropField.Size = new Size(TargetControl.Width / 2, ControlHeight);
                    dropField.TabIndex = 1;
                    dropField.Value = value.Data;
                    dropField.Tag = value;
                    y += ControlHeight;

                    TargetControl.Controls.Add(label);
                    TargetControl.Controls.Add(dropField);
                }
                else if (value.valueType == ValueType.Texture)
                {
                    Label label = new Label();
                    label.AutoSize = true;
                    label.Location = new Point(0, y);
                    label.Name = "GUI Label";
                    label.Text = value.Label;


                    TexturePicker picker = new TexturePicker();
                    picker.Location = new Point(TargetControl.Width / 2, y);
                    picker.Name = "GUI Value Box";
                    picker.Size = new Size(TargetControl.Width / 2, ControlHeight);
                    picker.TabIndex = 1;
                    picker.Texture = value.Data as Microsoft.Xna.Framework.Graphics.Texture2D;
                    picker.Tag = value;
                    picker.OnTextureChanged += Picker_OnTextureChanged;
                    y += ControlHeight;

                    TargetControl.Controls.Add(label);
                    TargetControl.Controls.Add(picker);
                }
                else
                {
                    Label label = new Label();
                    label.AutoSize = true;
                    label.Location = new Point(0, y);
                    label.Name = "GUI Label";
                    label.Text = value.Label;

                    TextBox valueBox = new TextBox();
                    valueBox.Location = new Point(TargetControl.Width / 2, y);
                    valueBox.Name = "GUI Value Box";
                    valueBox.Size = new Size(TargetControl.Width / 2, ControlHeight);
                    valueBox.TabIndex = 1;
                    valueBox.Text = value.Data.ToString();
                    valueBox.TextChanged += ValueBox_TextChanged;
                    valueBox.Tag = value;
                    y += ControlHeight;

                    TargetControl.Controls.Add(label);
                    TargetControl.Controls.Add(valueBox);
                }
            }

            Values = new List<GUIValue>();
            TargetControl.Invalidate(true);

            TargetControl = TargetControl.Parent;
        }

        private static void Picker_OnTextureChanged(object sender, EventArgs e)
        {
            TexturePicker picker = sender as TexturePicker;
            GUIValue value = picker.Tag as GUIValue;

            value.Data = picker.Texture;

            UpdateValues();
        }

        private static void EnumComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox box = sender as ComboBox;
            GUIValue value = box.Tag as GUIValue;

            value.Data = Enum.Parse(value.Data.GetType(), box.Text);

            UpdateValues();
        }

        private static void ColorBox_ColorChanged(object sender, EventArgs e)
        {
            ColorPicker picker = sender as ColorPicker;
            GUIValue value = picker.Tag as GUIValue;

            value.Data = picker.XnaColor;

            UpdateValues();
        }

        private static void UpdateValues()
        {
            foreach (Control c in TargetControl.Controls)
            {
                if (c.Tag == null)
                    continue;

                var comp = c.Tag as FPX.Component;
                var componentType = comp.GetType();
                if (c is Panel)
                {
                    foreach (Control pannelControl in c.Controls)
                    {
                        if (pannelControl is TextBox || pannelControl is ColorPicker || pannelControl is VectorEditor || pannelControl is QuaternionEditor || pannelControl is TexturePicker)
                        {
                            GUIValue value = pannelControl.Tag as GUIValue;
                            var prop = componentType.GetProperties().ToList().Find(property => property.Name == value.Label);
                            if (prop != null)
                                prop.GetSetMethod().Invoke(comp, new object[] { value.Data });
                            if (componentType.GetFields().ToList().Find(field => field.Name == value.Label) != null)
                                componentType.InvokeMember(value.Label, BindingFlags.SetField, null, comp, new object[] { value.Data });
                        }
                    }
                }
            }

            HierarchyWindow.instance.treeView1.Update();
        }

        public enum ValueType
        {
            Integer,
            Float,
            String,
            Vector3,
            Quaternion,
            Color,
            Enum,
            Object,
            Texture,
        }

        private class ValueChangedEventArgs : EventArgs
        {
            public GUIValue value;

            public ValueChangedEventArgs(GUIValue value)
            {
                this.value = value;
            }
        }

        private class GUIValue
        {
            public ValueType valueType;

            public object Data;

            public string Label;

            public GUIValue(ValueType valueType, object Data, string Label)
            {
                this.valueType = valueType;
                this.Data = Data;
                this.Label = Label;
            }
        }
    }
}
