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

        public Dockable parent
        {
            get
            {
                if (transform.parent == null)
                    return null;
                return transform.parent.GetComponent<Dockable>();
            }

            set
            {
                if (value == null)
                    transform.parent = null;
                else
                    transform.parent = value.transform;
            }
        }

        private Rect? undockedBounds;

        public void Dock(Dockable parent, DockStyle DockStyle)
        {
            this.DockStyle = DockStyle;

            var anchorMin = (transform as RectTransform).anchorMin;
            var anchorMax = (transform as RectTransform).anchorMax;

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
                    anchorMin = new Vector3(0.0f, 0.8f, 0.0f);
                    anchorMax = Vector2.One.ToVector3();
                    break;
                case DockStyle.Top:
                    anchorMin = Vector3.Zero;
                    anchorMax.Y = 0.2f;
                    anchorMax.X = 1.0f;
                    break;
                case DockStyle.Left:
                    anchorMin = Vector3.Zero;
                    anchorMax.Y = 1.0f;
                    anchorMax.X = 0.2f;
                    break;
                case DockStyle.Right:
                    anchorMin.X = 0.8f;
                    anchorMin.Y = 0.0f;
                    anchorMax = Vector2.One.ToVector3();
                    break;
            }
            this.parent = parent;

            if (DockStyle != DockStyle.None)
                undockedBounds = (transform as RectTransform).rect;
            (transform as RectTransform).anchorMin = anchorMin;
            (transform as RectTransform).anchorMax = anchorMax;
        }

        protected void UpdateDocking()
        {
            return;

            var anchorMin = (transform as RectTransform).anchorMin;
            var anchorMax = (transform as RectTransform).anchorMax;

            Vector3 parentMin = (parent.transform as RectTransform).anchorMin;
            Vector3 parentMax = (parent.transform as RectTransform).anchorMax;

            switch (DockStyle)
            {
                case DockStyle.Top:
                    anchorMin = parentMin;
                    anchorMax.X = parentMax.X;
                    break;
                case DockStyle.Left:
                    anchorMin = (parent.transform as RectTransform).anchorMin;
                    anchorMax.Y = (parent.transform as RectTransform).anchorMax.Y;
                    break;
                case DockStyle.Bottom:
                    anchorMin.X = parentMin.X;
                    anchorMax = parentMax;
                    break;
                case DockStyle.Right:
                    anchorMin.Y = parentMin.Y;
                    anchorMax = parentMax;
                    break;
            }

            (transform as RectTransform).anchorMin = anchorMin;
            (transform as RectTransform).anchorMax = anchorMax;
        }
    }

    internal class BaseDockable : Dockable
    {
        public override void Draw(GameTime gameTime) { }

        public override void Initialize() { }

        public void Update(GameTime gameTime)
        {
            (transform as RectTransform).anchorMin = Vector3.Zero;
            (transform as RectTransform).anchorMax = new Vector3(Screen.Width, Screen.Height, 0.0f);
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
