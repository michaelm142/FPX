using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FPX
{
    public class RectTransform : Transform
    {
        private Vector3 _anchorMin;
        private Vector3 _anchorMax;
        public Vector3 anchorMin
        {
            get
            {
                if (parent == null)
                    return _anchorMin;

                // TODO: fix the recursive nightmare this creates
                return MathHelper.Lerp((parent as RectTransform).anchorMin.X, (parent as RectTransform).anchorMax.X, _anchorMin.X) * Vector3.Right +
                    MathHelper.Lerp((parent as RectTransform).anchorMin.Y, (parent as RectTransform).anchorMax.Y, _anchorMin.Y) * Vector3.Up;
            }
            set { _anchorMin = parent == null ? value : Vector3.Clamp(value, Vector3.Zero, Vector3.One); }
        }
        public Vector3 anchorMax
        {
            get
            {
                if (parent == null)
                    return _anchorMax;

                // TODO: fix the recursive nightmare this creates
                return MathHelper.Lerp((parent as RectTransform).anchorMin.X, (parent as RectTransform).anchorMax.X, _anchorMax.X) * Vector3.Right +
                    MathHelper.Lerp((parent as RectTransform).anchorMin.Y, (parent as RectTransform).anchorMax.Y, _anchorMax.Y) * Vector3.Up;
            }
            set { _anchorMax = parent == null ? value : Vector3.Clamp(value, Vector3.Zero, Vector3.One); }
        }
        public float BorderLeft
        {
            get { return transform.parent == null ? (transform as RectTransform).anchorMin.X : MathHelper.Lerp(parentRect.BorderLeft, parentRect.BorderRight, _anchorMin.X); }
        }
        public float BorderRight
        {
            get { return transform.parent == null ? (transform as RectTransform).anchorMax.X : MathHelper.Lerp(parentRect.BorderLeft, parentRect.BorderRight, _anchorMax.X); }
        }
        public float BorderTop
        {
            get { return transform.parent == null ? (transform as RectTransform).anchorMin.Y : MathHelper.Lerp(parentRect.BorderTop, parentRect.BorderBottom, _anchorMin.Y); }
        }
        public float BorderBottom
        {
            get { return transform.parent == null ? (transform as RectTransform).anchorMax.Y : MathHelper.Lerp(parentRect.BorderTop, parentRect.BorderBottom, _anchorMax.Y); }
        }

        internal RectTransform parentRect
        {
            get { return transform.parent as RectTransform; }
        }

        public Rect rect
        {
            get { return new Rect(anchorMin, anchorMax); }

            set
            {
                anchorMin = value.Location.ToVector3();
                anchorMax = (value.Location + Vector2.UnitX * value.Width + Vector2.UnitY * value.Height).ToVector3();
            }
        }
    }
}
