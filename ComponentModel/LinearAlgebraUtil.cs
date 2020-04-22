using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;

namespace FPX
{
    public static class LinearAlgebraUtil
    {
        public const float Rad2Deg = 180.0f / (float)Math.PI;
        public const float Deg2Rad = (float)Math.PI / 180.0f;


        public static Vector4 ToVector4(this Vector3 v)
        {
            return new Vector4(v, 0.0f);
        }

        public static bool isEpsilon(float f)
        {
            if (Math.Abs(f) < 0.01f || float.IsNaN(f) || float.IsInfinity(f))
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
        public static void SetRow(this Matrix m, int index, Vector4 row)
        {
            unsafe
            {
                *((Vector4*)(&m) + index) = row;
            }
        }
        public static void SetRow(this Matrix m, int index, Vector3 row)
        {
            unsafe
            {
                *((Vector4*)(&m) + index) = row.ToVector4();
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

        public static Vector3 Normalized(this Vector3 v)
        {
            Vector3 _v = v;
            _v.Normalize();
            return _v;
        }

        public static Vector3 Vector3FromXml(XmlElement element)
        {
            if (element == null)
                return Vector3.Zero;
            Vector3 outval = Vector3.Zero;

            var xAttr = element.Attributes["X"];
            var yAttr = element.Attributes["Y"];
            var zAttr = element.Attributes["Z"];

            if (xAttr != null)
                outval.X = float.Parse(xAttr.Value);
            if (yAttr != null)
                outval.Y = float.Parse(yAttr.Value);
            if (zAttr != null)
                outval.Z = float.Parse(zAttr.Value);

            return outval;
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
            Matrix m = Matrix.CreateFromQuaternion(q);

            float pitch = (float)(Math.Acos(m.M11) * 2.0);
            float yaw = (float)(Math.Acos(m.M12) * 2.0);
            float roll = (float)(Math.Acos(m.M13) * 2.0);

            return new Vector3(pitch * Rad2Deg, yaw * Rad2Deg, roll * Rad2Deg);
        }

        public static void SetEulerAngles(this Quaternion q, Vector3 euler)
        {
            q = Quaternion.CreateFromYawPitchRoll(euler.Y * Deg2Rad, euler.X * Deg2Rad, euler.Z * Deg2Rad);
        }

        public static Quaternion QuaternionFromEuler(Vector3 euler)
        {
            return Quaternion.CreateFromYawPitchRoll(euler.Y * Deg2Rad, euler.X * Deg2Rad, euler.Z * Deg2Rad);
        }

        public static Quaternion ComponentMultiply(Quaternion q, float f)
        {
            q.X *= f;
            q.Y *= f;
            q.Z *= f;
            q.W *= f;

            return q;
        }
    }
}