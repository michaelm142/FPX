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
        public static Control TargetControl { get; set; }

        private static List<GUIValue> Values = new List<GUIValue>();

        public static void Begin()
        {
            TargetControl.Controls.Clear();
        }

        public static void End()
        {
            TargetControl.Controls.Clear();
            for (int i = 0; i < Values.Count; i++)
            {
                var value = Values[i];
                int y = i * 10;

                Label label = new Label();
                label.AutoSize = true;
                label.Location = new Point(0, y);
                label.Name = "GUI Label";
                label.Text = value.Label;

                TextBox valueBox = new TextBox();
                valueBox.Location = new Point(TargetControl.Width / 2, y);
                valueBox.Name = "GUI Value Box";
                valueBox.Size = new Size(100, 20);
                valueBox.TabIndex = 1;

                TargetControl.Controls.Add(label);
                TargetControl.Controls.Add(valueBox);
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

            return value;
        }

        public enum ValueType
        {
            Integer,
            Float,
            String,
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
