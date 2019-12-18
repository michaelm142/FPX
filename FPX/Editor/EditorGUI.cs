using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace FPX.Editor
{
    public static class EditorGUI
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

        public static void Begin(ComponentModel.GameObject gameObject)
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
            TargetControl.Invalidate(true);
        }

        public static void BeginControl(ComponentModel.Component component)
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
            p.Size = new Size(TargetControl.Width, 100);
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
            TargetControl.Height = Microsoft.Xna.Framework.MathHelper.Clamp(ControlHeight * Values.Count + 30, 10, int.MaxValue);
            for (int i = 0; i < Values.Count; i++)
            {
                var value = Values[i];
                int y = i * ControlHeight + 30;

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
                valueBox.Tag = Values[i];

                TargetControl.Controls.Add(label);
                TargetControl.Controls.Add(valueBox);
            }

            Values = new List<GUIValue>();
            TargetControl.Invalidate(true);

            TargetControl = TargetControl.Parent;
        }

        private static void UpdateValues()
        {
            foreach (Control c in TargetControl.Controls)
            {
                if (c is Panel && c.Tag != null)
                {
                    var comp = c.Tag as ComponentModel.Component;
                    var componentType = comp.GetType();
                    foreach (Control pannelControl in c.Controls)
                    {
                        TextBox box = pannelControl as TextBox;
                        if (box != null)
                        {
                            GUIValue value = box.Tag as GUIValue;
                            componentType.InvokeMember(value.Label, BindingFlags.SetField, null, comp, new object[] { value.Data });
                        }
                    }
                }
            }

            HierarchyWindow.instance.Invalidate();
        }

        private static void ValueBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.BackColor = Color.LightGray;
            GUIValue value = textBox.Tag as GUIValue;
            string enteredData = textBox.Text;

            switch (value.valueType)
            {
                case ValueType.Float:
                    float floatValue = 0.0f;
                    if (float.TryParse(enteredData, out floatValue))
                        value.Data = floatValue;
                    else
                        textBox.BackColor = Color.Red;
                    break;
                case ValueType.Integer:
                    int integerValue = 0;
                    if (int.TryParse(enteredData, out integerValue))
                        value.Data = integerValue;
                    else
                        textBox.BackColor = Color.Red;
                    break;
                case ValueType.String:
                    value.Data = enteredData;
                    break;
            }
        }

        public static int IntField(string label, int value)
        {
            var val = Values.Find(v => v.Label == label);
            if (val != null)
            {
                int i = (int)val.Data;
                val.Data = value;

                return i;
            }

            val = new GUIValue(ValueType.Integer, value, label);
            Values.Add(val);

            return value;
        }

        public static float FloatField(string label, float value)
        {
            var val = Values.Find(v => v.Label == label);
            if (val != null)
            {
                float f = (float)val.Data;
                val.Data = value;

                return f;
            }

            val = new GUIValue(ValueType.Integer, value, label);
            Values.Add(val);

            return value;
        }

        public static string StringField(string label, string value)
        {
            var val = Values.Find(v => v.Label == label);
            if (val != null)
            {
                string f = val.Data as string;
                val.Data = value;

                return f;
            }

            val = new GUIValue(ValueType.Integer, value, label);
            Values.Add(val);

            return value;
        }

        public enum ValueType
        {
            Integer,
            Float,
            String,
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
