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

        public Rectangle bounds { get; set; }
        private Rectangle movingBounds { get { return new Rectangle(bounds.Location, new Point(bounds.Width, Math.Min(25, bounds.Height))); } }

        protected bool pickedUp;

        private Vector2 pickupOffset;
        private Vector2 mousePrev;

        private ResizeHandler resizingFunction;

        public const float ResizeHandleDim = 5.0f;

        public virtual void Initialize()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
            if (pickedUp)
            {
                if (!Input.GetMouseButton(0))
                    pickedUp = false;

                Rectangle b = bounds;
                b.Location = (Input.mousePosition + pickupOffset).ToPoint();
                bounds = b;
            }
            else if (resizingFunction == null)
            {
                if (Input.GetMouseButton(0) && movingBounds.Contains(Input.mousePosition))
                {
                    pickedUp = true;
                    pickupOffset = bounds.Location.ToVector2() - Input.mousePosition;
                }

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
            mousePrev = Input.mousePosition;
        }

        private void ResizeLeft()
        {
            Rectangle r = bounds;
            int newWidth = bounds.Right - (int)Input.mousePosition.X;
            r.X = (int)Input.mousePosition.X;
            r.Width = (int)newWidth;
            bounds = r;
            if (!Input.GetMouseButton(0))
                resizingFunction = null;
        }

        private void ResizeRight()
        {
            Rectangle r = bounds;
            float newWidth = Input.mousePosition.X - bounds.Left;
            r.Width = (int)newWidth;
            bounds = r;
            if (!Input.GetMouseButton(0))
                resizingFunction = null;
        }

        private void ResizeBottomRight()
        {
            Rectangle r = bounds;
            int newWidth = (int)Input.mousePosition.X - bounds.Left;
            float newHeight = Input.mousePosition.Y - bounds.Top;
            r.Height = (int)newHeight;
            r.Width = (int)newWidth;
            bounds = r;
            if (!Input.GetMouseButton(0))
                resizingFunction = null;
        }

        private void ResizeBottom()
        {
            Rectangle r = bounds;
            float newHeight = Input.mousePosition.Y - bounds.Top;
            r.Height = (int)newHeight;
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
