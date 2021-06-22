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
                return MathHelper.Lerp(parent.anchorMin.X, parent.anchorMax.X, _anchorMin.X) * Vector3.Right +
                    MathHelper.Lerp(parent.anchorMin.Y, parent.anchorMax.Y, _anchorMin.Y) * Vector3.Up;
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
                return MathHelper.Lerp(parent.anchorMin.X, parent.anchorMax.X, _anchorMax.X) * Vector3.Right +
                    MathHelper.Lerp(parent.anchorMin.Y, parent.anchorMax.Y, _anchorMax.Y) * Vector3.Up;
            }
            set { _anchorMax = parent == null ? value : Vector3.Clamp(value, Vector3.Zero, Vector3.One); }
        }

        public Rect rect
        {
            get { return new Rect(anchorMin, anchorMax); }
        }

        private RectTransform _parent;
        public new RectTransform parent
        {
            get { return _parent; }
            set
            {
                if (value == this)
                    return;

                _parent = value;
            }
        }
    }
}
