﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

using Point = System.Drawing.Point;

namespace FPX.Editor
{
    public partial class VectorEditor : UserControl
    {
        public float xValue
        {
            get
            {
                float val = 0.0f;
                if (float.TryParse(posXBox.Text, out val))
                    return val;

                return 0.0f;
            }

            set
            {
                posXBox.Text = Math.Round(value, 2).ToString();
            }
        }

        public float yValue
        {
            get
            {
                float val = 0.0f;
                if (float.TryParse(posYBox.Text, out val))
                    return val;

                return 0.0f;
            }

            set
            {
                posYBox.Text = Math.Round(value, 2).ToString();
            }
        }

        public float zValue
        {
            get
            {
                float val = 0.0f;
                if (float.TryParse(posZBox.Text, out val))
                    return val;

                return 0.0f;
            }

            set
            {
                posZBox.Text = Math.Round(value, 2).ToString();
            }
        }

        public Vector3 Value
        {
            get { return new Vector3(xValue, yValue, zValue); }

            set
            {
                xValue = value.X;
                yValue = value.Y;
                zValue = value.Z;
            }
        }

        public string VectorName
        {
            get { return Title.Text; }

            set { Title.Text = value; }
        }

        private bool isProperty;

        private Point mousePointPrev;

        private string memberName;

        private FPX.Component component;

        private VectorEditor()
        {
            InitializeComponent();
        }

        public VectorEditor(FPX.Component component, string memberName)
        {
            InitializeComponent();

            this.component = component;
            this.memberName = memberName;
            if (component.GetType().GetFields().ToList().Find(f => f.Name == memberName) == null)
            {
                if (component.GetType().GetProperties().ToList().Find(p => p.Name == memberName) != null)
                    isProperty = true;
                else
                    throw new InvalidOperationException(string.Format("Component {0} does not contain a field or property named {1}", component.GetType().ToString(), memberName));
            }
        }

        private void ValueScroll(object sender, MouseEventArgs e)
        {
            Label l = sender as Label;
            string valueName = l.Text;
            int delta_x = e.Location.X - mousePointPrev.X;
            if (valueName.IndexOf("X") != -1)
                xValue += delta_x;
            else if (valueName.IndexOf("Y") != -1)
                yValue += delta_x;
            else if (valueName.IndexOf("Z") != -1)
                zValue += delta_x;
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            if (isProperty)
                component.GetType().GetProperty(memberName).GetSetMethod().Invoke(component, new object[] { Value });
            else
                component.GetType().GetField(memberName).SetValue(component, Value);
        }

        private void ValueMouseHover(object sender, MouseEventArgs e)
        {
            mousePointPrev = e.Location;
        }

        public static int DefaultLayoutHeight
        {
            get
            {
                VectorEditor v = new VectorEditor();
                return v.Height;
            }
        }
    }
}
