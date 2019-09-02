using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;

namespace ComponentModel
{
    public static class LinearAlgebraUtil
    {
        public static Vector4 ToVector4(this Vector3 v)
        {
            return new Vector4(v, 0.0f);
        }

        public static bool isEpsilon(float f)
        {
            if (Math.Abs(f) < 0.01f)
                return true;

            return false;
        }

        public static bool isEpsilon(Vector3 v)
        {
            if (v.Length() < 0.01f)
                return true;

            return false;
        }

        public static Vector4 GetRow(this Matrix m, int index)
        {
            unsafe
            {
                return *((Vector4*)(&m) + index);
            }
        }
        public static Matrix SetMatrixRow(Matrix m, int index, Vector4 value)
        {
            unsafe
            {
                *((Vector4*)(&m)) = value;
            }
            return m;
        }

        public static float GetRowColumn(this Matrix m, int column, int row)
        {
            unsafe
            {
                return *((float*)&m + column + row * 4);
            }
        }

        public static Matrix SetRowColumn(Matrix m, int column, int row, float value)
        {
            unsafe
            {
                float* pData = (float*)&m;
                *(pData + (column + row * 4)) = value;
            }
            return m;
        }

        public static float GetVectorIndex(Vector3 v, int index)
        {
            unsafe
            {
                return *(((float*)&v) + index);
            }
        }

        public static Vector3 SetVectorIndex(Vector3 v, int index, float value)
        {
            unsafe
            {
                float* pData = (float*)&v;
                *(pData + index) = value;
            }

            return v;
        }

        public static float GetIndex(this Vector3 v, int index)
        {
            unsafe
            {
                return *((float*)&v + index);
            }
        }

        public static float GetIndex(this Vector4 v, int index)
        {
            unsafe
            {
                return *((float*)&v + index);
            }
        }

        public static Vector2 ToVector2(this Vector3 v)
        {
            unsafe
            {
                return *((Vector2*)&v);
            }
        }

        public static Vector2 ToVector2(this Point p)
        {
            return new Vector2(p.X, p.Y);
        }

        public static Vector2 ToVector2(this Vector4 v)
        {
            unsafe
            {
                return *((Vector2*)&v);
            }
        }

        public static Vector3 ToVector3(this Vector2 v)
        {
            return new Vector3(v.X, v.Y, 0.0f);
        }

        public static Vector3 ToVector3(this Vector4 v)
        {
            unsafe
            {
                return *((Vector3*)&v);
            }
        }

        public static Vector3 Vector3FromXml(XmlElement element)
        {
            if (element == null)
                return Vector3.Zero;

            return new Vector3(float.Parse(element.Attributes["X"].Value),
                float.Parse(element.Attributes["Y"].Value),
                float.Parse(element.Attributes["Z"].Value));
        }

        public static Vector4 Vector4FromXml(XmlElement element)
        {
            if (element == null)
                return Vector4.Zero;

            if (element.Attributes["X"] != null)
                return new Vector4(float.Parse(element.Attributes["X"].Value),
                    float.Parse(element.Attributes["Y"].Value),
                    float.Parse(element.Attributes["Z"].Value),
                    float.Parse(element.Attributes["W"].Value));
            else
                return new Vector4(float.Parse(element.Attributes["R"].Value),
                                   float.Parse(element.Attributes["G"].Value),
                                   float.Parse(element.Attributes["B"].Value),
                                   float.Parse(element.Attributes["A"].Value));

        }

        public static XmlElement Vector3ToXml(XmlDocument doc, string name, Vector3 v)
        {
            var element = doc.CreateElement(name);

            XmlAttribute xAttr = doc.CreateAttribute("X");
            XmlAttribute yAttr = doc.CreateAttribute("Y");
            XmlAttribute zAttr = doc.CreateAttribute("Z");

            xAttr.Value = v.X.ToString();
            yAttr.Value = v.Y.ToString();
            zAttr.Value = v.Z.ToString();

            element.Attributes.Append(xAttr);
            element.Attributes.Append(yAttr);
            element.Attributes.Append(zAttr);

            return element;
        }

        public static XmlElement Vector4ToXml(XmlDocument doc, string name, Vector4 v)
        {
            var element = doc.CreateElement(name);

            XmlAttribute xAttr = doc.CreateAttribute("X");
            XmlAttribute yAttr = doc.CreateAttribute("Y");
            XmlAttribute zAttr = doc.CreateAttribute("Z");
            XmlAttribute wAttr = doc.CreateAttribute("W");

            xAttr.Value = v.X.ToString();
            yAttr.Value = v.Y.ToString();
            zAttr.Value = v.Z.ToString();
            wAttr.Value = v.W.ToString();

            element.Attributes.Append(xAttr);
            element.Attributes.Append(yAttr);
            element.Attributes.Append(zAttr);
            element.Attributes.Append(wAttr);

            return element;
        }

        public static XmlElement ColorToXml(XmlDocument doc, string name, Color v)
        {
            var element = doc.CreateElement(name);

            XmlAttribute rAttr = doc.CreateAttribute("R");
            XmlAttribute gAttr = doc.CreateAttribute("G");
            XmlAttribute bAttr = doc.CreateAttribute("B");
            XmlAttribute aAttr = doc.CreateAttribute("A");

            rAttr.Value = v.R.ToString();
            gAttr.Value = v.G.ToString();
            bAttr.Value = v.B.ToString();
            aAttr.Value = v.A.ToString();

            element.Attributes.Append(rAttr);
            element.Attributes.Append(gAttr);
            element.Attributes.Append(bAttr);
            element.Attributes.Append(aAttr);

            return element;
        }

        public static Quaternion EulerFromXml(System.Xml.XmlElement element)
        {
            Vector3 euler = Vector3FromXml(element);
            return Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(euler.Y), MathHelper.ToRadians(euler.X), MathHelper.ToRadians(euler.Z));
        }

        public static Quaternion GetRotation(this Matrix m)
        {
            Vector3 pos, scale;
            Quaternion outval;
            m.Decompose(out scale, out outval, out pos);

            return outval;
        }

        public static Vector3 GetEulerAngles(this Quaternion q)
        {
            float yaw, pitch, roll;
            // roll (x-axis rotation)
            float sinr_cosp = 2.0F * (q.W * q.X + q.Y * q.Z);
            float cosr_cosp = 1.0F - 2.0F * (q.X * q.X + q.Y * q.Y);
            roll = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch (y-axis rotation)
            float sinp = 2.0f * (q.X * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1.0f)
                pitch = (float)(Math.PI / 2.0f) * (sinp > 0.0f ? 1.0f : -1.0f);// copysign(Math.PI / 2, sinp); // use 90 degrees if out of range
            else
                pitch = (float)Math.Asin(sinp);

            // yaw (z-axis rotation)
            float siny_cosp = 2.0F * (q.W * q.Z + q.X * q.Y);
            float cosy_cosp = 1.0F - 2.0F * (q.Y * q.Y + q.Z * q.Z);
            yaw = (float)Math.Atan2(siny_cosp, cosy_cosp);

            return new Vector3(MathHelper.ToDegrees(pitch), MathHelper.ToDegrees(yaw), MathHelper.ToDegrees(roll));
        }

        public static void SetEulerAngles(this Quaternion q, Vector3 euler)
        {
            q = Quaternion.CreateFromYawPitchRoll(euler.Y, euler.X, euler.Z);
        }
    }
}