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
using XnaColor = Microsoft.Xna.Framework.Color;

namespace FPX.Editor
{
    public static partial class EditorGUI
    {

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

            val = new GUIValue(ValueType.Float, value, label);
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

        public static Vector3 Vector3Field(string label, Vector3 value)
        {
            var val = Values.Find(v => v.Label == label);
            if (val != null)
            {
                Vector3 v = (Vector3)val.Data;
                val.Data = value;

                return v;
            }

            val = new GUIValue(ValueType.Vector3, value, label);
            Values.Add(val);

            return value;
        }

        public static Quaternion QuaternionField(string label, Quaternion value)
        {
            var val = Values.Find(v => v.Label == label);
            if (val != null)
            {
                Quaternion v = (Quaternion)val.Data;
                val.Data = value;

                return v;
            }

            val = new GUIValue(ValueType.Quaternion, value, label);
            Values.Add(val);

            return value;
        }

        public static XnaColor ColorField(string label, XnaColor value)
        {
            var val = Values.Find(v => v.Label == label);
            if (val != null)
            {
                XnaColor v = (XnaColor)val.Data;
                val.Data = value;

                return v;
            }

            val = new GUIValue(ValueType.Color, value, label);
            Values.Add(val);

            return value;
        }

        public static Enum EnumField(string label, Enum value)
        {
            var val = Values.Find(v => v.Label == label);
            if (val != null)
            {
                Enum v = (Enum)val.Data;
                val.Data = value;

                return v;
            }

            val = new GUIValue(ValueType.Enum, value, label);
            Values.Add(val);

            return value;
        }

        public static object ObjectField(string label, object value)
        {
            var val = Values.Find(v => v.Label == label);
            if (val != null)
            {
                object v = (object)val.Data;
                val.Data = value;

                return v;
            }

            val = new GUIValue(ValueType.Object, value, label);
            Values.Add(val);

            return value;
        }

        public static Microsoft.Xna.Framework.Graphics.Texture2D TextureField(string label, Microsoft.Xna.Framework.Graphics.Texture2D value)
        {
            var val = Values.Find(v => v.Label == label);
            if (val != null)
            {
                Microsoft.Xna.Framework.Graphics.Texture2D v = (Microsoft.Xna.Framework.Graphics.Texture2D)val.Data;
                val.Data = value;

                return v;
            }

            val = new GUIValue(ValueType.Texture, value, label);
            Values.Add(val);

            return value;
        }
    }
}
