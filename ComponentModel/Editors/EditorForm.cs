using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPX.Editor
{
    public class EditorForm : IGameComponent, IDrawable, IUpdateable
    {
        public int DrawOrder { get; set; }

        public bool Visible { get; set; } = true;

        public bool Enabled { get; set; } = true;

        public int UpdateOrder { get; set; }

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public Rect bounds
        {
            get { return transform.rect; }

            set
            {
                transform.anchorMin = value.Location.ToVector3();
                transform.anchorMax = (value.Location + Vector2.UnitX * value.Width + Vector2.UnitY * value.Height).ToVector3();
            }
        }
        private Rect movingBounds { get { return new Rect(bounds.X, bounds.Y, bounds.Width, Math.Min(25, bounds.Height)); } }

        private Vector2? pickupOffset;

        private ResizeHandler resizingFunction;

        private GameObject transformObject;
        public RectTransform transform
        {
            get { return transformObject.GetComponent<RectTransform>(); }
        }

        public const float ResizeHandleDim = 5.0f;

        public virtual void Initialize()
        {
            transformObject = new GameObject();
            transformObject.AddComponent<RectTransform>();
        }

        public virtual void Update(GameTime gameTime)
        {
            if (pickupOffset != null)
            {
                if (!Input.GetMouseButton(0))
                    pickupOffset = null;
                else
                {
                    Rect b = bounds;
                    b.Location = Input.mousePosition + (Vector2)pickupOffset;
                    bounds = b;
                }
            }
            else if (resizingFunction == null)
            {
                if (Input.GetMouseButton(0) && movingBounds.Contains(Input.mousePosition))
                    pickupOffset = bounds.Location - Input.mousePosition;

                if (MathHelper.Distance(Input.mousePosition.X, bounds.Left) < ResizeHandleDim && Input.mousePosition.Y > bounds.Top && Input.mousePosition.Y < bounds.Bottom)
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeWE;
                    if (Input.GetMouseButton(0))
                        resizingFunction = ResizeLeft;

                }

                if (MathHelper.Distance(Input.mousePosition.X, bounds.Right) < ResizeHandleDim && Input.mousePosition.Y > bounds.Top && Input.mousePosition.Y < bounds.Bottom)
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeWE;
                    if (Input.GetMouseButton(0))
                        resizingFunction = ResizeRight;

                }

                if (MathHelper.Distance(Input.mousePosition.Y, bounds.Bottom) < ResizeHandleDim && MathHelper.Distance(Input.mousePosition.X, bounds.Right) < ResizeHandleDim)
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeNWSE;
                    if (Input.GetMouseButton(0))
                        resizingFunction = ResizeBottomRight;

                }

                if (MathHelper.Distance(Input.mousePosition.Y, bounds.Bottom) < ResizeHandleDim && Input.mousePosition.X > bounds.Left && Input.mousePosition.X < bounds.Right)
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeNS;
                    if (Input.GetMouseButton(0))
                        resizingFunction = ResizeBottom;

                }
            }

            resizingFunction?.DynamicInvoke();
        }

        private void ResizeLeft()
        {
            Rect r = bounds;
            float newWidth = bounds.Right - Input.mousePosition.X;
            r.X = Input.mousePosition.X;
            r.Width = newWidth;
            bounds = r;
            if (!Input.GetMouseButton(0))
                resizingFunction = null;
        }

        private void ResizeRight()
        {
            Rect r = bounds;
            float newWidth = Input.mousePosition.X - bounds.Left;
            r.Width = newWidth;
            bounds = r;
            if (!Input.GetMouseButton(0))
                resizingFunction = null;
        }

        private void ResizeBottomRight()
        {
            Rect r = bounds;
            float newWidth = Input.mousePosition.X - bounds.Left;
            float newHeight = Input.mousePosition.Y - bounds.Top;
            r.Height = newHeight;
            r.Width = newWidth;
            bounds = r;
            if (!Input.GetMouseButton(0))
                resizingFunction = null;
        }

        private void ResizeBottom()
        {
            Rect r = bounds;
            float newHeight = Input.mousePosition.Y - bounds.Top;
            r.Height = newHeight;
            bounds = r;
            if (!Input.GetMouseButton(0))
                resizingFunction = null;
        }

        public virtual void Draw(GameTime gameTime)
        {
            GameCore.spriteBatch.Draw(Editor.uiPannelTexture, bounds, Color.White);
            GameCore.spriteBatch.Draw(Material.DefaultTexture, movingBounds, Color.DarkGray);
        }

        private delegate void ResizeHandler();
    }
}
