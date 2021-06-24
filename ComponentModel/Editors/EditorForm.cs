using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPX.Editor
{
    public class EditorForm : Dockable, IUpdateable
    {
        public bool isCollapsed { get; set; }
        public bool Enabled { get; set; } = true;

        public int UpdateOrder { get; set; }

        // size of resizing handles
        public const float ResizeHandleDim = 5.0f;
        // width of docking area
        const float DockingDim = 3.0f;
        const float BorderTop = 0.0f;

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public Rect bounds
        {
            get { return (transform as RectTransform).rect; }

            set { (transform as RectTransform).rect = value; }
        }
        private Rect movingBounds { get { return new Rect(bounds.X, bounds.Y, bounds.Width, 25.0f); } }

        private Rect? dockingBounds;

        private Vector2? pickupOffset;

        private Handler resizingFunction;
        public FormClosingHandler Closing;

        private Texture2D nubDownTexture;
        private Texture2D nubUpTexture;
        private Texture2D closeButtonTexture;

        public List<GameObject> Content = new List<GameObject>();

        private GameObject transformObject;
        public override Transform transform
        {
            get { return transformObject.GetComponent<RectTransform>() as Transform; }
        }

        public override void Initialize()
        {
            transformObject = new GameObject();
            transformObject.AddComponent<RectTransform>();
            Destroy(transformObject.transform);
            Content.Add(transformObject);

            bounds = new Rect(0, 0, 100.0f, 100.0f);

            nubDownTexture = GameCore.content.Load<Texture2D>("Textures/UINubDown");
            nubUpTexture = GameCore.content.Load<Texture2D>("Textures/UINubUp");
            closeButtonTexture = GameCore.content.Load<Texture2D>("Textures/UICloseButton");

            var backPanelObject = new GameObject();
            Destroy(backPanelObject.transform);
            backPanelObject.AddComponent<RectTransform>();
            backPanelObject.transform.parent = transform;
            

            Image backPanelImage = backPanelObject.AddComponent<Image>();
            backPanelImage.image = Editor.uiPannelTexture;
            backPanelImage.color = Color.White;
            backPanelImage.GetComponent<RectTransform>().anchorMax = Vector2.One.ToVector3();
            backPanelImage.Initialize();

            Content.Add(backPanelObject);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (pickupOffset != null)
            {
                if (!Input.GetMouseButton(0))
                {
                    pickupOffset = null;
                    #region Docking
                    if (MathHelper.Distance(Input.mousePosition.Y, BorderTop) < DockingDim)
                    {
                        Dock(DockStyle.Top);
                        dockingBounds = null;
                    }
                    if (MathHelper.Distance(Input.mousePosition.X, 0.0f) < DockingDim)
                    {
                        Dock(DockStyle.Left);
                        dockingBounds = null;
                    }
                    if (MathHelper.Distance(Input.mousePosition.Y, Screen.Height) < DockingDim)
                    {
                        Dock(DockStyle.Bottom);
                        dockingBounds = null;
                    }
                    if (MathHelper.Distance(Input.mousePosition.X, Screen.Width) < DockingDim)
                    {
                        Dock(DockStyle.Right);
                        dockingBounds = null;
                    }
                    #endregion
                    #region Expand/Collapse
                    var nubLocation = movingBounds.Location;
                    if (new Rect(nubLocation.X, nubLocation.Y, 25.0f, 25.0f).Contains(Input.mousePosition))
                        isCollapsed = !isCollapsed;
                    #endregion
                }
                else
                {
                    Rect b = bounds;
                    b.Location = Input.mousePosition + (Vector2)pickupOffset;
                    bounds = b;
                    if (dockingBounds == null)
                    {
                        if (MathHelper.Distance(Input.mousePosition.Y, BorderTop) < DockingDim) // begin dock top
                            dockingBounds = new Rect(0, 0, Screen.Width, Screen.Height * 0.2f);
                        if (MathHelper.Distance(Input.mousePosition.X, 0.0f) < DockingDim) // begin dock left
                            dockingBounds = new Rect(0, 0, Screen.Width * 0.2f, Screen.Height);
                        if (MathHelper.Distance(Input.mousePosition.Y, Screen.Height) < DockingDim)// begin dock bottom
                            dockingBounds = new Rect(0, Screen.Height - Screen.Height * 0.2f, Screen.Width, Screen.Height * 0.2f);
                        if (MathHelper.Distance(Input.mousePosition.X, Screen.Width) < DockingDim) // begin dock right
                            dockingBounds = new Rect(Screen.Width * 0.8f, 0.0f, Screen.Width * 0.2f, Screen.Height);
                    }
                    else
                    {
                        if (MathHelper.Distance(Input.mousePosition.Y, BorderTop) > DockingDim &&
                         MathHelper.Distance(Input.mousePosition.X, 0.0f) > DockingDim && // begin dock left
                             MathHelper.Distance(Input.mousePosition.Y, Screen.Height) > DockingDim &&// begin dock bottom
                                 MathHelper.Distance(Input.mousePosition.X, Screen.Width) > DockingDim) // begin dock right
                            dockingBounds = null;
                    }
                }

            }
            else if (resizingFunction == null)
            {
                if (Input.GetMouseButton(0) && movingBounds.Contains(Input.mousePosition))
                {
                    Dock(DockStyle.None);
                    pickupOffset = bounds.Location - Input.mousePosition;
                }

                #region Resizing
                // Resize Left Side
                if (MathHelper.Distance(Input.mousePosition.X, bounds.Left) < ResizeHandleDim && Input.mousePosition.Y > bounds.Top && Input.mousePosition.Y < bounds.Bottom)
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeWE;
                    if (Input.GetMouseButton(0))
                        resizingFunction = ResizeLeft;

                }

                // Resize Right Side
                if (MathHelper.Distance(Input.mousePosition.X, bounds.Right) < ResizeHandleDim && Input.mousePosition.Y > bounds.Top && Input.mousePosition.Y < bounds.Bottom)
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeWE;
                    if (Input.GetMouseButton(0))
                        resizingFunction = ResizeRight;

                }

                // Resize right and bottom side
                if (MathHelper.Distance(Input.mousePosition.Y, bounds.Bottom) < ResizeHandleDim && MathHelper.Distance(Input.mousePosition.X, bounds.Right) < ResizeHandleDim)
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeNWSE;
                    if (Input.GetMouseButton(0))
                        resizingFunction = ResizeBottomRight;

                }

                // Resize bottom side
                if (MathHelper.Distance(Input.mousePosition.Y, bounds.Bottom) < ResizeHandleDim && Input.mousePosition.X > bounds.Left && Input.mousePosition.X < bounds.Right)
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeNS;
                    if (Input.GetMouseButton(0))
                        resizingFunction = ResizeBottom;

                }
                #endregion
            }

            Content.FindAll(c => c.Enabled).ForEach(c => c.BroadcastMessage("Update", gameTime));

            UpdateDocking();

            Debug.Log("Bounds:{0}", bounds);

            resizingFunction?.DynamicInvoke();
        }

        #region Resizing functions
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
        #endregion

        public override void Draw(GameTime gameTime)
        {
            if (!isCollapsed && Content != null)
            {
                Content.Sort((a, b) => a.DrawOrder < b.DrawOrder ? -1 : 1);
                Content.FindAll(c => c.Visible).ForEach(c => c.Draw(gameTime));
            }

            Color barColor = Color.DarkGray;
            GameCore.spriteBatch.Draw(Material.DefaultTexture, movingBounds, barColor);

            Vector2 nubLocation = movingBounds.Location;
            var nubTexture = isCollapsed ? nubUpTexture : nubDownTexture;
            GameCore.spriteBatch.Draw(nubTexture, new Rect(nubLocation.X, nubLocation.Y, 25.0f, 25.0f), barColor);
            GameCore.spriteBatch.Draw(closeButtonTexture, bounds.Location + Vector2.UnitX * (bounds.Width - 25.0f), barColor);

            if (dockingBounds != null)
                GameCore.spriteBatch.Draw(Material.DefaultTexture, dockingBounds.Value, Color.CornflowerBlue * 0.2f);
        }
    }

    public delegate void FormClosingHandler(EditorForm form);
}
