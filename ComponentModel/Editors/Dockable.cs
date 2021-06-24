using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FPX.Editor
{
    public abstract class Dockable : UIElement
    {
        public virtual DockStyle DockStyle { get; private set; }

        private Rect? undockedBounds;

        public void Dock(DockStyle dockStyle)
        {
            DockStyle = dockStyle;
            switch (DockStyle)
            {
                case DockStyle.None:
                    if (undockedBounds != null)
                    {
                        Rect currentBounds = Rect.Empty;
                        currentBounds.Width = undockedBounds.Value.Width;
                        currentBounds.Height = undockedBounds.Value.Height;
                        currentBounds.Location = Input.mousePosition - Vector2.UnitX * (currentBounds.Width / 2.0f);
                        (transform as RectTransform).rect = currentBounds;
                        undockedBounds = null;
                    }
                    break;
                case DockStyle.Bottom:
                    var anchorMin = (transform as RectTransform).anchorMin;
                    anchorMin.Y = Screen.Height * 0.8f;
                    (transform as RectTransform).anchorMin = anchorMin;
                    goto case DockStyle.Right;
                case DockStyle.Top:
                case DockStyle.Left:
                case DockStyle.Right:
                    undockedBounds = (transform as RectTransform).rect;
                    break;
            }
        }

        protected void UpdateDocking()
        {
            var anchorMin = (transform as RectTransform).anchorMin;
            var anchorMax = (transform as RectTransform).anchorMax;

            switch (DockStyle)
            {
                case DockStyle.Top:
                    anchorMin = Vector3.Zero;
                    anchorMax.X = Screen.Width;
                    break;
                case DockStyle.Left:
                    anchorMin = Vector3.Zero;
                    anchorMax.Y = Screen.Height;
                    break;
                case DockStyle.Bottom:
                    anchorMin.X = 0.0f;
                    anchorMax.X = Screen.Width;
                    anchorMax.Y = Screen.Height;
                    break;
                case DockStyle.Right:
                    anchorMin.Y = 0.0f;
                    anchorMax.X = Screen.Width;
                    anchorMax.Y = Screen.Height;
                    break;
            }

            (transform as RectTransform).anchorMin = anchorMin;
            (transform as RectTransform).anchorMax = anchorMax;
        }
    }

    public enum DockStyle
    {
        None,
        Center,
        Top,
        Left,
        Right,
        Bottom
    }
}
