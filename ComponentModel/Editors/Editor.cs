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
        public List<UIElement> components { get; private set; } = new List<UIElement>();

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

        public void Initialize()
        {
            Graphics.instance.Visible = false;

            EditorForm test = new EditorForm();
            components.Add(test);

            uiPannelTexture = GameCore.content.Load<Texture2D>("Textures/UIPanel");
            components.ForEach(c => c.Initialize());
        }

        public void Draw(GameTime gameTime)
        {
            GameCore.graphicsDevice.Clear(BackgroundColor);
            GameCore.spriteBatch.Begin(SpriteSortMode.Immediate);
            {
                components.Sort(delegate (UIElement a, UIElement b)
                {
                    if (a.DrawOrder > b.DrawOrder)
                        return 1;

                    return -1;
                });
                components.ForEach(d => d.Draw(gameTime));
            }
            GameCore.spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var comp in components.FindAll(c => c is IUpdateable))
            {
                IUpdateable updateable = comp as IUpdateable;
                updateable.Update(gameTime);
            }
        }
    }
}
