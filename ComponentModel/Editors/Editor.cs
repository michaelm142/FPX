using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using MonoGame.UI.Forms;

namespace FPX.Editor
{
    public class Editor : IGameComponent, IUpdateable, IDrawable
    {
        static Editor instance;
        public List<GameObject> components { get; private set; } = new List<GameObject>();
        public static List<GameObject> Components { get { return instance.components; } }

        public int DrawOrder => 0;

        public bool Visible { get; set; } = true;

        public int UpdateOrder => 0;

        public bool Enabled { get; set; } = true;

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        internal static Texture2D uiPannelTexture;

        internal static Color BackgroundColor = new Color(0.1f, 0.1f, 0.1f);

        public Editor()
        {
            if (instance != null)
                throw new InvalidOperationException("More than one instance of editor");
            instance = this;
        }

        ~Editor()
        {
            instance = null;
        }

        public void Initialize()
        {
            uiPannelTexture = GameCore.content.Load<Texture2D>("Textures/UIPanel");
            Graphics.instance.Visible = false;

            GameObject baseDockable = new GameObject("Base Dockable", typeof(RectTransform), typeof(BaseDockable));
            Component.Destroy(baseDockable.GetComponent<Transform>());

            components.Add(baseDockable);
        }

        public void Draw(GameTime gameTime)
        {
            GameCore.graphicsDevice.Clear(BackgroundColor);
            GameCore.spriteBatch.Begin(SpriteSortMode.Immediate);
            {
                components.ForEach(d => d.Draw(gameTime));
            }
            GameCore.spriteBatch.End();
        }

        bool mousePrev;
        public void Update(GameTime gameTime)
        {
            if (Input.GetMouseButton(1) && !mousePrev)
            {
                GameObject test = new GameObject("Test Form 1", typeof(RectTransform), typeof(EditorForm));
                Component.Destroy(test.transform);
                var testForm = test.GetComponent<EditorForm>();
                testForm.Initialize();
                testForm.bounds = new Rect(Input.mousePosition.X, Input.mousePosition.Y, 100.0f, 100.0f);
                components.Add(test);
            }
            components.ForEach(c => c.BroadcastMessage("Update", gameTime));
            components.RemoveAll(c => c.destroyed);

            mousePrev = Input.GetMouseButton(1);
        }
    }
}
