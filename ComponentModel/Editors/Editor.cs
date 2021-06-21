using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows;
using MonoGame.UI.Forms;

namespace FPX.Editor
{
    public class Editor : IGameComponent, IUpdateable, IDrawable
    {
        public List<IGameComponent> components { get; private set; } = new List<IGameComponent>();

        public int DrawOrder => 0;

        public bool Visible { get; set; } = true;

        public int UpdateOrder => 0;

        public bool Enabled  { get; set; } = true;

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        internal static Texture2D uiPannelTexture;

        public void Initialize()
        {
            Graphics.instance.Visible = false;

            uiPannelTexture = GameCore.content.Load<Texture2D>("Textures/UIPanel");
            components.ForEach(c => c.Initialize());
        }
        public void Draw(GameTime gameTime)
        {
            var drawables = components.FindAll(c => c is IDrawable).ConvertAll<IDrawable>(delegate (IGameComponent c) { return c as IDrawable; });
            drawables.Sort(delegate (IDrawable a, IDrawable b)
            {
                if (a.DrawOrder > b.DrawOrder)
                    return 1;

                return -1;
            });
            drawables.ForEach(d => d.Draw(gameTime));
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
